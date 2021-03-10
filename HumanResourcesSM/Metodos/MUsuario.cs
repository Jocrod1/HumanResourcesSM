using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MUsuario:DUsuario
    {
        public string Insertar(DUsuario Usuario)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO usuario(
                            idRol,
                            usuario,
                            contraseña,
                            confirmacion,
                            entrevistando
                        ) VALUES(
                            @idRol,
                            @usuario,
                            @contraseña,
                            @confirmacion,
                            @entrevistando
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                    comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                    comm.Parameters.AddWithValue("@contraseña", Usuario.contraseña);
                    comm.Parameters.AddWithValue("@confirmacion", Usuario.confirmacion);
                    comm.Parameters.AddWithValue("@entrevistando", 0);

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
                            contraseña,
                            confirmacion,
                            entrevistando
                        ) VALUES(
                            @idRol,
                            @usuario,
                            @contraseña,
                            @confirmacion,
                            @entrevistando
                        ) WHERE idUsuario = @idUsuario;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                    comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                    comm.Parameters.AddWithValue("@contraseña", Usuario.contraseña);
                    comm.Parameters.AddWithValue("@confirmacion", Usuario.confirmacion);
                    comm.Parameters.AddWithValue("@entrevistando", Usuario.entrevistando);

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


        public string Eliminar(DUsuario Usuario)
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

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DUsuario
                                {
                                    idUsuario = reader.GetInt32(0),
                                    idRol = reader.GetInt32(1),
                                    usuario = reader.GetString(2),
                                    contraseña = reader.GetString(3),
                                    confirmacion = reader.GetString(4),
                                    entrevistando = reader.GetInt16(5)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public List<DUsuario> Login(string Usuario, string Contraseña)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [usuario] WHERE usuario= '" + Usuario + "' AND contraseña= '" + Contraseña + "'";


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
                                    idUsuario = reader.GetInt32(0),
                                    idRol = reader.GetInt32(1),
                                    usuario = reader.GetString(2),
                                    contraseña = reader.GetString(3),
                                    confirmacion = reader.GetString(4),
                                    entrevistando = reader.GetInt16(5)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public List<DUsuario> Encontrar(int Buscar)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [usuario] WHERE idUsuario= " + Buscar + "";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DUsuario
                                {
                                    idUsuario = reader.GetInt32(0),
                                    idRol = reader.GetInt32(1),
                                    usuario = reader.GetString(2),
                                    contraseña = reader.GetString(3),
                                    confirmacion = reader.GetString(4),
                                    entrevistando = reader.GetInt16(5)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
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




        public string Entrevistando(int IdUsuario, bool Entrevistando)
        {
            string respuesta = "";

            string query = @"
                        UPDATE usuario SET (
                            entrevistando
                        ) VALUES(
                            @entrevistando
                        ) WHERE idUsuario = @idUsuario;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    if (Entrevistando)
                        comm.Parameters.AddWithValue("@entrevistando", 0);
                    else
                        comm.Parameters.AddWithValue("@entrevistando", 1);


                    comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

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
    }
}
