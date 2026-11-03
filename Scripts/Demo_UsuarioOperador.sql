/* ============================================================
   ORICA SafeHands - Usuario demo con rol Operador
   Crea OP-001 (Lucía Ramos) con PIN 2468 para probar FrmPrincipalOperador.
   Se puede ejecutar varias veces sin duplicar datos.
   ============================================================ */
USE ORICA_SafeHandsDB;
GO

DECLARE @IdRolOperador INT = (SELECT IdRol FROM dbo.Rol WHERE Codigo = 'OPE');

IF @IdRolOperador IS NULL
    THROW 50001, 'No existe el rol OPE en dbo.Rol. Ejecute primero el script base de la BD.', 1;

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Codigo = 'OP-001')
    INSERT dbo.Usuario (Codigo, Nombres, Apellidos, IdRol)
    VALUES ('OP-001', 'Lucía', 'Ramos', @IdRolOperador);
GO

DECLARE @IdOperador INT = (SELECT IdUsuario FROM dbo.Usuario WHERE Codigo = 'OP-001');
DECLARE @IdTipoPIN INT = (SELECT IdTipoCredencial FROM dbo.TipoCredencial WHERE Codigo = 'PIN');

-- Mismo formato que el PIN demo del administrador: SHA2_256 sobre VARCHAR, sin salt.
IF NOT EXISTS (SELECT 1 FROM dbo.Credencial WHERE Codigo = 'CRD-OP-001-PIN')
    INSERT dbo.Credencial (Codigo, IdUsuario, IdTipoCredencial, HashCredencial, ReferenciaVisible)
    VALUES ('CRD-OP-001-PIN', @IdOperador, @IdTipoPIN, HASHBYTES('SHA2_256', '2468'), 'PIN DEMO');
GO

-- Comprobación
SELECT u.Codigo, u.Nombres, u.Apellidos, r.Nombre AS Rol, c.Codigo AS Credencial,
       c.Estado AS CredencialActiva, c.IntentosFallidos
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
LEFT JOIN dbo.Credencial c ON c.IdUsuario = u.IdUsuario
ORDER BY u.Codigo;
GO
