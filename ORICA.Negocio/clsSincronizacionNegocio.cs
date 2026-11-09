using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Sincronización entre Azure SQL (base principal) y la copia local (LocalDB).
    /// Cada 30 segundos, si se está sin conexión, intenta volver a Azure:
    ///   1) sube lo registrado sin internet, 2) pasa a trabajar con Azure, 3) renueva la copia local.
    /// La primera vez en una PC hay que elegir en la pantalla Sincronización qué base se queda.
    /// </summary>
    public static class clsSincronizacionNegocio
    {
        private const int IntervaloMs = 30000;

        private static readonly clsSincronizacionDatos _datos = new clsSincronizacionDatos();
        private static Timer _temporizador;
        private static int _trabajando;   // 1 mientras se sincroniza (evita dos a la vez)

        public static DateTime? UltimaSubida { get; private set; }

        /// <summary>Texto corto para las cabeceras de las pantallas.</summary>
        public static string TextoEstado
        {
            get
            {
                if (!clsConexion.TieneAzure) return "● Base local (sin Azure)";
                return clsConexion.EnLinea ? "● Azure conectado" : "● Sin conexión (local)";
            }
        }

        public static bool EnLinea { get { return clsConexion.EnLinea; } }

        /// <summary>true si hay Azure configurado pero ahora no responde (se trabaja con la copia local).</summary>
        public static bool SinConexion { get { return clsConexion.TieneAzure && !clsConexion.EnLinea; } }

        /// <summary>Se llama una vez al abrir la aplicación (Program.cs).</summary>
        public static void mtdIniciar()
        {
            try { _datos.mtdPrepararLocal(); }
            catch (Exception ex) { RegistrarError("No se pudo preparar la base local: " + ex.Message); }

            if (!clsConexion.TieneAzure) return;   // sin conexion.config se trabaja solo en local, como antes

            // Primer intento enseguida y luego cada 30 s.
            _temporizador = new Timer(_ => Revisar(), null, 0, IntervaloMs);
        }

        public static void mtdDetener()
        {
            if (_temporizador != null) _temporizador.Dispose();
        }

        // Lo que hace el temporizador: si estamos sin conexión, intenta volver.
        private static void Revisar()
        {
            if (clsConexion.EnLinea || clsConexion.SimularSinConexion) return;
            mtdSincronizar();
        }

        /// <summary>Sube los pendientes a Azure, pasa a modo en línea y renueva la copia local.</summary>
        public static Resultado mtdSincronizar()
        {
            if (Interlocked.Exchange(ref _trabajando, 1) == 1)
                return Resultado.Error("Ya hay una sincronización en curso.");

            try
            {
                if (_datos.mtdFechaCopia() == null)
                    return Resultado.Error("Esta PC nunca se sincronizó. En la pantalla Sincronización elija " +
                                           "si descargar los datos de Azure o subir los de esta PC.");

                // Se conecta antes del candado: si Azure tarda o no responde, la app sigue trabajando en local.
                using (SqlConnection azure = clsConexion.mtdAbrirAzure())
                lock (clsConexion.Candado)   // mientras tanto, nadie escribe en la base
                {
                    int subidos = _datos.mtdSubirPendientes(azure);
                    if (subidos > 0) UltimaSubida = DateTime.Now;

                    clsConexion.mtdPasarAModoAzure();   // desde aquí lo nuevo va a Azure
                    _datos.mtdDescargarDeAzure(azure);  // la copia local queda igual a Azure

                    return Resultado.Ok(subidos > 0
                        ? "Se subieron " + subidos + " registros a Azure y se actualizó la copia local."
                        : "Conectado a Azure. Copia local actualizada.", subidos);
                }
            }
            catch (Exception ex)
            {
                RegistrarError(ex.Message);
                return Resultado.Error("No se pudo sincronizar con Azure: " + ex.Message);
            }
            finally
            {
                Interlocked.Exchange(ref _trabajando, 0);
            }
        }

        /// <summary>Primera vez: esta PC se queda con los datos de Azure (se borra lo local).</summary>
        public static Resultado mtdDescargarDeAzure()
        {
            return CopiaCompleta(false);
        }

        /// <summary>Primera vez: Azure se queda con los datos de esta PC (se borra lo de Azure). Solo ADMIN.</summary>
        public static Resultado mtdSubirTodoAAzure()
        {
            if (!Sesion.Activa || Sesion.UsuarioActual.RolCodigo != "ADMIN")
                return Resultado.Error("Solo un administrador puede reemplazar los datos de Azure.");
            return CopiaCompleta(true);
        }

        private static Resultado CopiaCompleta(bool subir)
        {
            if (Interlocked.Exchange(ref _trabajando, 1) == 1)
                return Resultado.Error("Ya hay una sincronización en curso.");

            try
            {
                using (SqlConnection azure = clsConexion.mtdAbrirAzure())
                lock (clsConexion.Candado)
                {
                    if (!subir && _datos.mtdContarPendientes() > 0)
                        return Resultado.Error("Hay registros sin subir. Use \"Sincronizar ahora\" primero.");

                    if (subir) _datos.mtdSubirTodoAAzure(azure);
                    else _datos.mtdDescargarDeAzure(azure);

                    clsConexion.mtdPasarAModoAzure();
                    return Resultado.Ok(subir ? "Azure ahora tiene los datos de esta PC."
                                              : "Esta PC ahora tiene una copia de los datos de Azure.");
                }
            }
            catch (Exception ex)
            {
                RegistrarError(ex.Message);
                return Resultado.Error("No se pudo copiar: " + ex.Message);
            }
            finally
            {
                Interlocked.Exchange(ref _trabajando, 0);
            }
        }

        /// <summary>Para probar sin desconectar internet: true = trabajar como si no hubiera conexión.</summary>
        public static void mtdSimularSinConexion(bool activar)
        {
            clsConexion.SimularSinConexion = activar;
        }

        public static EstadoSincronizacion mtdEstado()
        {
            EstadoSincronizacion estado = new EstadoSincronizacion
            {
                TieneAzure = clsConexion.TieneAzure,
                EnLinea = clsConexion.EnLinea,
                Simulado = clsConexion.SimularSinConexion,
                UltimaSubida = UltimaSubida,
                UltimoError = clsConexion.UltimoError
            };

            // Si se está sincronizando no se espera (la pantalla no se congela): se muestra "Sincronizando".
            if (!Monitor.TryEnter(clsConexion.Candado)) { estado.Ocupado = true; return estado; }
            try
            {
                estado.FechaCopia = _datos.mtdFechaCopia();
                estado.Pendientes = _datos.mtdContarPendientes();
            }
            finally { Monitor.Exit(clsConexion.Candado); }
            return estado;
        }

        public static List<RegistroSincronizacion> mtdListarHistorial()
        {
            List<RegistroSincronizacion> lista = new List<RegistroSincronizacion>();
            foreach (DataRow r in _datos.mtdListarHistorial(100).Rows)
            {
                lista.Add(new RegistroSincronizacion
                {
                    Id = Convert.ToInt64(r["IdSincronizacion"]),
                    Tabla = clsLectura.mtdLeerTexto(r, "TipoEntidad"),
                    Detalle = clsLectura.mtdLeerTexto(r, "IdRegistroOrigen"),
                    Intentos = clsLectura.mtdLeerEntero(r, "Intentos"),
                    Estado = clsLectura.mtdLeerTexto(r, "EstadoSincronizacion"),
                    Fecha = clsLectura.mtdLeerFecha(r, "FechaCompletado")
                });
            }
            return lista;
        }

        private static void RegistrarError(string mensaje)
        {
            clsConexion.mtdPasarAModoLocal(mensaje);
        }
    }
}
