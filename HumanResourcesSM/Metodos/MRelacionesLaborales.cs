using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MRelacionesLaborales:DRelacionesLaborales
    {
        public string Insertar(DRelacionesLaborales RelacionesLaborales)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO relacionesLaborales(
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

                                    comm2.CommandText = "SELECT statusCambio from [relacionesLaborales] where idTipoCambio = @idTipoCambio";

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
                                                    UPDATE empleado SET
                                                        estadoLegal = @estadoLegal
                                                    WHERE idEmpleado = @idEmpleado;
	                                    ";

                                        if(TipoCambio != "")
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
                                        else
                                        {
                                            respuesta = "Tipo de Tramite erroneo";
                                        }

                                    }
                                    catch (SqlException e)
                                    {
                                        //error
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
    }
}
