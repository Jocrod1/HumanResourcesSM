using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MEducacion:DEducacion
    {

        public string Insertar(DEducacion Educacion)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO educacion(
                            idEmpleado,
                            nombreCarrera,
                            nombreInstitucion,
                            fechaIngreso,
                            fechaEgreso
                        ) VALUES(
                            @idEmpleado,
                            @nombreCarrera,
                            @nombreInstitucion,
                            @fechaIngreso,
                            @fechaEgreso
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idEmpleado", Educacion.idEmpleado);
                    comm.Parameters.AddWithValue("@nombreCarrera", Educacion.nombreCarrera);
                    comm.Parameters.AddWithValue("@nombreInstitucion", Educacion.nombreInstitucion);
                    comm.Parameters.AddWithValue("@fechaIngreso", Educacion.fechaIngreso);
                    comm.Parameters.AddWithValue("@fechaEgreso", Educacion.fechaEgreso);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la educacion";
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


        public string Editar(DEducacion Educacion)
        {
            string respuesta = "";

            string query = @"
                        UPDATE educacion SET 
                            idEmpleado = @idEmpleado,
                            nombreCarrera = @nombreCarrera,
                            nombreInstitucion = @nombreInstitucion,
                            fechaIngreso = @fechaIngreso,
                            fechaEgreso = @fechaEgreso
                        WHERE idEducacion = @idEducacion;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idEmpleado", Educacion.idEmpleado);
                    comm.Parameters.AddWithValue("@nombreCarrera", Educacion.nombreCarrera);
                    comm.Parameters.AddWithValue("@nombreInstitucion", Educacion.nombreInstitucion);
                    comm.Parameters.AddWithValue("@fechaIngreso", Educacion.fechaIngreso);
                    comm.Parameters.AddWithValue("@fechaEgreso", Educacion.fechaEgreso);

                    comm.Parameters.AddWithValue("@idEducacion", Educacion.idEducacion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la educacion";
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


        public string Eliminar(int id)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM educacion WHERE idEducacion=@idEducacion
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idEducacion", id);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la educacion";
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
        public List<DEducacion> Mostrar(int Buscar)
        {
            List<DEducacion> ListaGenerica = new List<DEducacion>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [educacion] where idEmpleado = " + Buscar + " order by idEmpleado";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            
                            while (reader.Read())
                            {
                                DateTime? fe = null;
                                if (!reader.IsDBNull(5))
                                {
                                    fe = reader.GetDateTime(5);
                                }

                                ListaGenerica.Add(new DEducacion
                                {
                                    idEducacion = reader.GetInt32(0),
                                    idEmpleado = reader.GetInt32(1),
                                    nombreCarrera = reader.GetString(2),
                                    nombreInstitucion = reader.GetString(3),
                                    fechaIngreso = reader.GetDateTime(4),
                                    fechaEgreso = fe
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

        public List<DEducacion> Encontrar(int Buscar)
        {
            List<DEducacion> ListaGenerica = new List<DEducacion>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [educacion] where idEducacion = " + Buscar + "";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {


                            while (reader.Read())
                            {
                                DateTime? fe = null;
                                if (!reader.IsDBNull(5))
                                {
                                    fe = reader.GetDateTime(5);
                                }

                                ListaGenerica.Add(new DEducacion
                                {
                                    idEducacion = reader.GetInt32(0),
                                    idEmpleado = reader.GetInt32(1),
                                    nombreCarrera = reader.GetString(2),
                                    nombreInstitucion = reader.GetString(3),
                                    fechaIngreso = reader.GetDateTime(4),
                                    fechaEgreso = fe
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
    }
}
