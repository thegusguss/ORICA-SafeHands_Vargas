-- Verifica la base ORICA_SafeHandsDB. Sirve igual para LocalDB y para Azure.
-- Solo hace consultas (SELECT): no modifica nada.
-- Antes de ejecutar, elige ORICA_SafeHandsDB en la lista de bases de SSMS
-- (Azure no permite USE, por eso no está aquí).

-- 1) ¿Dónde estoy conectado? (servidor, base y tipo de motor)
SELECT @@SERVERNAME                          AS Servidor,
       DB_NAME()                             AS BaseDatos,
       SERVERPROPERTY('Edition')             AS Edicion,
       CASE SERVERPROPERTY('EngineEdition')
            WHEN 5 THEN 'Azure SQL Database'
            ELSE 'SQL Server (LocalDB)'
       END                                   AS TipoMotor,
       SERVERPROPERTY('ProductVersion')      AS Version,
       DATABASEPROPERTYEX(DB_NAME(), 'Collation') AS Intercalacion;

-- 2) Objetos creados: en Azure salen 20 tablas y 22 llaves foráneas.
--    En LocalDB sale 1 tabla más (CopiaLocal), que la app crea al usarla como copia,
--    y las llaves foráneas aparecen apagadas a propósito (ver consulta 4).
SELECT CASE type WHEN 'U' THEN 'Tablas'
                 WHEN 'F' THEN 'Llaves foráneas'
                 WHEN 'PK' THEN 'Llaves primarias'
                 WHEN 'UQ' THEN 'Restricciones UNIQUE'
                 WHEN 'C' THEN 'Restricciones CHECK'
                 WHEN 'FN' THEN 'Funciones'
       END      AS Objeto,
       COUNT(*) AS Cantidad
FROM sys.objects
WHERE type IN ('U', 'F', 'PK', 'UQ', 'C', 'FN')
  AND is_ms_shipped = 0
GROUP BY type
ORDER BY Objeto;

-- 3) Cada tabla con su número de filas (sirve para comparar LocalDB y Azure)
SELECT t.name                   AS Tabla,
       SUM(p.rows)              AS Filas
FROM sys.tables t
INNER JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
GROUP BY t.name
ORDER BY t.name;

-- 4) Relaciones entre tablas (llaves foráneas).
--    En Azure deben estar activas; en la copia local quedan apagadas para poder guardar
--    lo que se hace sin conexión y subirlo después.
SELECT fk.name                              AS LlaveForanea,
       OBJECT_NAME(fk.parent_object_id)     AS TablaHija,
       OBJECT_NAME(fk.referenced_object_id) AS TablaPadre,
       CASE WHEN fk.is_disabled = 1 THEN 'Apagada' ELSE 'Activa' END AS Estado
FROM sys.foreign_keys fk
ORDER BY TablaHija, TablaPadre;

-- 5) Versión de la base y datos de catálogo cargados
SELECT VersionBD, Descripcion, FechaAplicacion FROM dbo.VersionBaseDatos;

SELECT Codigo, Nombre FROM dbo.Rol ORDER BY IdRol;

SELECT Codigo, Nombre, NumeroConfirmaciones, TimeoutSegundos
FROM dbo.NivelCriticidad
ORDER BY IdNivelCriticidad;

-- 6) Hora de la base: en Azure fnAhora() devuelve la hora de Perú (el servidor trabaja en UTC)
SELECT SYSDATETIME() AS HoraServidor;
IF OBJECT_ID('dbo.fnAhora') IS NOT NULL
    EXEC('SELECT dbo.fnAhora() AS HoraPeru');
