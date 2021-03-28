using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MEvaluacion:DEvaluacion
    {

        public string Insertar(DEvaluacion Evaluacion)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO evaluacion(
                            idUsuario,
                            idMeta,
                            valorEvaluado,
                            observacion,
                            status,
                            fechaEvaluacion
                        ) VALUES(
                            @idUsuario,
                            @idMeta,
                            @valorEvaluado,
                            @observacion,
                            @status,
                            @fechaEvaluacion
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idUsuario", Evaluacion.idUsuario);
                    comm.Parameters.AddWithValue("@idMeta", Evaluacion.idMeta);
                    comm.Parameters.AddWithValue("@valorEvaluado", Evaluacion.valorEvaluado);
                    comm.Parameters.AddWithValue("@observacion", Evaluacion.observacion);
                    comm.Parameters.AddWithValue("@status", Evaluacion.status);
                    comm.Parameters.AddWithValue("@fechaEvaluacion", Evaluacion.fechaEvaluacion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la evaluacion del empleado";

                        if(respuesta.Equals("OK"))
                        {
                            string query2 = @"
                                        UPDATE meta SET
                                            status = 2
                                        WHERE idUsuario = @idUsuario;
	                        ";

                            using (SqlCommand comm2 = new SqlCommand(query2, conn))
                            {
                                comm2.Parameters.AddWithValue("@idUsuario", Evaluacion.idUsuario);

                                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la meta";
                            }
                        }
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


        public string Editar(DEvaluacion Evaluacion)
        {
            string respuesta = "";

            string query = @"
                        UPDATE evaluacion SET
                            idUsuario = @idUsuario,
                            idMeta = @idMeta,
                            valorEvaluado = @valorEvaluado,
                            observacion = @observacion,
                            status = @status,
                            fechaEvaluacion = @fechaEvaluacion
                        WHERE idEvaluacion = @idEvaluacion;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idUsuario", Evaluacion.idUsuario);
                    comm.Parameters.AddWithValue("@idMeta", Evaluacion.idMeta);
                    comm.Parameters.AddWithValue("@valorEvaluado", Evaluacion.valorEvaluado);
                    comm.Parameters.AddWithValue("@observacion", Evaluacion.observacion);
                    comm.Parameters.AddWithValue("@status", Evaluacion.status);
                    comm.Parameters.AddWithValue("@fechaEvaluacion", Evaluacion.fechaEvaluacion);
                    comm.Parameters.AddWithValue("@idEvaluacion", Evaluacion.idEvaluacion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la evaluacion del empleado";
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


        public string Eiminar(DEvaluacion Evaluacion)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM evaluacion WHERE idEvaluacion=@idEvaluacion
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idEvaluacion", Evaluacion.idEvaluacion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la evaluacion del empleado";
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
        public List<DEvaluacion> Mostrar(string Buscar)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT ev.idEvaluacion, ev.idMeta, e.cedula, ev.valorEvaluado, ev.observacion, ev.fechaEvaluacion, ev.status from [evaluacion] ev inner join [empleado] e on ev.idUsuario=e.idEmpleado where e.cedula like '" + Buscar + "%' order by e.cedula";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DEvaluacion
                                {
                                    idEvaluacion = reader.GetInt32(0),
                                    idMeta = reader.GetInt32(1),
                                    cedula = reader.GetString(2),
                                    valorEvaluado = reader.GetDouble(3),
                                    observacion = reader.GetString(4),
                                    fechaEvaluacion = reader.GetDateTime(5),
                                    status = reader.GetInt32(6)
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
