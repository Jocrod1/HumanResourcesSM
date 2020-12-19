using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MUsuario:DUsuario
    {

        //Metodos

        public string Insertar(DUsuario Usuario)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO usuario(
                            idRol,
                            usuario,
                            constraseña,
                            confirmacion
                        ) VALUES(
                            @idRol,
                            @usuario,
                            @constraseña,
                            @confirmacion
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                    comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                    comm.Parameters.AddWithValue("@constraseña", Usuario.contraseña);
                    comm.Parameters.AddWithValue("@confirmacion", Usuario.confirmacion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del usuario";
                    }
                    catch (SqlException e)
                    {
                        respuesta = e.Message;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return respuesta;
                }
            }
        }


        public string Editar(DUsuario Usuario)
        {
            string respuesta = "";

            string query = @"
                        UPDATE usuario SET (
                            idRol,
                            usuario,
                            constraseña,
                            confirmacion
                        ) VALUES(
                            @idRol,
                            @usuario,
                            @constraseña,
                            @confirmacion
                        ) WHERE idUsuario = @idUsuario;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                    comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                    comm.Parameters.AddWithValue("@constraseña", Usuario.contraseña);
                    comm.Parameters.AddWithValue("@confirmacion", Usuario.confirmacion);

                    comm.Parameters.AddWithValue("@idUsuario", Usuario.idUsuario);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del usuario";
                    }
                    catch (SqlException e)
                    {
                        respuesta = e.Message;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return respuesta;
                }
            }
        }


        public string Eiminar(DUsuario Usuario)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM usuario WHERE idUsuario=@idUsuario
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idUsuario", Usuario.idUsuario);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del usuario";
                    }
                    catch (SqlException e)
                    {
                        respuesta = e.Message;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return respuesta;
                }
            }
        }



        //funcionando
        public List<DUsuario> Mostrar(string Buscar)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [usuario] where usuario like '" + Buscar + "%' order by usuario";


                    //comm.Parameters.AddWithValue("@textoBuscar", "");

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DUsuario
                                {
                                    idRol = reader.GetInt32(1),
                                    usuario = reader.GetString(2)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        //error
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return ListaGenerica;
                }
            }

        }
    }
}
