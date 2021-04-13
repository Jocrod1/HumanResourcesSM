using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MBonificacion:DBonificacion
    {


        public string Insertar(List<DBonificacion> Detalle)
        {
            string respuesta = "";

            int i = 0;

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {
                try
                {
                    foreach (DBonificacion det in Detalle)
                    {

                        string query = @"
                                INSERT INTO Bonificacion(
                                        idEmpleado,
                                        montoBonificacion,
                                        pagado,
                                        concepto,
                                        tipoDeuda,
                                        status
                                    ) VALUES(
                                        @idEmpleado,
                                        @montoBonificacion,
                                        @pagado,
                                        @concepto,
                                        @tipoDeuda,
                                        @status
                                    );
	                            ";

                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@idEmpleado", Detalle[i].idEmpleado);
                            comm.Parameters.AddWithValue("@montoBonificacion", Detalle[i].montoBonificacion);
                            comm.Parameters.AddWithValue("@pagado", Detalle[i].pagado);
                            comm.Parameters.AddWithValue("@concepto", Detalle[i].concepto);
                            comm.Parameters.AddWithValue("@tipoDeuda", Detalle[i].tipoDeuda);
                            comm.Parameters.AddWithValue("@status", 1);


                            respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingresaron los detalles de la bonificacion";

                            i++;
                        }

                        if (!respuesta.Equals("OK"))
                        {
                            break;
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




        public string Anular(DBonificacion Bonificacion)
        {
            string respuesta = "";

            string query = @"
                        UPDATE Bonificacion SET
                            status = @status
                        WHERE idBonificacion = @idBonificacion;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@status", 0);
                    comm.Parameters.AddWithValue("@idBonificacion", Bonificacion.idBonificacion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se anulo la bonificacion";
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
