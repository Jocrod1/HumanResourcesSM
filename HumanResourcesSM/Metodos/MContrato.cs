using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Windows;
using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MContrato : DSeleccion
    {
        #region QUERIES
        private string queryUpdateSelection = @"
            UPDATE [Seleccion] SET
                idEntrevistador = @idEntrevistador
            WHERE idEmpleado = @idEmpleado;
	    ";

        private string queryNotHired = @"
            UPDATE [Empleado] SET
                status = 4
            WHERE idEmpleado = @idEmpleado;
	    ";

        private string queryInsert = @"
            INSERT INTO [Contrato] (
                idEmpleado,
                fechaContratacion,
                nombrePuesto,
                sueldo,
                horasSemanales
            ) VALUES (
                @idEmpleado,
                @fechaContratacion,
                @nombrePuesto,
                @sueldo,
                @horasSemanales
            );
	    ";

        private string queryUpdate = @"
            UPDATE [Contrato] SET 
                sueldo = @sueldo,
                horasSemanales = @horasSemanales
            WHERE idContrato = @idContrato;
        ";

        private string queryList = @"
            SELECT * FROM [Contrato]
            WHERE idEmpleado = @idEmpleado;
        ";

        private string queryReportNumberContract = @"
            SELECT
	            e.idEmpleado,
	            e.cedula,
	            CONCAT(e.nombre, ' ' , e.apellido) AS nombreEmpleado,
	            ISNULL((
		            SELECT COUNT(c.idEmpleado)
		            FROM [Contrato] c
			            INNER JOIN [Seleccion] s ON c.idEmpleado = s.idEmpleado
		            WHERE e.idEmpleado = s.idSeleccionador
			            AND c.fechaContratacion @primeraFecha AND @segundaFecha
	            ),0) AS numeroContrataciones,
	            ISNULL((
		            SELECT COUNT(s.idEmpleado)
		            FROM [Seleccion] s
		            WHERE e.idEmpleado = s.idSeleccionador
			            AND c.fechaContratacion @primeraFecha AND @segundaFecha
	            ),0) AS numeroSelecciones,
	            ISNULL((
		            SELECT TOP 1 c.fechaContratacion
		            FROM [Contrato] c
			            INNER JOIN [Seleccion] s ON c.idEmpleado = s.idEmpleado
		            WHERE e.idEmpleado = s.idSeleccionador
		            ORDER BY c.idContrato DESC
	            ), null) AS ultimaContratacion
            FROM [Empleado] e
            ORDER BY CONCAT(e.nombre, ' ' , e.apellido), numeroContrataciones  DESC
        ";
        #endregion

        public string AsignarEntrevistador(int IdEmpleado, int IdEntrevistador)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEntrevistador", IdEntrevistador);
                comm.Parameters.AddWithValue("@idSeleccion", IdEmpleado);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno al entrevistador";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string NoContratado(int IdEmpleado)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNotHired, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno cancelo la contratacion del empleado";
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return new MSeleccion().CambiarStatus(IdEmpleado, 4);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Insertar(DContrato Contrato)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", Contrato.idEmpleado);
                comm.Parameters.AddWithValue("@fechaContratacion", DateTime.Now);
                comm.Parameters.AddWithValue("@nombrePuesto", Contrato.nombrePuesto);
                comm.Parameters.AddWithValue("@sueldo", Contrato.sueldo);
                comm.Parameters.AddWithValue("@horasSemanales", Contrato.horasSemanales);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del Contrato";
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return new MSeleccion().Contratado(Contrato.idEmpleado);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DContrato Contrato)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@sueldo", Contrato.sueldo);
                comm.Parameters.AddWithValue("@horasSemanales", Contrato.horasSemanales);
                comm.Parameters.AddWithValue("@idContrato", Contrato.idContrato);


                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del Contrato";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DContrato> Encontrar(int IdContrato)
        {
            List<DContrato> ListaGenerica = new List<DContrato>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdContrato);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DContrato
                    {
                        idContrato = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        fechaContratacion = reader.GetDateTime(2),
                        nombrePuesto = reader.GetString(3),
                        sueldo = (double)reader.GetDecimal(4),
                        horasSemanales = reader.GetInt32(5),
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> ReporteNumeroContrato(DateTime PrimeraFecha, DateTime SegundaFecha)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryReportNumberContract, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@primeraFecha", PrimeraFecha);
                comm.Parameters.AddWithValue("@segundaFecha", SegundaFecha);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        cedula = reader.GetString(1),
                        nombre = reader.GetString(2),
                        numeroContrataciones = reader.GetInt32(3),
                        numeroSelecciones = reader.GetInt32(4),
                        ultimaContratacion = reader.GetDateTime(5) == null ? "Sin Contrataciones" : reader.GetDateTime(5).ToString("MM-dd-yyyy")
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
