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
                            idTipoMetrica,
                            valorMeta,
                            idEmpleado,
                            idDepartamento,
                            status,
                            fechaInicio,
                            fechaFinal,
                            idUsuario
                        ) VALUES(
                            @idTipoMetrica,
                            @valorMeta,
                            @idEmpleado,
                            @idDepartamento,
                            @status,
                            @fechaInicio,
                            @fechaFinal,
                            @idUsuario
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idTipoMetrica", Meta.idTipoMetrica);
                    comm.Parameters.AddWithValue("@valorMeta", Meta.valorMeta);
                    comm.Parameters.AddWithValue("@idEmpleado", Meta.idEmpleado);
                    comm.Parameters.AddWithValue("@idDepartamento", Meta.idDepartamento);
                    comm.Parameters.AddWithValue("@status", Meta.status);
                    comm.Parameters.AddWithValue("@fechaInicio", Meta.fechaInicio);
                    comm.Parameters.AddWithValue("@fechaFinal", Meta.fechaFinal);
                    comm.Parameters.AddWithValue("@idUsuario", Meta.idUsuario);

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
                            idTipoMetrica = @idTipoMetrica,
                            valorMeta = @valorMeta,
                            idEmpleado = @idEmpleado,
                            idDepartamento = @idDepartamento,
                            status = @status,
                            fechaInicio = @fechaInicio,
                            fechaFinal = @fechaFinal,
                            idUsuario = @idUsuario
                        WHERE idMeta = @idMeta;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idMeta", Meta.idMeta);
                    comm.Parameters.AddWithValue("@idTipoMetrica", Meta.idTipoMetrica);
                    comm.Parameters.AddWithValue("@valorMeta", Meta.valorMeta);
                    comm.Parameters.AddWithValue("@idEmpleado", Meta.idEmpleado);
                    comm.Parameters.AddWithValue("@idDepartamento", Meta.idDepartamento);
                    comm.Parameters.AddWithValue("@status", Meta.status);
                    comm.Parameters.AddWithValue("@fechaInicio", Meta.fechaInicio);
                    comm.Parameters.AddWithValue("@fechaFinal", Meta.fechaFinal);
                    comm.Parameters.AddWithValue("@idUsuario", Meta.idUsuario);

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

                    comm.CommandText = "SELECT m.idMeta, tm.nombre, m.valorMeta, e.cedula, u.usuario, d.nombre, m.fechaInicio, m.fechaFinal, m.status from [meta] m inner join [Empleado] e on m.idEmpleado=e.idEmpleado inner join [Usuario] u on m.idUsuario=u.idUsuario inner join [Departamento] d on m.idDepartamento=d.idDepartamento inner join [TipoMetrica] tm on m.idTipoMetrica=tm.idTipoMetrica where e.cedula like '" + Buscar + "%' order by e.cedula";

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
                                    nombreMetrica = reader.GetString(1),
                                    valorMeta = reader.GetDouble(2),
                                    cedula = reader.GetString(3),
                                    usuario = reader.GetString(4),
                                    departamento = reader.GetString(5),
                                    fechaInicio = reader.GetDateTime(6),
                                    fechaFinal = reader.GetDateTime(7),
                                    status = reader.GetInt32(8)
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
