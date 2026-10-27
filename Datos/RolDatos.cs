using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PROYECTO_ORICA_SAFEHANDS_VARGAS.Entidades;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS.Datos
{
    public class RolDatos
    {
        public List<Rol> ListarActivos()
        {
            List<Rol> lista = new List<Rol>();

            using (SqlConnection conexion = Conexion.CrearConexion())
            {
                conexion.Open();

                string consulta = @"
                    SELECT
                        IdRol,
                        Codigo,
                        Nombre,
                        Descripcion,
                        Estado,
                        FechaRegistro
                    FROM Rol
                    WHERE Estado = 1
                    ORDER BY Nombre";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Rol rol = new Rol();

                            rol.IdRol = Convert.ToInt32(lector["IdRol"]);
                            rol.Codigo = lector["Codigo"].ToString();
                            rol.Nombre = lector["Nombre"].ToString();
                            rol.Descripcion = lector["Descripcion"] == DBNull.Value
                                ? ""
                                : lector["Descripcion"].ToString();
                            rol.Estado = Convert.ToBoolean(lector["Estado"]);
                            rol.FechaRegistro = Convert.ToDateTime(lector["FechaRegistro"]);

                            lista.Add(rol);
                        }
                    }
                }
            }

            return lista;
        }
    }
}