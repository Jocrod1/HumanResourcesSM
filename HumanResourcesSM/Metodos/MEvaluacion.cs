using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MEvaluacion:DEvaluacion
    {
        #region QUERIES
        //insertar
        private string queryInsert = @"
            INSERT INTO [Evaluacion] (
                idUsuario,
                idMeta,
                valorEvaluado,
                observacion,
                status,
                fechaEvaluacion
            ) VALUES (
                @idUsuario,
                @idMeta,
                @valorEvaluado,
                @observacion,
                @status,
                @fechaEvaluacion
            );
	    ";

        private string queryUpdateGoal = @"
            UPDATE [Meta] SET
                status = @status
            WHERE idMeta = @idMeta;
        ";

        //editar
        private string queryUpdate = @"
            UPDATE [Evaluacion] SET
                idMeta = @idMeta,
                valorEvaluado = @valorEvaluado,
                observacion = @observacion
            WHERE idEvaluacion = @idEvaluacion;
	    ";

        //eliminar
        private string queryDelete = @"
            DELETE FROM [Evaluacion] 
            WHERE idEvaluacion = @idEvaluacion;
	    ";


        //mostrar
        private string queryList = @"
            SELECT 
                ev.idEvaluacion, 
                ev.idMeta, 
                e.cedula, 
                ev.valorEvaluado, 
                ev.observacion, 
                ev.fechaEvaluacion, 
                ev.status 
            FROM [Evaluacion] ev 
                INNER JOIN [Empleado] e ON ev.idUsuario = e.idEmpleado 
            WHERE e.cedula LIKE @cedula + '%'
            ORDER BY e.cedula;
        ";

        //Encontrar
        private string queryListDetail = @"
           SELECT 
                *
            FROM [Evaluacion]
            WHERE idEvaluacion = @idEvaluacion;
        ";

        private string queryListAllByDepartment = @"
            SELECT 
                e.idEvaluacion,
				m.idMeta, 
                tp.nombre, 
                d.nombre, 
                m.valorMeta,
				e.valorEvaluado
            FROM [Evaluacion] e
				INNER JOIN [Meta] m ON e.idMeta = m.idMeta
                INNER JOIN [Departamento] d ON m.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tp ON m.idTipoMetrica = tp.idTipoMetrica 
            WHERE m.status <> 0 AND m.idDepartamento <> 1";

        private string queryListAllByEmployee = @"
            SELECT 
                ev.idEvaluacion,
				m.idMeta, 
                tm.nombre, 
                m.valorMeta,
				ev.valorEvaluado,
                (e.nombre + ' ' + e.apellido) AS nombreEmpleado, 
                d.nombre 
            FROM [Evaluacion] ev
				INNER JOIN [Meta] m ON ev.idMeta = m.idMeta
                INNER JOIN [Empleado] e ON m.idEmpleado = e.idEmpleado 
                INNER JOIN [Departamento] d ON e.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tm ON m.idTipoMetrica = tm.idTipoMetrica 
            WHERE m.status <> 0 AND m.idEmpleado <> 1";

        private string queryEfficiency = @"
            SELECT 
				e.cedula,
                e.nombre,
				d.nombre,
				p.periodoInicio,
				p.periodoFinal,
				tm.nombre,
				m.valorMeta,
				ev.valorEvaluado,
				ev.observacion
            FROM [Empleado] e
				INNER JOIN [Departamento] d ON d.idDepartamento=e.idDepartamento
				INNER JOIN [Pago] p ON p.idEmpleado=e.idEmpleado
				INNER JOIN [TipoMetrica] tm ON tm.idDepartamento=d.idDepartamento
				INNER JOIN [Meta] m ON m.idEmpleado=e.idEmpleado
				INNER JOIN [Evaluacion] ev ON ev.idMeta=m.idMeta
        ";
        #endregion

        public string Insertar(DEvaluacion Evaluacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", Evaluacion.idUsuario);
                comm.Parameters.AddWithValue("@idMeta", Evaluacion.idMeta);
                comm.Parameters.AddWithValue("@valorEvaluado", Evaluacion.valorEvaluado);
                comm.Parameters.AddWithValue("@observacion", Evaluacion.observacion);
                comm.Parameters.AddWithValue("@status", Evaluacion.status);
                comm.Parameters.AddWithValue("@fechaEvaluacion", Evaluacion.fechaEvaluacion);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la evaluacion del empleado";

                if (!respuesta.Equals("OK")) return respuesta;
                return ActualizarMeta(Evaluacion.idMeta, 2);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string ActualizarMeta(int IdMeta, int Status)
        {
            try
            {
                using SqlCommand comm = new SqlCommand(queryUpdateGoal, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", IdMeta);
                comm.Parameters.AddWithValue("@status", Status);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Registro de la Meta";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DEvaluacion Evaluacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", Evaluacion.idMeta);
                comm.Parameters.AddWithValue("@valorEvaluado", Evaluacion.valorEvaluado);
                comm.Parameters.AddWithValue("@observacion", Evaluacion.observacion);
                comm.Parameters.AddWithValue("@idEvaluacion", Evaluacion.idEvaluacion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la evaluacion del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int IdEvaluacion, int IdMeta)
        {
            string respuesta = "";
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEvaluacion", IdEvaluacion);

                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la evaluacion del empleado";

                if (!respuesta.Equals("OK")) return respuesta;
                return ActualizarMeta(IdMeta, 1);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DEvaluacion> Mostrar(string Cedula)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@cedula", Cedula);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEvaluacion
                    {
                        idEvaluacion = reader.GetInt32(0),
                        idMeta = reader.GetInt32(1),
                        cedula = reader.GetString(2),
                        valorEvaluado = reader.GetDouble(3),
                        observacion = reader.GetString(4),
                        fechaEvaluacion = reader.GetDateTime(5),
                        status = reader.GetInt32(6)
                    });
                }
                
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEvaluacion> Encontrar(int idEvaluacion)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListDetail, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEvaluacion", idEvaluacion);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEvaluacion
                    {
                        idEvaluacion = reader.GetInt32(0),
                        idUsuario = reader.GetInt32(1),
                        idMeta = reader.GetInt32(2),
                        valorEvaluado = reader.GetDouble(3),
                        observacion = reader.GetString(4),
                        status = reader.GetInt32(5),
                        fechaEvaluacion = reader.GetDateTime(6)
                    });
                }

            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEvaluacion> MostrarTodoByDepartamento(int BuscarDepartamento, DateTime? FechaInicio = null, DateTime? FechaFinal = null)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();
            string Searcher = "";
            if (BuscarDepartamento > 0)
            {
                Searcher += " AND m.idDepartamento = " + BuscarDepartamento;
            }
            if (FechaInicio != null && FechaFinal != null)
            {
                Searcher += " AND e.fechaEvaluacion <= '" + FechaFinal?.ToString("s") + "' AND e.fechaEvaluacion >= '" + FechaInicio?.ToString("s") + "'";
            }

            try
            {
                Console.WriteLine(queryListAllByDepartment + Searcher);
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListAllByDepartment + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEvaluacion
                    {
                        idEvaluacion = reader.GetInt32(0),
                        idMeta = reader.GetInt32(1),
                        nombreMetrica = reader.GetString(2),
                        departamento = reader.GetString(3),
                        valorMeta = reader.GetDouble(4),
                        valorEvaluado = reader.GetDouble(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEvaluacion> MostrarTodoByEmpleado(int BuscarEmpleado, DateTime? FechaInicio = null, DateTime? FechaFinal = null)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();
            string Searcher = "";
            if (BuscarEmpleado > 0)
            {
                Searcher += " AND m.idEmpleado = " + BuscarEmpleado;
            }
            if (FechaInicio != null && FechaFinal != null)
            {
                Searcher += " AND ev.fechaEvaluacion <= '" + FechaFinal?.ToString("s") + "' AND ev.fechaEvaluacion >= '" + FechaInicio?.ToString("s") + "'";
            }

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListAllByEmployee + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEvaluacion
                    {
                        idEvaluacion = reader.GetInt32(0),
                        idMeta = reader.GetInt32(1),
                        nombreMetrica = reader.GetString(2),
                        valorMeta = reader.GetDouble(3),
                        valorEvaluado = reader.GetDouble(4),
                        empleado = reader.GetString(5),
                        departamento = reader.GetString(6)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEvaluacion> Rendimiento()
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryEfficiency, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEvaluacion
                    {
                        empleadoCedula = reader.GetString(0),
                        empleado = reader.GetString(1),
                        departamento = reader.GetString(2),
                        periodoInicial = reader.GetDateTime(3),
                        periodoFinal = reader.GetDateTime(4),
                        tipoMetrica = reader.GetString(5),
                        valorMeta = reader.GetInt32(6),
                        valorEvaluado = reader.GetInt32(7),
                        observacion = reader.GetString(8)
                    });
                }

            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
