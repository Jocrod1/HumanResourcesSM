using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MRelacionesLaborales:DRelacionesLaborales
    {
        public string Insertar(DRelacionesLaborales RelacionesLaborales)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO RelacionesLaborales(
                            idEmpleado,
                            idTipoTramite,
                            fechaTramite,
                            documentoUrl
                        ) VALUES(
                            @idEmpleado,
                            @idTipoTramite,
                            @fechaTramite,
                            @documentoUrl
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idEmpleado", RelacionesLaborales.idEmpleado);
                    comm.Parameters.AddWithValue("@idTipoTramite", RelacionesLaborales.idTipoTramite);
                    comm.Parameters.AddWithValue("@fechaTramite", RelacionesLaborales.fechaTramite);
                    comm.Parameters.AddWithValue("@documentoUrl", RelacionesLaborales.documentoUrl);


                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la evaluacion de la relacion laboral";

                        if (respuesta.Equals("OK"))
                        {
                                using (SqlCommand comm2 = new SqlCommand())
                                {
                                    comm2.Connection = conn;

<<<<<<< Updated upstream
                                    comm2.CommandText = "SELECT statusCambio from [tipoTramite] where idTipoTramite = " + RelacionesLaborales.idTipoTramite + "";

                                try
                                {
                                    string TipoCambio = "";

                                    using (SqlDataReader reader = comm2.ExecuteReader())
                                    {

                                        if (reader.Read())
                                        {
                                            TipoCambio = reader.GetString(0);
                                        }
                                    }

                                    string query3 = @"
                                                    UPDATE empleado SET
=======
                                    comm2.CommandText = "SELECT t.statusCambio from [TipoTramite] t inner join [RelacionesLaborales] r on t.idTipoTramite=r.idTipoTramite where t.idTipoTramite = @idTipoTramite";

                                    try
                                    {
                                        string TipoCambio="";

                                        using (SqlDataReader reader = comm2.ExecuteReader())
                                        {

                                            if (reader.Read())
                                            {
                                                TipoCambio = reader.GetString(0);
                                            }
                                        }

                                        string query3 = @"
                                                    UPDATE Empleado SET
>>>>>>> Stashed changes
                                                        estadoLegal = @estadoLegal
                                                    WHERE idEmpleado = @idEmpleado;
	                                    ";

                                    if (TipoCambio != "")
                                    {
                                        using (SqlCommand comm3 = new SqlCommand(query3, conn))
                                        {
                                            comm3.Parameters.AddWithValue("@estadoLegal", TipoCambio);
                                            comm3.Parameters.AddWithValue("@idEmpleado", RelacionesLaborales.idEmpleado);

                                            try
                                            {
                                                conn.Open();
                                                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el empleado";
                                            }
                                            catch (SqlException e)
                                            {
                                                MessageBox.Show(e.Message);
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
                                    else
                                    {
                                        respuesta = "Tipo de Tramite erroneo";
                                    }

                                }
                                catch (SqlException e)
                                {
                                    //error
                                    MessageBox.Show(e.Message);
                                }
                                
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

        public List<DRelacionesLaborales> Mostrar(int idEmpleado, int idTramite)
        {
            List<DRelacionesLaborales> ListaGenerica = new List<DRelacionesLaborales>();

            string Query = "";

            if(idEmpleado > -1 || idTramite > -1)
            {
                Query += "where ";
                if(idEmpleado > -1 && idTramite > -1)
                {
                    Query += "idEmpleado= " + idEmpleado + " AND idTipoTramite= " + idTramite; 
                }
                else if(idEmpleado > -1)
                {
                    Query += "idEmpleado= " + idEmpleado;
                }
                else if(idEmpleado > -1)
                {
                    Query += " idTipoTramite= " + idTramite;
                }
            }

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [relacionesLaborales] " + Query + "";


                    //comm.Parameters.AddWithValue("@textoBuscar", "");

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DRelacionesLaborales
                                {
                                    idRelacionesLaborales = reader.GetInt32(0),
                                    idEmpleado = reader.GetInt32(1),
                                    idTipoTramite = reader.GetInt32(2),
                                    fechaTramite = reader.GetDateTime(3),
                                    documentoUrl = reader.GetString(4)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        //error
                        MessageBox.Show(e.Message);
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

        public List<DRelacionesLaborales> Encontrar(int Buscar)
        {
            List<DRelacionesLaborales> ListaGenerica = new List<DRelacionesLaborales>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [relacionesLaborales] where idRelacionesLaborales= " + Buscar + "";


                    //comm.Parameters.AddWithValue("@textoBuscar", "");

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DRelacionesLaborales
                                {
                                    idRelacionesLaborales = reader.GetInt32(0),
                                    idEmpleado = reader.GetInt32(1),
                                    idTipoTramite = reader.GetInt32(2),
                                    fechaTramite = reader.GetDateTime(3),
                                    documentoUrl = reader.GetString(4)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        //error
                        MessageBox.Show(e.Message);
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




        public string Eiminar(DRelacionesLaborales RelacionesLaborales)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM RelacionesLaborales WHERE idRelacionesLaborales=@idRelacionesLaborales
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idRelacionesLaborales", RelacionesLaborales.idRelacionesLaborales);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la relacion laboral";

                        if(respuesta.Equals("OK"))
                        {
                            string query2 = @"
                                        SELECT statusCambio from [relacionesLaborales] where idTipoCambio = @idTipoCambio
                            ";

                            using (SqlCommand comm2 = new SqlCommand(query2, conn))
                            {

                                comm2.Parameters.AddWithValue("@idRelacionesLaborales", RelacionesLaborales.idRelacionesLaborales);

                                try
                                {
                                    conn.Open();
                                    respuesta = comm2.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la relacion laboral";

                                    
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
