using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MMeta:DMeta
    {


        public string Insertar(DMeta Meta)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO meta(
                            tipoMetrica,
                            valorMeta,
                            idEmpleado,
                            status,
                            periodo
                        ) VALUES(
                            @tipoMetrica,
                            @valorMeta,
                            @idEmpleado,
                            @status,
                            @periodo
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@tipoMetrica", Meta.tipoMetrica);
                    comm.Parameters.AddWithValue("@valorMeta", Meta.valorMeta);
                    comm.Parameters.AddWithValue("@idEmpleado", Meta.idEmpleado);
                    comm.Parameters.AddWithValue("@status", Meta.status);
                    comm.Parameters.AddWithValue("@periodo", Meta.periodo);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la meta del empleado";
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


        public string Editar(DMeta Meta)
        {
            string respuesta = "";

            string query = @"
                        UPDATE meta SET
                            tipoMetrica = @tipoMetrica,
                            valorMeta = @valorMeta,
                            idEmpleado = @idEmpleado,
                            status = @status,
                            periodo = @periodo
                        WHERE idMeta = @idMeta;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idMeta", Meta.idMeta);
                    comm.Parameters.AddWithValue("@tipoMetrica", Meta.tipoMetrica);
                    comm.Parameters.AddWithValue("@valorMeta", Meta.valorMeta);
                    comm.Parameters.AddWithValue("@idEmpleado", Meta.idEmpleado);
                    comm.Parameters.AddWithValue("@status", Meta.status);
                    comm.Parameters.AddWithValue("@periodo", Meta.periodo);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la meta del empleado";
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


        public string Eiminar(DMeta Meta)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM meta WHERE idMeta=@idMeta
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idMeta", Meta.idMeta);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la meta del empleado";
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
        public List<DMeta> Mostrar(string Buscar)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT m.idMeta, m.tipoMetrica, m.valorMeta, e.cedula, m.periodo, m.status from [meta] m inner join [empleado] e on m.idEmpleado=e.idEmpleado where e.cedula like '" + Buscar + "%' order by e.cedula";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DMeta
                                {
                                    idMeta = reader.GetInt32(0),
                                    tipoMetrica = reader.GetString(1),
                                    valorMeta = reader.GetDouble(2),
                                    cedula = reader.GetString(3),
                                    periodo = reader.GetDateTime(4),
                                    status = reader.GetInt32(5)
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
