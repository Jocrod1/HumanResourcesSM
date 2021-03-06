using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MIdiomaHablado : DIdiomaHablado
    {
        public string Insertar(DIdiomaHablado IdiomaHablado)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO idiomaHablado(
                            idIdioma,
                            idEmpleado,
                            nivel
                        ) VALUES(
                            @idIdioma,
                            @idEmpleado,
                            @nivel
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idIdioma", IdiomaHablado.idIdioma);
                    comm.Parameters.AddWithValue("@idEmpleado", IdiomaHablado.idEmpleado);
                    comm.Parameters.AddWithValue("@nivel", IdiomaHablado.nivel);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del idioma hablado";
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


        public string Editar(DIdiomaHablado IdiomaHablado)
        {
            string respuesta = "";

            string query = @"
                        UPDATE idiomaHablado SET (
                            idIdioma,
                            idEmpleado,
                            nivel
                        ) VALUES(
                            @idIdioma,
                            @idEmpleado,
                            @nivel
                        ) WHERE idIdiomaHablado = @idIdiomaHablado;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idIdioma", IdiomaHablado.idIdioma);
                    comm.Parameters.AddWithValue("@idEmpleado", IdiomaHablado.idEmpleado);
                    comm.Parameters.AddWithValue("@nivel", IdiomaHablado.nivel);

                    comm.Parameters.AddWithValue("@idIdiomaHablado", IdiomaHablado.idIdiomaHablado);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del idioma hablado";
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


        public string Eiminar(DIdiomaHablado IdiomaHablado)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM idiomaHablado WHERE idIdiomaHablado=@idIdiomaHablado
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idIdiomaHablado", IdiomaHablado.idIdiomaHablado);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del idioma hablado";
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
        public List<DIdiomaHablado> Mostrar(string Buscar)
        {
            List<DIdiomaHablado> ListaGenerica = new List<DIdiomaHablado>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [idiomaHablado] where idIdioma = " + Buscar + " order by idIdioma";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DIdiomaHablado
                                {
                                    idIdioma = reader.GetInt32(1),
                                    idEmpleado = reader.GetInt32(2),
                                    nivel = reader.GetInt32(3)
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



        public List<DIdioma> MostrarIdioma(string Buscar)
        {
            List<DIdioma> ListaGenerica = new List<DIdioma>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [idioma] order by idIdioma ASC";


                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DIdioma
                                {
                                    idIdioma = reader.GetInt32(0),
                                    nombre = reader.GetString(1)
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
