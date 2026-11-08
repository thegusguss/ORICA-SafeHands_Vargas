# ORICA SafeHands: avance y pendientes

Última actualización: 07/11/2026 (Gustavo)

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
| **Eventos / Auditoría** | `FrmEventos`, `clsEventoNegocio/Datos`, `clsAuditoriaNegocio/Datos`, `Modelos/RegistroHistorial` |

### Reglas que ya aplica el código
- **Criticidad:** de 0 a 2 confirmaciones. El timeout va de 3 a 60 s, y es 0 cuando no hay confirmaciones. N4 siempre lleva doble confirmación.
- **Comandos:** la frase se guarda en MAYÚSCULAS y sin tildes (`clsComandoNegocio.mtdNormalizarFrase`, que también sirve para comparar lo que reconozca la voz). Debe tener al menos 2 palabras y no puede ser CONFIRMAR ni CANCELAR. Los niveles N3 y N4 piden 90 % de confianza o más. Cada comando necesita al menos un equipo.
- **Equipos:** el código es único y el identificador de hardware también.

### Cómo registrar eventos y auditoría (úsalo en los módulos nuevos)
- **Operación** (voz, sesión, parada): `new clsEventoNegocio().mtdRegistrar(clsEventoNegocio.TipoComandoVoz, clsEventoNegocio.ResultadoEjecutado, detalle, idEquipo, idComando, idNivel, texto, confianza, confirmado)`.
- **Cambios administrativos**: `clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloEquipos, "MODIFICAR", "Equipo", id, descripcion, antes, despues)`.
- Ninguno de los dos lanza excepciones: si falla la BD, la operación principal sigue funcionando.
- Ya quedan registrados: inicio y cierre de sesión, PIN incorrecto o bloqueado, y los cambios en operadores, credenciales, equipos, comandos, criticidad y configuración.
- **Leer la configuración** desde cualquier módulo: `new clsConfiguracionNegocio().mtdObtenerDecimal(clsConfiguracionNegocio.UmbralConfianza)`.

## Pendiente (en este orden)
1. **Mi historial** del operador (`FrmMiHistorial`): ya existe `clsEventoNegocio.mtdListarHistorialUsuario(idUsuario)`.
2. **Alertas** (sugerencia: crear una alerta en `clsOperacionVozNegocio` cuando haya timeout o rechazo en N3/N4) (`FrmAlertas`, tabla `Alerta`: ACTIVA → RECONOCIDA → CERRADA).
3. **Dashboard** con totales reales.
4. **Permisos por rol**: ahora SUP y MANT ven el menú completo del administrador.
5. **Login con RFID**: el botón todavía solo prueba la conexión.
6. Reportes, enlace serial real con el ESP32 (reemplazar la simulación en `clsOperacionVozNegocio.mtdEjecutar`), Diagnóstico y Sincronización (fase ESP32).

## Para trabajar sin conflictos
- No modificar `clsBaseDatos`, `clsLectura`, `Resultado` ni `Sesion` sin avisar.
- Los `.csproj` listan cada archivo. Si git marca conflicto ahí, conservar las líneas de ambos.
- Los eventos de los formularios nuevos se conectan en el constructor (`.cs`), no en el diseñador.
