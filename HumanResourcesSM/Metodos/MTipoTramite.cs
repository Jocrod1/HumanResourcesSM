using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MTipoTramite:DTipoTramite
    {


        //Metodos

        public string Insertar(DTipoTramite TipoTramite)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO tipoTramite(
                            nombre,
                            statusCambio
                        ) VALUES(
                            @nombre,
                            @statusCambio
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", TipoTramite.nombre);
                    comm.Parameters.AddWithValue("@statusCambio", TipoTramite.statusCambio);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del tipo de tramite";
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


        public string Editar(DTipoTramite TipoTramite)
        {
            string respuesta = "";

            string query = @"
                        UPDATE tipoTramite SET
                            nombre = @nombre,
                            statusCambio = @statusCambio
                            WHERE idTipoTramite = @idTipoTramite;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", TipoTramite.nombre);
                    comm.Parameters.AddWithValue("@statusCambio", TipoTramite.statusCambio);

                    comm.Parameters.AddWithValue("@idTipoTramite", TipoTramite.idTipoTramite);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del tipo de tramite";
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


        public string Eliminar(DTipoTramite TipoTramite)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM tipoTramite WHERE idTipoTramite=@idTipoTramite
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idTipoTramite", TipoTramite.idTipoTramite);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del tipo de tramite";
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
        public List<DTipoTramite> Mostrar(string Buscar)
        {
            List<DTipoTramite> ListaGenerica = new List<DTipoTramite>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [tipoTramite] where nombre like '" + Buscar + "%' order by nombre";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DTipoTramite
                                {
                                    idTipoTramite = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    statusCambio = reader.GetString(2)
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

        public List<DTipoTramite> Encontrar(int Buscar)
        {
            List<DTipoTramite> ListaGenerica = new List<DTipoTramite>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [tipoTramite] where idTipoTramite= " + Buscar + " ";


                    //comm.Parameters.AddWithValue("@textoBuscar", "");

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DTipoTramite
                                {
                                    idTipoTramite = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    statusCambio = reader.GetString(2)
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
