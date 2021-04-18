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
            INSERT INTO [Pago] (
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
            INSERT INTO [DetallePago] (
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

        private string queryUpdateDebt = @"
            UPDATE [Deuda] SET
                pagado = pagado + @pagado
            WHERE idDeuda = @idDeuda;
	    ";

        private string queryUpdateDebtComplete = @"
            UPDATE [Deuda] SET
                status = 2
            WHERE pagado = monto AND idDeuda = @idDeuda;
        ";

        //anular
        private string queryNullPay = @"
            UPDATE [Pago] SET
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
            FROM [Pago] p 
                INNER JOIN [Empleado] e ON p.idEmpleado=e.idEmpleado 
            WHERE p.numeroReferencia = @numeroReferencia
            ORDER BY p.numeroReferencia;
        ";
        #endregion


        public string Insertar(DPago Pago, List<DDetallePago> DetallePago, List<double> MontoDeuda)
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

                if (!respuesta.Equals("OK")) return "No se ingreso el Registro del pago";

                this.idPago = Convert.ToInt32(comm.Parameters["@idPago"].Value);
                return InsertarDetallePago(this.idPago, DetallePago, MontoDeuda);
            }
            catch (SqlException e) { return e.Message; }
            catch (Exception ex) { return ex.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarDetallePago(int IdPago, List<DDetallePago> DetallePago, List<double> MontoDeuda)
        {
            string respuesta = "";
            int i = 0;

            foreach (DDetallePago det in DetallePago)
            {
                using SqlCommand comm = new SqlCommand(queryInsertPayDetail, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idPago", IdPago);
                comm.Parameters.AddWithValue("@idDeuda", DetallePago[i].idDeuda == 0 ? DetallePago[i].idDeuda : DBNull.Value);
                comm.Parameters.AddWithValue("@concepto", DetallePago[i].concepto);
                comm.Parameters.AddWithValue("@subTotal", DetallePago[i].subTotal);

                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingresaron los detalles del pago";

                if (!respuesta.Equals("OK") || DetallePago[i].idDeuda != 0 || !ActualizarDeuda(DetallePago[i].idDeuda, MontoDeuda[i]).Equals("OK"))
                    throw new NullReferenceException("Error en el Registro del Detalle del Pago");

                i++;
            }

            return respuesta; 
        }

        private string ActualizarDeuda(int IdDeuda, double MontoDeuda)
        {
            using SqlCommand comm = new SqlCommand(queryUpdateDebt, Conexion.ConexionSql);
            comm.Parameters.AddWithValue("@pagado", MontoDeuda);
            comm.Parameters.AddWithValue("@idDeuda", IdDeuda);

            string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó la Deuda";
            if (respuesta.Equals("OK")) return CompletarDeuda(IdDeuda);

            throw new NullReferenceException("Error en la Actualización de la Deuda");
        }

        private string CompletarDeuda(int IdDeuda)
        {
            using SqlCommand comm = new SqlCommand(queryUpdateDebtComplete, Conexion.ConexionSql);
            comm.Parameters.AddWithValue("@idDeuda", IdDeuda);

            return comm.ExecuteNonQuery() == 1 ? "OK" : "No Completó la Deuda";
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


        public List<DPago> Mostrar(string NumeroReferencia)
        {
            List<DPago> ListaGenerica = new List<DPago>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListPay, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@numeroReferencia", NumeroReferencia);

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
