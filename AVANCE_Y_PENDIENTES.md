# ORICA SafeHands: avance y pendientes

Última actualización: 08/11/2026 (Gustavo)

## Hecho y conectado a la BD

| Módulo | Archivos principales |
|---|---|
| Login con PIN y sesión | `FrmLogin`, `clsAutenticacionNegocio/Datos` |
| Operadores | `FrmOperadores`, `FrmOperadorEditar`, `clsUsuarioNegocio/Datos` |
| Credenciales | `Credenciales` (FrmCredenciales), `FrmCredencialRegistrar`, `clsCredencialNegocio/Datos` |
| **Equipos** | `FrmEquipos`, `FrmEquipoEditar`, `clsEquipoNegocio/Datos`, `Modelos/Equipo`, `Modelos/TipoEquipo` |
| **Criticidad** | `FrmCriticidad`, `FrmCriticidadEditar`, `clsCriticidadNegocio/Datos`, `Modelos/NivelCriticidad` |
| **Comandos** (con equipos asignados) | `FrmComandos`, `FrmComandoEditar`, `clsComandoNegocio/Datos`, `Modelos/Comando`, `Modelos/TipoAccion` |
| **Comandos disponibles** (operador) | `FrmComandosDisponibles`: ahora lee de la BD; constructor `(Equipo)` opcional |
| **Configuración** | `FrmConfiguracion`, `clsConfiguracionNegocio/Datos`, `Modelos/ParametroConfiguracion` |
| **Operación por voz** | `FrmOperacionVoz`, `clsOperacionVozNegocio`, `Modelos/OperacionVoz` (System.Speech; ejecución simulada hasta conectar el ESP32) |
| **Mi historial** (operador) | `FrmMiHistorial` (lee `clsEventoNegocio.mtdListarHistorialUsuario`) |
| **Alertas** | `FrmAlertas`, `clsAlertaNegocio/Datos`, `Modelos/Alerta`, `clsMonitorArduino` (desconexión USB) |
| **Dashboard** (administrador) | `FrmDashboard` (frmPrincipal), `clsDashboardNegocio/Datos`, `Modelos/ResumenDashboard` |
| **Inicio del operador** | `FrmPrincipalOperador` (datos de su sesión), `clsDashboardNegocio.mtdResumenOperador`, `Modelos/ResumenOperador` |
| **Permisos por rol** | `clsPermisoNegocio` (qué módulo ve cada rol), `FrmDashboard.AplicarPermisos` (oculta botones del menú) |
| **Reportes** | `FrmReportes` (filtros, rango de fechas, gráficos), `clsReporteNegocio/Datos`, `Modelos/Reporte`, `clsImpresionReporte` (genera el PDF y lo abre) |
| **Azure + modo sin conexión** | `clsConexion` (elige Azure o LocalDB), `clsSincronizacionDatos/Negocio`, `Modelos/EstadoSincronizacion`, `FrmSincronizacion`, `Scripts/ORICA_SafeHandsDB_Azure.sql`, `conexion.ejemplo.config` |
| **Eventos / Auditoría** | `FrmEventos`, `clsEventoNegocio/Datos`, `clsAuditoriaNegocio/Datos`, `Modelos/RegistroHistorial` |

### Reglas que ya aplica el código
- **Criticidad:** de 0 a 2 confirmaciones. El timeout va de 3 a 60 s, y es 0 cuando no hay confirmaciones. N4 siempre lleva doble confirmación.
- **Comandos:** la frase se guarda en MAYÚSCULAS y sin tildes (`clsComandoNegocio.mtdNormalizarFrase`, que también sirve para comparar lo que reconozca la voz). Debe tener al menos 2 palabras y no puede ser CONFIRMAR ni CANCELAR. Los niveles N3 y N4 piden 90 % de confianza o más. Cada comando necesita al menos un equipo.
- **Equipos:** el código es único y el identificador de hardware también.
- **Reportes:** solo eventos de operación (comandos de voz, voz no reconocida y paradas). Rango máximo de un año, sin fechas futuras, hasta 5000 eventos (los más recientes). Para el PDF se imprime con "Microsoft Print to PDF".
- **Permisos por rol** (menú del administrador): ADMIN ve todo. SUP ve Dashboard, Alertas, Eventos y Reportes. MANT ve Dashboard, Diagnóstico, Alertas, Equipos y Sincronización. Un rol desconocido solo ve el Dashboard. OPE usa `FrmPrincipalOperador`. La parada general queda visible para todos. Para cambiar permisos, editar las listas de `clsPermisoNegocio`.

- **Azure y sin conexión:** Azure SQL es la base principal (compartida). La LocalDB es una copia: si Azure no responde, la app sigue con la copia y lo que haga el operador (sesión, eventos, confirmaciones, alertas) queda pendiente. Cada 30 s se reintenta; al volver, se sube lo pendiente en una transacción, se vuelve a Azure y se renueva la copia. Sin conexión, los roles que no son operador solo pueden consultar. Las fechas usan `dbo.fnAhora()` (hora de Perú; Azure trabaja en UTC). En la copia local las llaves foráneas están apagadas y los eventos quedan con Origen `SIN_CONEXION`.

### Cómo registrar eventos y auditoría (úsalo en los módulos nuevos)
- **Operación** (voz, sesión, parada): `new clsEventoNegocio().mtdRegistrar(clsEventoNegocio.TipoComandoVoz, clsEventoNegocio.ResultadoEjecutado, detalle, idEquipo, idComando, idNivel, texto, confianza, confirmado)`.
- **Cambios administrativos**: `clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloEquipos, "MODIFICAR", "Equipo", id, descripcion, antes, despues)`.
- Ninguno de los dos lanza excepciones: si falla la BD, la operación principal sigue funcionando.
- Ya quedan registrados: inicio y cierre de sesión, PIN incorrecto o bloqueado, y los cambios en operadores, credenciales, equipos, comandos, criticidad y configuración.
- **Alertas automáticas**: se crean solas en `clsOperacionVozNegocio` (N3/N4 rechazado, cancelado, vencido o con error; parada) y en `clsMonitorArduino` (desconexión del puerto COM). Para una falla del enlace serial, usar `new clsAlertaNegocio().mtdAlertaEnlace(puerto, detalle)`.
- **Leer la configuración** desde cualquier módulo: `new clsConfiguracionNegocio().mtdObtenerDecimal(clsConfiguracionNegocio.UmbralConfianza)`.

## Configurar Azure (una vez)
1. En el portal de Azure: crear la base `ORICA_SafeHandsDB` (autenticación SQL) y en **Redes** agregar la IP de cada uno.
2. En SSMS, conectado a esa base (no a master), ejecutar `Scripts/ORICA_SafeHandsDB_Azure.sql`.
3. Copiar `conexion.ejemplo.config` como `conexion.config` (misma carpeta que App.config) y pegar la cadena ADO.NET con la contraseña. **Ese archivo no se sube a git.** Sin él, la app trabaja solo en local, como antes.
4. Compilar y abrir **Sincronización**:
   - Quien tenga los datos buenos (ADMIN): **Subir esta PC a Azure** (reemplaza todo Azure, solo la primera vez).
   - Los demás: **Descargar de Azure**.
5. Para probar sin internet: **Simular sin conexión** → entrar como operador y dar órdenes → **Volver a conectar**.

## Pendiente (en este orden)
1. Enlace serial real con el **Arduino Uno** por USB (antes era ESP32) (reemplazar la simulación en `clsOperacionVozNegocio.mtdEjecutar`), Diagnóstico y Sincronización (fase Arduino).

## Para trabajar sin conflictos
- `clsBaseDatos` ahora abre la conexión con `clsConexion.mtdAbrirConexion` (Azure o local) y usa un candado. No modificar `clsBaseDatos`, `clsLectura`, `Resultado` ni `Sesion` sin avisar.
- Los `.csproj` listan cada archivo. Si git marca conflicto ahí, conservar las líneas de ambos.
- Los eventos de los formularios nuevos se conectan en el constructor (`.cs`), no en el diseñador.
