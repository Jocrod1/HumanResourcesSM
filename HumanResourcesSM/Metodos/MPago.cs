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
        #region QUERIES
        //insertar
        private string queryInsertPay = @"
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
                1
            );
	    ";

        private string queryInsertPayDetail = @"
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

        //anular
        private string queryNullPay = @"
            UPDATE pago SET
                estado = 0
            WHERE idPago = @idPago;
	    ";

        //mostrar
        private string queryListPay = @"
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
            WHERE p.numeroReferencia = @numeroReferencia
            ORDER BY p.numeroReferencia
        ";
        #endregion


        public string Insertar(DPago Pago, List<DDetallePago> Detalle)
        {
            string respuesta = "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertPay, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idPago", Pago.idPago);
                comm.Parameters.AddWithValue("@idEmpleado", Pago.idEmpleado);
                comm.Parameters.AddWithValue("@fechaPago", Pago.fechaPago);
                comm.Parameters.AddWithValue("@banco", Pago.banco);
                comm.Parameters.AddWithValue("@numeroReferencia", Pago.numeroReferencia);
                comm.Parameters.AddWithValue("@cantidadHoras", Pago.cantidadHoras);
                comm.Parameters.AddWithValue("@periodoInicio", Pago.periodoInicio);
                comm.Parameters.AddWithValue("@periodoFinal", Pago.periodoFinal);
                comm.Parameters.AddWithValue("@montoTotal", Pago.montoTotal);

                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del pago";

                this.idPago = Convert.ToInt32(comm.Parameters["@idPago"].Value);

                if (!respuesta.Equals("OK")) return "No se ingreso el Registro del pago";
                return InsertarDetallePago(this.idPago, Detalle);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarDetallePago(int IdPago, List<DDetallePago> Detalle)
        {
            string respuesta = "";
            int i = 0;

            try
            {
                foreach (DDetallePago det in Detalle)
                {
                    using SqlCommand comm = new SqlCommand(queryInsertPayDetail, Conexion.ConexionSql);
                    comm.Parameters.AddWithValue("@idPago", IdPago);
                    comm.Parameters.AddWithValue("@idDeuda", Detalle[i].idDeuda);
                    comm.Parameters.AddWithValue("@concepto", Detalle[i].concepto);
                    comm.Parameters.AddWithValue("@subTotal", Detalle[i].subTotal);

                    respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingresaron los detalles del pago";

                    if (!respuesta.Equals("OK")) break;
                    i++;
                }
            }
            catch (SqlException e) { respuesta = e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return respuesta; 
        }

        public string Anular(int IdPago)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNullPay, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idPago", IdPago);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del pago";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        public List<DPago> Mostrar(string numeroReferencia)
        {
            List<DPago> ListaGenerica = new List<DPago>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListPay, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@numeroReferencia", numeroReferencia);

                using SqlDataReader reader = comm.ExecuteReader();
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
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;

        }
    }
}
