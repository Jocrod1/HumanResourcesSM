using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace Metodos
{
    public class MPago:DPago
    {


        public string Insertar(DPago Pago, List<DDetallePago> Detalle)
        {

            //colocar bonificaciones

            string respuesta = "";

            int i = 0;


            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                try
                {
                    conn.Open();

                    #region Insertar Pago
                    string queryInsertPay = @"
                            INSERT INTO pago (
                                idPago,
                                idEmpleado,
                                fechaPago,
                                banco,
                                numeroReferencia,
                                cantidadHoras,
                                periodoInicio,
                                periodoFinal,
                                montoTotal,
                                estado
                            ) VALUES (
                                @idPago,
                                @idEmpleado,
                                @fechaPago,
                                @banco,
                                @numeroReferencia,
                                @cantidadHoras,
                                @periodoInicio,
                                @periodoFinal,
                                @montoTotal,
                                @estado
                            );
	                ";

                    using (SqlCommand comm = new SqlCommand(queryInsertPay, conn))
                    {
                        comm.Parameters.AddWithValue("@idPago", Pago.idPago);
                        comm.Parameters.AddWithValue("@idEmpleado", Pago.idEmpleado);
                        comm.Parameters.AddWithValue("@fechaPago", Pago.fechaPago);
                        comm.Parameters.AddWithValue("@banco", Pago.banco);
                        comm.Parameters.AddWithValue("@numeroReferencia", Pago.numeroReferencia);
                        comm.Parameters.AddWithValue("@cantidadHoras", Pago.cantidadHoras);
                        comm.Parameters.AddWithValue("@periodoInicio", Pago.periodoInicio);
                        comm.Parameters.AddWithValue("@periodoFinal", Pago.periodoFinal);
                        comm.Parameters.AddWithValue("@montoTotal", Pago.montoTotal);
                        comm.Parameters.AddWithValue("@estado", 1);


                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del pago";

                        this.idPago = Convert.ToInt32(comm.Parameters["@idPago"].Value);
                    }
                    #endregion

                    if (!respuesta.Equals("OK")) return "No se ingreso el Registro del pago";

                    foreach (DDetallePago det in Detalle)
                    {
                        #region Insertar Detalle Pago
                        string queryInsertPayDetail = @"
                                INSERT INTO detallePago (
                                    idPago,
                                    idDeuda,
                                    concepto,
                                    subtotal
                                ) VALUES (
                                    @idPago,
                                    @idDeuda,
                                    @concepto,
                                    @subTotal
                                );
	                    ";

                        using (SqlCommand comm = new SqlCommand(queryInsertPayDetail, conn))
                        {
                            comm.Parameters.AddWithValue("@idPago", this.idPago);
                            comm.Parameters.AddWithValue("@idDeuda", Detalle[i].idDeuda);
                            comm.Parameters.AddWithValue("@concepto", Detalle[i].concepto);
                            comm.Parameters.AddWithValue("@subTotal", Detalle[i].subTotal);

                            respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingresaron los detalles del pago";

                            i++;
                        }
                        #endregion

                        if (!respuesta.Equals("OK")) break;
                    }
                }
                catch (SqlException e) { respuesta = e.Message; }
                finally { if (conn.State == ConnectionState.Open) conn.Close(); }

                return respuesta;
            }
        }


        public string Anular(DPago Pago)
        {
            string respuesta = "";

            string queryNullPay = @"
                        UPDATE pago SET
                            estado = @estado
                        WHERE idPago = @idPago;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(queryNullPay, conn))
                {
                    comm.Parameters.AddWithValue("@estado", 0);
                    comm.Parameters.AddWithValue("@idPago", Pago.idPago);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del pago";
                    }
                    catch (SqlException e) { respuesta = e.Message; }
                    finally { if (conn.State == ConnectionState.Open) conn.Close(); }

                    return respuesta;
                }
            }
        }


        public List<DPago> Mostrar(string Buscar)
        {
            List<DPago> ListaGenerica = new List<DPago>();


            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = @"
                                    SELECT 
                                        p.idPago, 
                                        p.fechaPago, 
                                        e.cedula, 
                                        e.apellido + ' ' + e.nombre as NombreCompleto, 
                                        p.numeroReferencia, 
                                        p.banco, 
                                        p.periodoInicio,
                                        p.periodoFinal,
                                        p.montoTotal, 
                                        p.estado 
                                    FROM [pago] p 
                                        INNER JOIN [empleado] e on p.idEmpleado=e.idEmpleado 
                                    WHERE p.numeroReferencia = " + Buscar + @" 
                                    ORDER BY p.numeroReferencia";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DPago
                                {
                                    idPago = reader.GetInt32(0),
                                    fechaPago = reader.GetDateTime(1),
                                    cedula = reader.GetString(2),
                                    nombre = reader.GetString(3),
                                    numeroReferencia = reader.GetString(4),
                                    banco = reader.GetString(5),
                                    periodoInicio = reader.GetDateTime(6),
                                    periodoFinal = reader.GetDateTime(7),
                                    montoTotal = reader.GetDouble(8),
                                    estado = reader.GetInt32(9)
                                });
                            }
                        }
                    }
                    catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
                    finally { if (conn.State == ConnectionState.Open) conn.Close(); }

                    return ListaGenerica;
                }
            }

        }
    }
}
