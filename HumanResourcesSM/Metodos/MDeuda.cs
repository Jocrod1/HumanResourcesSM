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
            FROM [deuda] d 
                INNER JOIN [empleado] e ON d.idEmpleado = e.idEmpleado
                INNER JOIN [departamento] de ON de.idDepartamento = e.idDepartamento
            WHERE d.status != 0";

        //mostrar
        private string queryGetDebt = @"
            SELECT 
                *
            FROM [deuda] d 
            WHERE d.idDeuda = @idDeuda 
        ";


        private string queryActivePerEmployee = @"
            SELECT 
				d.idDeuda,
				d.concepto,
				d.monto,
				d.pagado,
				d.tipoDeuda,
				e.cedula,
				CONCAT(e.apellido, ' ', e.nombre) AS nombreEmpleado
            FROM [Deuda] d
				INNER JOIN [Empleado] e ON e.idEmpleado=d.idEmpleado
			WHERE d.status != 0";

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


        public List<DDeuda> MostrarDeudaEmpleado(int IdEmpleado, int? Status, int? TipoDeuda = -1)
        {
            List<DDeuda> ListaGenerica = new List<DDeuda>();
            string Searcher = "";
            if (IdEmpleado > 0)
            {
                Searcher += " AND e.idEmpleado = " + IdEmpleado;
            }
            if (Status != null)
            {
                Searcher += " AND d.status = " + Status;
            }
            if(TipoDeuda > -1)
            {
                Searcher += " AND d.tipoDeuda = " + TipoDeuda;
            }

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList + Searcher, Conexion.ConexionSql);

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

        public List<DDeuda> Encontrar(int IdDeuda)
        {
            List<DDeuda> ListaGenerica = new List<DDeuda>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryGetDebt, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDeuda", IdDeuda);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DDeuda
                    {
                        idDeuda = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        monto = (double)reader.GetDecimal(2),
                        pagado = (double)reader.GetDecimal(3),
                        concepto = reader.GetString(4),
                        tipoDeuda = reader.GetInt32(5),
                        status = reader.GetInt32(6)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }



        public List<DDeuda> DeudasPorEmpleado(int IdEmpleado, int? Status, int? TipoDeuda)
        {
            List<DDeuda> ListaGenerica = new List<DDeuda>();

            try
            {
                Conexion.ConexionSql.Open();

                string Searcher = "";
                if (IdEmpleado > 0)
                {
                    Searcher += " AND e.idEmpleado = " + IdEmpleado;
                }
                if (Status != null)
                {
                    Searcher += " AND d.status = " + Status;
                }
                if (TipoDeuda > -1)
                {
                    Searcher += " AND d.tipoDeuda = " + TipoDeuda;
                }

                using SqlCommand comm = new SqlCommand(queryActivePerEmployee + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DDeuda
                    {
                        idDeuda = reader.GetInt32(0),
                        concepto = reader.GetString(1),
                        monto = (double)reader.GetDecimal(2),
                        pagado = (double)reader.GetDecimal(3),
                        tipoDeudaString = TipoDeudaString(reader.GetInt32(4)),
                        cedula = reader.GetString(5),
                        nombreCompleto = reader.GetString(6)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        private string TipoDeudaString(int tipoDeuda)
        {
            if (tipoDeuda == 0)
            {
                return "Bonificación";
            }
            else if (tipoDeuda == 1)
            {
                return "Deducción";
            }
            else return "ERROR";
        }
    }
}
