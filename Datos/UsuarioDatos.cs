using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PROYECTO_ORICA_SAFEHANDS_VARGAS.Entidades;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS.Datos
{
    public class UsuarioDatos
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                using (SqlCommand comando = new SqlCommand("usp_Usuario_Listar", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(MapearUsuario(lector));
                        }
                    }
                }
            }

            return lista;
        }

        public List<Usuario> Buscar(string texto)
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                using (SqlCommand comando = new SqlCommand("usp_Usuario_Buscar", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add("@Texto", SqlDbType.VarChar, 120).Value = texto;

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(MapearUsuario(lector));
                        }
                    }
                }
            }

            return lista;
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                string consulta = @"
                    SELECT
                        U.IdUsuario,
                        U.Codigo,
                        U.Nombres,
                        U.Apellidos,
                        U.Documento,
                        U.Telefono,
                        U.Correo,
                        U.IdRol,
                        R.Nombre AS Rol,
                        U.Estado,
                        U.FechaRegistro,
                        U.FechaModificacion,
                        U.UltimoAcceso,
                        U.RowVersion
                    FROM Usuario U
                    INNER JOIN Rol R
                        ON U.IdRol = R.IdRol
                    WHERE U.IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            usuario = MapearUsuario(lector);
                        }
                    }
                }
            }

            return usuario;
        }

        public int Registrar(Usuario usuario)
        {
            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                string consulta = @"
                    INSERT INTO Usuario
                    (
                        Codigo,
                        Nombres,
                        Apellidos,
                        Documento,
                        Telefono,
                        Correo,
                        IdRol,
                        Estado
                    )
                    VALUES
                    (
                        @Codigo,
                        @Nombres,
                        @Apellidos,
                        @Documento,
                        @Telefono,
                        @Correo,
                        @IdRol,
                        @Estado
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@Codigo", SqlDbType.VarChar, 15).Value =
                        usuario.Codigo;

                    comando.Parameters.Add("@Nombres", SqlDbType.VarChar, 80).Value =
                        usuario.Nombres;

                    comando.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100).Value =
                        usuario.Apellidos;

                    comando.Parameters.Add("@Documento", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(usuario.Documento)
                        ? (object)DBNull.Value
                        : usuario.Documento;

                    comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(usuario.Telefono)
                        ? (object)DBNull.Value
                        : usuario.Telefono;

                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 120).Value =
                        string.IsNullOrWhiteSpace(usuario.Correo)
                        ? (object)DBNull.Value
                        : usuario.Correo;

                    comando.Parameters.Add("@IdRol", SqlDbType.Int).Value =
                        usuario.IdRol;

                    comando.Parameters.Add("@Estado", SqlDbType.Bit).Value =
                        usuario.Estado;

                    return Convert.ToInt32(comando.ExecuteScalar());
                }
            }
        }

        public bool Modificar(Usuario usuario)
        {
            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                string consulta = @"
                    UPDATE Usuario
                    SET
                        Codigo = @Codigo,
                        Nombres = @Nombres,
                        Apellidos = @Apellidos,
                        Documento = @Documento,
                        Telefono = @Telefono,
                        Correo = @Correo,
                        IdRol = @IdRol,
                        FechaModificacion = SYSDATETIME()
                    WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@Codigo", SqlDbType.VarChar, 15).Value =
                        usuario.Codigo;

                    comando.Parameters.Add("@Nombres", SqlDbType.VarChar, 80).Value =
                        usuario.Nombres;

                    comando.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100).Value =
                        usuario.Apellidos;

                    comando.Parameters.Add("@Documento", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(usuario.Documento)
                        ? (object)DBNull.Value
                        : usuario.Documento;

                    comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(usuario.Telefono)
                        ? (object)DBNull.Value
                        : usuario.Telefono;

                    comando.Parameters.Add("@Correo", SqlDbType.VarChar, 120).Value =
                        string.IsNullOrWhiteSpace(usuario.Correo)
                        ? (object)DBNull.Value
                        : usuario.Correo;

                    comando.Parameters.Add("@IdRol", SqlDbType.Int).Value =
                        usuario.IdRol;

                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value =
                        usuario.IdUsuario;

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool CambiarEstado(int idUsuario, bool estado)
        {
            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                string consulta = @"
                    UPDATE Usuario
                    SET
                        Estado = @Estado,
                        FechaModificacion = SYSDATETIME()
                    WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@Estado", SqlDbType.Bit).Value = estado;
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        private Usuario MapearUsuario(SqlDataReader lector)
        {
            Usuario usuario = new Usuario();

            usuario.IdUsuario = Convert.ToInt32(lector["IdUsuario"]);
            usuario.Codigo = lector["Codigo"].ToString();
            usuario.Nombres = lector["Nombres"].ToString();
            usuario.Apellidos = lector["Apellidos"].ToString();

            usuario.Documento = lector["Documento"] == DBNull.Value
                ? ""
                : lector["Documento"].ToString();

            usuario.Telefono = lector["Telefono"] == DBNull.Value
                ? ""
                : lector["Telefono"].ToString();

            usuario.Correo = lector["Correo"] == DBNull.Value
                ? ""
                : lector["Correo"].ToString();

            usuario.IdRol = Convert.ToInt32(lector["IdRol"]);

            if (ExisteColumna(lector, "Rol"))
            {
                usuario.RolNombre = lector["Rol"].ToString();
            }

            usuario.Estado = Convert.ToBoolean(lector["Estado"]);
            usuario.FechaRegistro = Convert.ToDateTime(lector["FechaRegistro"]);

            if (ExisteColumna(lector, "FechaModificacion"))
            {
                usuario.FechaModificacion =
                    lector["FechaModificacion"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(lector["FechaModificacion"]);
            }

            if (ExisteColumna(lector, "UltimoAcceso"))
            {
                usuario.UltimoAcceso =
                    lector["UltimoAcceso"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(lector["UltimoAcceso"]);
            }

            if (ExisteColumna(lector, "RowVersion"))
            {
                usuario.RowVersion =
                    lector["RowVersion"] == DBNull.Value
                    ? null
                    : (byte[])lector["RowVersion"];
            }

            return usuario;
        }

        private bool ExisteColumna(SqlDataReader lector, string nombreColumna)
        {
            for (int i = 0; i < lector.FieldCount; i++)
            {
                if (lector.GetName(i).Equals(
                    nombreColumna,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}