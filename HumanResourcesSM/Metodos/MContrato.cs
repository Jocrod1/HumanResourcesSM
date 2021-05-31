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
	            u.idUsuario,
	            r.nombre,
				u.usuario,
	            ISNULL((
		            SELECT COUNT(s.idSeleccion)
		            FROM [Seleccion] s
					INNER JOIN [Empleado] e on e.idEmpleado = s.idEmpleado Inner Join [Contrato] c on c.idEmpleado = e.idEmpleado
		            WHERE u.idUsuario = s.idSeleccionador and s.status = 3 and s.fechaRevision >= @fechaInicio and s.fechaRevision <= @fechaFinal
	            ),0) AS numeroContrataciones,
	            ISNULL((
		            SELECT COUNT(s.idSeleccion)
		            FROM [Seleccion] s
		            WHERE u.idUsuario = s.idSeleccionador and s.fechaAplicacion >= @fechaInicio and s.fechaAplicacion <= @fechaFinal
	            ),0) AS numeroSelecciones,
	            ISNULL((
		            SELECT TOP 1 s.fechaRevision
		            FROM [Seleccion] s
		            WHERE u.idUsuario = s.idSeleccionador and s.fechaRevision >= @fechaInicio and s.fechaRevision <= @fechaFinal
		            ORDER BY s.idSeleccion DESC
	            ), null) AS ultimaContratacion
            FROM [Usuario] u
				INNER JOIN [Rol] r on r.idRol = u.idRol
            WHERE u.idUsuario <> 1";
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


        public string NoContratado(int IdEmpleado, int idEntrevistador)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNotHired, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno cancelo la contratacion del empleado";
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return new MSeleccion().CambiarStatus(IdEmpleado, 4, idEntrevistador);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Insertar(DContrato Contrato, int idEntrevistador)
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

                return new MSeleccion().Contratado(Contrato.idEmpleado, idEntrevistador);
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


        public List<DEmpleado> ReporteNumeroContrato(DateTime FechaInicio, DateTime FechaFinal)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryReportNumberContract, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@fechaInicio", FechaInicio.ToString("s"));
                comm.Parameters.AddWithValue("@fechaFinal", FechaFinal.ToString("s"));

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    var numeroContrataciones = reader.GetInt32(3);
                    var numeroSelecciones = reader.GetInt32(4);
                    if (numeroContrataciones == 0 && numeroSelecciones == 0)
                        continue;
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        cedula = reader.GetString(1),
                        nombre = reader.GetString(2),
                        numeroContrataciones = reader.GetInt32(3),
                        numeroSelecciones = reader.GetInt32(4),
                        ultimaContratacion = reader.IsDBNull(5) ? "Sin Contrataciones" : reader.GetDateTime(5).ToShortDateString()
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
