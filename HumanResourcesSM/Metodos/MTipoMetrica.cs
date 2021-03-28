using System;
using System.Collections.Generic;
using System.Text;

using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MTipoMetrica:DTipoMetrica
    {


        public string Insertar(DTipoMetrica TipoMetrica)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO TipoMetrica(
                            nombre,
                            idDepartamento
                        ) VALUES (
                            @nombre,
                            @idDepartamento
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", TipoMetrica.nombre);
                    comm.Parameters.AddWithValue("@idDepartamento", TipoMetrica.idDepartamento);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del tipo de métrica";
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


        public string Editar(DTipoMetrica TipoMetrica)
        {
            string respuesta = "";

            string query = @"
                        UPDATE educacion SET 
                            nombre = @nombre,
                            idDepartamento = @idDepartamento,
                        WHERE idTipoMetrica = @idTipoMetrica;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", TipoMetrica.nombre);
                    comm.Parameters.AddWithValue("@idDepartamento", TipoMetrica.idDepartamento);

                    comm.Parameters.AddWithValue("@idTipoMetrica", TipoMetrica.idTipoMetrica);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del tipo de metrica";
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
                        DELETE FROM TipoMetrica WHERE idTipoMetrica=@idTipoMetrica
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idTipoMetrica", id);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del tipo de metrica";
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
        public List<DTipoMetrica> Mostrar(int Buscar)
        {
            List<DTipoMetrica> ListaGenerica = new List<DTipoMetrica>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [TipoMetrica] where idTipoMetrica = " + Buscar + " order by idTipoMetrica";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {


                            while (reader.Read())
                            {

                                ListaGenerica.Add(new DTipoMetrica
                                {
                                    idTipoMetrica = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    idDepartamento = reader.GetInt32(2)
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

        public List<DTipoMetrica> Encontrar(int Buscar)
        {
            List<DTipoMetrica> ListaGenerica = new List<DTipoMetrica>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [TipoMetrica] where idTipoMetrica = " + Buscar + "";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {


                            while (reader.Read())
                            {

                                ListaGenerica.Add(new DTipoMetrica
                                {
                                    idTipoMetrica = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    idDepartamento = reader.GetInt32(2)
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
