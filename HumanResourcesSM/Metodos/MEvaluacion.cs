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
                fechaEvaluacion,
                recomendacion
            ) VALUES (
                @idUsuario,
                @idMeta,
                @valorEvaluado,
                @observacion,
                @status,
                @fechaEvaluacion,
                @recomendacion
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
                observacion = @observacion,
                recomendacion = @recomendacion
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
                ev.recomendacion,
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

        private string queryEfficiencybyEmployee = @"
            SELECT 
                e.nombre,
				d.nombre,
				m.fechaInicio,
				m.fechaFinal,
				tm.nombre,
				m.valorMeta,
				ev.valorEvaluado,
				((ev.valorEvaluado/m.valorMeta)*100) as Rendimiento,
				ev.observacion,
                ev.recomendacion
            FROM [Evaluacion] ev
				INNER JOIN [Meta] m on m.idMeta = ev.idMeta
				INNER JOIN [Empleado] e ON e.idEmpleado = m.idEmpleado
				INNER JOIN [Departamento] d ON d.idDepartamento=e.idDepartamento
				INNER JOIN [TipoMetrica] tm ON tm.idDepartamento=e.idDepartamento
            WHERE m.status <> 0 AND m.idEmpleado <> 1";
        private string queryEfficiencybyDepartment = @"
            SELECT 
				d.nombre,
				m.fechaInicio,
				m.fechaFinal,
				tm.nombre,
				m.valorMeta,
				ev.valorEvaluado,
				((ev.valorEvaluado/m.valorMeta)*100) as Rendimiento,
				ev.observacion,
                ev.recomendacion
            FROM [Evaluacion] ev
				INNER JOIN [Meta] m on m.idMeta = ev.idMeta
				INNER JOIN [Departamento] d ON d.idDepartamento=m.idDepartamento
				INNER JOIN [TipoMetrica] tm ON tm.idDepartamento=d.idDepartamento
            WHERE m.status <> 0 AND m.idDepartamento <> 1";
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
                comm.Parameters.AddWithValue("@recomendacion", Evaluacion.recomendacion);

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
                comm.Parameters.AddWithValue("@recomendacion", Evaluacion.recomendacion);

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
                        recomendacion = reader.GetString(5),
                        fechaEvaluacion = reader.GetDateTime(6),
                        status = reader.GetInt32(7)
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
                        fechaEvaluacion = reader.GetDateTime(6),
                        recomendacion = reader.GetString(7)
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

            Searcher += " ORDER BY e.idEvaluacion DESC";

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

            Searcher += " ORDER BY ev.idEvaluacion DESC";


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


        public List<DEvaluacion> RendimientobyEmpleado(int BuscarEmpleado = -1, DateTime? FechaInicioev = null, DateTime? FechaFinalev = null)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            try
            {
                Conexion.ConexionSql.Open();
                string Searcher = "";
                if (BuscarEmpleado > 0)
                {
                    Searcher += " AND m.idEmpleado = " + BuscarEmpleado;
                }
                if (FechaInicioev != null && FechaFinalev != null)
                {
                    Searcher += " AND ev.fechaEvaluacion <= '" + FechaFinalev?.ToString("s") + "' AND ev.fechaEvaluacion >= '" + FechaInicioev?.ToString("s") + "'";
                }


                using SqlCommand comm = new SqlCommand(queryEfficiencybyEmployee + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();

                int i = 0;
                double SumRend = 0;
                while (reader.Read())
                {
                    i++;
                    var rend = Math.Truncate(reader.GetDouble(7) * 100) / 100;
                    SumRend += rend;
                    var RendAc = Math.Truncate((SumRend / i) * 100) / 100;

                    DateTime FechaInicio = reader.GetDateTime(2);
                    DateTime FechaFinal = reader.GetDateTime(3);
                    ListaGenerica.Add(new DEvaluacion
                    {
                        empleado = reader.GetString(0),
                        departamento = reader.GetString(1),
                        periodoInicial = reader.GetDateTime(2),
                        periodoFinal = reader.GetDateTime(3),
                        tipoMetrica = reader.GetString(4),
                        valorMeta = reader.GetDouble(5),
                        valorEvaluado = reader.GetDouble(6),
                        rendimiento = rend,
                        rendimientoAcumulado = SumRend,
                        periodoString = FechaInicio.ToShortDateString() + " - " + FechaFinal.ToShortDateString(),
                        observacion = reader.GetString(8),
                        recomendacion = reader.GetString(9),
                        usuarioEmisor = Globals.USUARIO_SISTEMA
                    });
                    ListaGenerica.Reverse();

                }

            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DEvaluacion> RendimientobyDepartamento(int BuscarEmpleado = -1, DateTime? FechaInicioev = null, DateTime? FechaFinalev = null)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            try
            {
                Conexion.ConexionSql.Open();
                string Searcher = "";
                if (BuscarEmpleado > 0)
                {
                    Searcher += " AND m.idEmpleado = " + BuscarEmpleado;
                }
                if (FechaInicioev != null && FechaFinalev != null)
                {
                    Searcher += " AND ev.fechaEvaluacion <= '" + FechaFinalev?.ToString("s") + "' AND ev.fechaEvaluacion >= '" + FechaInicioev?.ToString("s") + "'";
                }


                using SqlCommand comm = new SqlCommand(queryEfficiencybyDepartment + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();

                int i = 0;
                double SumRend = 0;
                while (reader.Read())
                {
                    i++;
                    var rend = Math.Truncate(reader.GetDouble(6) * 100) / 100;
                    SumRend += rend;
                    var RendAc = SumRend / i;

                    DateTime FechaInicio = reader.GetDateTime(1);
                    DateTime FechaFinal = reader.GetDateTime(2);

                    ListaGenerica.Add(new DEvaluacion
                    {
                        departamento = reader.GetString(0),
                        periodoInicial = reader.GetDateTime(1),
                        periodoFinal = reader.GetDateTime(2),
                        tipoMetrica = reader.GetString(3),
                        valorMeta = reader.GetDouble(4),
                        valorEvaluado = reader.GetDouble(5),
                        rendimiento = rend,
                        rendimientoAcumulado = SumRend,
                        periodoString = FechaInicio.ToShortDateString() + " - " + FechaFinal.ToShortDateString(),
                        observacion = reader.GetString(7),
                        recomendacion = reader.GetString(8),
                        usuarioEmisor = Globals.USUARIO_SISTEMA
                    });
                }
                ListaGenerica.Reverse();

            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
