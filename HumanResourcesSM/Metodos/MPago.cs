using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;


namespace Metodos
{
    public class MPago:DPago
    {


        public string Insertar(DPago Pago, List<DDetallePago> Detalle)
        {

            //colocar bonificaciones

            string respuesta = "";


            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {


                string query = @"
                        INSERT INTO pago(
                            idPago,
                            idEmpleado,
                            fechaPago,
                            banco,
                            numeroReferencia,
                            cantidadHoras,
                            periodoPago,
                            montoTotal,
                            estado
                        ) VALUES(
                            @idPago,
                            @idEmpleado,
                            @fechaPago,
                            @banco,
                            @numeroReferencia,
                            @cantidadHoras,
                            @periodoPago,
                            @montoTotal,
                            @estado
                        );
	            ";

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idPago", Pago.idPago);
                    comm.Parameters.AddWithValue("@idEmpleado", Pago.idEmpleado);
                    comm.Parameters.AddWithValue("@fechaPago", Pago.fechaPago);
                    comm.Parameters.AddWithValue("@banco", Pago.banco);
                    comm.Parameters.AddWithValue("@numeroReferencia", Pago.numeroReferencia);
                    comm.Parameters.AddWithValue("@cantidadHoras", Pago.cantidadHoras);
                    comm.Parameters.AddWithValue("@periodoPago", Pago.periodoPago);
                    comm.Parameters.AddWithValue("@montoTotal", Pago.montoTotal);
                    comm.Parameters.AddWithValue("@estado", 1);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del pago";

                        this.idPago = Convert.ToInt32(comm.Parameters["@idPago"].Value);


                        if (respuesta.Equals("OK"))
                        {
                            int i = 0;
                            foreach (DDetallePago det in Detalle)
                            {

                                string query2 = @"
                                INSERT INTO detallePago(
                                        idPago,
                                        concepto,
                                        subtotal
                                    ) VALUES(
                                        @idPago,
                                        @concepto,
                                        @subTotal
                                    );
	                            ";

                                using (SqlCommand comm2 = new SqlCommand(query2, conn))
                                {
                                    comm2.Parameters.AddWithValue("@idPago", this.idPago);
                                    comm2.Parameters.AddWithValue("@concepto", Detalle[i].concepto);
                                    comm2.Parameters.AddWithValue("@subTotal", Detalle[i].subTotal);

                                    respuesta = comm2.ExecuteNonQuery() == 1 ? "OK" : "No se ingresaron los detalles del pago";

                                    i++;
                                }

                                if (!respuesta.Equals("OK"))
                                {
                                    break;
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





<<<<<<< Updated upstream
        public string Eliminar(DPago Pago)
=======
        public string Anular(DPago Pago)
>>>>>>> Stashed changes
        {
            string respuesta = "";

            string query = @"
                        UPDATE pago SET
                            estado = @estado
                        WHERE idPago = @idPago;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@estado", 0);
                    comm.Parameters.AddWithValue("@idPago", Pago.idPago);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del pago";
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



        public List<DPago> Mostrar(string Buscar)
        {
            List<DPago> ListaGenerica = new List<DPago>();


            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT p.idPago, p.fechaPago, e.cedula, e.apellido " + ' '+" e.nombre as NombreCompleto, p.numeroReferencia, p.banco, p.periodoPago, p.montoTotal, p.estado from [pago] p inner join [empleado] e on p.idEmpleado=e.idEmpleado where p.numeroReferencia = " + Buscar + " order by p.numeroReferencia";


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
                                    periodoPago = reader.GetDateTime(6),
                                    montoTotal = reader.GetDouble(7),
                                    estado = reader.GetInt32(8)
                                });
                            }
                        }
                    }
                    catch
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
