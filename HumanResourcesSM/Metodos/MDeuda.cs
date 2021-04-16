using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MDeuda:DDeuda
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [Deuda] (
                idEmpleado,
                monto,
                pagado,
                concepto,
                tipoDeuda,
                status
            ) VALUES (
                @idEmpleado,
                @monto,
                @pagado,
                @concepto,
                @tipoDeuda,
                1
            );
	    ";

        private string queryNull = @"
            UPDATE [Deuda] SET
                status = 0
            WHERE idDeuda = @idDeuda;
	    ";

        private string queryList = @"
            SELECT 
                d.idDeuda, 
                e.cedula, 
                (e.apellido + ' ' + e.nombre) as nombreCompleto, 
                de.nombre, 
                d.monto, 
                d.pagado,
                d.concepto,
                d.tipoDeuda, 
                d.status 
            FROM [Deuda] d 
                INNER JOIN [Empleado] e ON d.idEmpleado = e.idEmpleado
                INNER JOIN [Departamento] de ON de.idDepartamento = e.idDepartamento
            WHERE e.idEmpleado = @idEmpleado AND d.status LIKE @status AND d.status != 0
            ORDER BY d.idDeuda;
        ";
        #endregion

        public string Insertar(List<DDeuda> Detalle)
        {
            string respuesta = "";
            int i = 0;

            try
            {
                Conexion.ConexionSql.Open();

                foreach (DDeuda det in Detalle)
                {
                    using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                    comm.Parameters.AddWithValue("@idEmpleado", Detalle[i].idEmpleado);
                    comm.Parameters.AddWithValue("@monto", Detalle[i].monto);
                    comm.Parameters.AddWithValue("@pagado", Detalle[i].pagado);
                    comm.Parameters.AddWithValue("@concepto", Detalle[i].concepto);
                    comm.Parameters.AddWithValue("@tipoDeuda", Detalle[i].tipoDeuda);

                    respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresaron las Deudas del Empleado";
                    if (!respuesta.Equals("OK")) break;
                    i++;
                }
            }
            catch (SqlException e) { respuesta = e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return respuesta;
        }


        public string Anular(int IdDeuda)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNull, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDeuda", IdDeuda);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Anuló la Deuda";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DDeuda> MostrarDeudaEmpleado(int IdEmpleado, int? Status)
        {
            List<DDeuda> ListaGenerica = new List<DDeuda>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);
                comm.Parameters.AddWithValue("@status", Status);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DDeuda
                    {
                        idDeuda = reader.GetInt32(0),
                        cedula = reader.GetString(1),
                        nombreCompleto = reader.GetString(2),
                        departamento = reader.GetString(3),
                        monto = (double)reader.GetDecimal(4),
                        pagado = (double)reader.GetDecimal(5),
                        concepto = reader.GetString(6),
                        tipoDeuda = reader.GetInt32(7),
                        status = reader.GetInt32(8)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
