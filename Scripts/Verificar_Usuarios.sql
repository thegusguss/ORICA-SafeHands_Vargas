-- Verifica que los usuarios creados/editados desde la app se guardaron en la base.
-- Solo hace consultas (SELECT): no modifica nada.
USE ORICA_SafeHandsDB;
GO

-- 1) Todos los usuarios, el más nuevo primero.
SELECT u.IdUsuario,
       u.Codigo,
       u.Nombres + ' ' + u.Apellidos AS Nombre,
       r.Nombre                      AS Rol,
       CASE WHEN u.Estado = 1 THEN 'Activo' ELSE 'Suspendido' END AS Estado,
       u.Documento, u.Telefono, u.Correo,
       u.FechaRegistro,
       u.FechaModificacion,
       u.UltimoAcceso
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
ORDER BY u.IdUsuario DESC;

-- 2) Credenciales de cada usuario (los nuevos aún no tienen: se asignan en Credenciales).
SELECT u.Codigo,
       ISNULL(tc.Codigo, '(sin credencial)') AS Credencial,
       c.Estado           AS CredencialActiva,
       c.IntentosFallidos,
       c.UltimoUso
FROM dbo.Usuario u
LEFT JOIN dbo.Credencial c      ON c.IdUsuario = u.IdUsuario
LEFT JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
ORDER BY u.Codigo;

-- 3) Últimas 10 sesiones (cada inicio de sesión con PIN deja una fila aquí).
SELECT TOP 10 s.IdSesion,
       u.Codigo,
       s.MetodoAutenticacion,
       s.Estacion,
       s.FechaInicio,
       s.FechaFin,
       s.EstadoSesion
FROM dbo.SesionOperador s
INNER JOIN dbo.Usuario u ON u.IdUsuario = s.IdUsuario
ORDER BY s.FechaInicio DESC;
