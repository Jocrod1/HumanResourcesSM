using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MMeta:DMeta
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [Meta] (
                idTipoMetrica,
                valorMeta,
                idEmpleado,
                idDepartamento,
                status,
                fechaInicio,
                fechaFinal,
                idUsuario
            ) VALUES (
                @idTipoMetrica,
                @valorMeta,
                @idEmpleado,
                @idDepartamento,
                @status,
                @fechaInicio,
                @fechaFinal,
                @idUsuario
            );
	    ";

        private string queryUpdate = @"
            UPDATE [Meta] SET
                idTipoMetrica = @idTipoMetrica,
                valorMeta = @valorMeta,
                idEmpleado = @idEmpleado,
                idDepartamento = @idDepartamento,
                status = @status,
                fechaInicio = @fechaInicio,
                fechaFinal = @fechaFinal,
                idUsuario = @idUsuario
            WHERE idMeta = @idMeta;
	    ";

        private string queryDelete = @"
            DELETE FROM [Meta] 
            WHERE idMeta = @idMeta
	    ";

        private string queryListCedula = @"
            SELECT 
                m.idMeta, 
                tm.nombre, 
                m.valorMeta, 
                e.cedula, 
                u.usuario, 
                d.nombre, 
                m.fechaInicio, 
                m.fechaFinal, 
                m.status 
            FROM [Meta] m 
                INNER JOIN [Empleado] e ON m.idEmpleado=e.idEmpleado 
                INNER JOIN [Usuario] u ON m.idUsuario=u.idUsuario 
                INNER JOIN [Departamento] d ON m.idDepartamento=d.idDepartamento 
                INNER JOIN [TipoMetrica] tm ON m.idTipoMetrica=tm.idTipoMetrica 
            WHERE e.cedula LIKE @cedula + '%'
            ORDER BY e.cedula;
        ";

        private string queryListByDepartment = @"
            SELECT 
                m.idMeta, 
                tp.nombre, 
                d.nombre, 
                m.valorMeta 
            FROM [Meta] m 
                INNER JOIN [Departamento] d ON m.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tp ON m.idTipoMetrica = tp.idTipoMetrica 
            WHERE m.status = 1 AND m.idDepartamento <> 1";

        private string queryListByEmployee = @"
            SELECT 
                m.idMeta, 
                tm.nombre, 
                m.valorMeta, 
                e.idEmpleado, 
                (e.nombre + ' ' + e.apellido) AS nombreEmpleado, 
                d.nombre 
            FROM [Meta] m 
                INNER JOIN [Empleado] e ON m.idEmpleado = e.idEmpleado 
                INNER JOIN [Departamento] d ON e.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tm ON m.idTipoMetrica = tm.idTipoMetrica 
            WHERE m.status = 1 AND m.idEmpleado <> 1";

        private string queryListAllByDepartment = @"
            SELECT 
                m.idMeta, 
                tp.nombre, 
                d.nombre, 
                m.valorMeta 
            FROM [Meta] m 
                INNER JOIN [Departamento] d ON m.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tp ON m.idTipoMetrica = tp.idTipoMetrica 
            WHERE m.status = 1 AND m.idDepartamento <> 1";

        private string queryListAllByEmployee = @"
            SELECT 
                m.idMeta, 
                tm.nombre, 
                m.valorMeta, 
                e.idEmpleado, 
                (e.nombre + ' ' + e.apellido) AS nombreEmpleado, 
                d.nombre 
            FROM [Meta] m 
                INNER JOIN [Empleado] e ON m.idEmpleado = e.idEmpleado 
                INNER JOIN [Departamento] d ON e.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tm ON m.idTipoMetrica = tm.idTipoMetrica 
            WHERE m.status = 1 AND m.idEmpleado <> 1";

        private string queryListByEmployeeDetail = @"
            SELECT 
                m.idMeta,
				m.idTipoMetrica,
				m.valorMeta,
				d.idDepartamento,
				m.idEmpleado,
				m.status,
				m.fechaInicio,
				m.fechaFinal,
				m.idUsuario,
                tm.nombre, 
                (e.nombre + ' ' + e.apellido) AS nombreEmpleado, 
                d.nombre
            FROM [Meta] m 
                INNER JOIN [Empleado] e ON m.idEmpleado = e.idEmpleado 
                INNER JOIN [Departamento] d ON e.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tm ON m.idTipoMetrica = tm.idTipoMetrica 
            WHERE m.idMeta = @idMeta;
        ";

        private string queryListByDepartmentDetail = @"
            SELECT 
                m.idMeta,
				m.idTipoMetrica,
				m.valorMeta,
				m.idDepartamento,
				m.status,
				m.fechaInicio,
				m.fechaFinal,
				m.idUsuario,
                tp.nombre, 
                d.nombre
            FROM [Meta] m 
                INNER JOIN [Departamento] d ON m.idDepartamento = d.idDepartamento 
                INNER JOIN [TipoMetrica] tp ON m.idTipoMetrica = tp.idTipoMetrica 
            WHERE m.idMeta = @idMeta;
        ";

        #endregion

        public string Insertar(DMeta Meta)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idTipoMetrica", Meta.idTipoMetrica);
                comm.Parameters.AddWithValue("@valorMeta", Meta.valorMeta);
                comm.Parameters.AddWithValue("@idEmpleado", Meta.idEmpleado);
                comm.Parameters.AddWithValue("@idDepartamento", Meta.idDepartamento);
                comm.Parameters.AddWithValue("@status", Meta.status);
                comm.Parameters.AddWithValue("@fechaInicio", Meta.fechaInicio);
                comm.Parameters.AddWithValue("@fechaFinal", Meta.fechaFinal);
                comm.Parameters.AddWithValue("@idUsuario", Meta.idUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la meta del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DMeta Meta)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", Meta.idMeta);
                comm.Parameters.AddWithValue("@idTipoMetrica", Meta.idTipoMetrica);
                comm.Parameters.AddWithValue("@valorMeta", Meta.valorMeta);
                comm.Parameters.AddWithValue("@idEmpleado", Meta.idEmpleado);
                comm.Parameters.AddWithValue("@idDepartamento", Meta.idDepartamento);
                comm.Parameters.AddWithValue("@status", Meta.status);
                comm.Parameters.AddWithValue("@fechaInicio", Meta.fechaInicio);
                comm.Parameters.AddWithValue("@fechaFinal", Meta.fechaFinal);
                comm.Parameters.AddWithValue("@idUsuario", Meta.idUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la meta del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int idMeta)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", idMeta);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la meta del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DMeta> Mostrar(string Cedula)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListCedula, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@cedula", Cedula);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        nombreMetrica = reader.GetString(1),
                        valorMeta = reader.GetDouble(2),
                        cedula = reader.GetString(3),
                        usuario = reader.GetString(4),
                        departamento = reader.GetString(5),
                        fechaInicio = reader.GetDateTime(6),
                        fechaFinal = reader.GetDateTime(7),
                        status = reader.GetInt32(8)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DMeta> MostrarByDepartamento(int BuscarDepartamento)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();
            string Searcher = (BuscarDepartamento > 0) ? (" AND m.idDepartamento = " + BuscarDepartamento) : "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListByDepartment + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        nombreMetrica = reader.GetString(1),
                        departamento = reader.GetString(2),
                        valorMeta = reader.GetDouble(3),
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DMeta> MostrarByEmpleado(int BuscarEmpleado)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();
            string Searcher = (BuscarEmpleado > 0) ? (" AND m.idEmpleado = " + BuscarEmpleado) : "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListByEmployee + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        nombreMetrica = reader.GetString(1),
                        valorMeta = reader.GetDouble(2),
                        idEmpleado = reader.GetInt32(3),
                        empleado = reader.GetString(4),
                        departamento = reader.GetString(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DMeta> MostrarTodoByDepartamento(int BuscarDepartamento, DateTime? FechaInicio = null, DateTime? FechaFinal = null)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();
            string Searcher = "";
            if(BuscarDepartamento > 0)
            {
                Searcher += " AND m.idDepartamento = " + BuscarDepartamento;
            }
            if(FechaInicio != null && FechaFinal != null)
            {
                Searcher += " AND m.fechaInicio <= '" + FechaFinal?.ToString("s") + "' AND m.fechaFinal >= '" + FechaInicio?.ToString("s") + "'";
            }

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListAllByDepartment + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        nombreMetrica = reader.GetString(1),
                        departamento = reader.GetString(2),
                        valorMeta = reader.GetDouble(3),
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DMeta> MostrarTodoByEmpleado(int BuscarEmpleado, DateTime? FechaInicio = null, DateTime? FechaFinal = null)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();
            string Searcher = "";
            if (BuscarEmpleado > 0)
            {
                Searcher += " AND m.idEmpleado = " + BuscarEmpleado;
            }
            if (FechaInicio != null && FechaFinal != null)
            {
                Searcher += " AND m.fechaInicio <= '" + FechaFinal?.ToString("s") + "' AND m.fechaFinal >= '" + FechaInicio?.ToString("s") + "'";
            }

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListAllByEmployee + Searcher, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        nombreMetrica = reader.GetString(1),
                        valorMeta = reader.GetDouble(2),
                        idEmpleado = reader.GetInt32(3),
                        empleado = reader.GetString(4),
                        departamento = reader.GetString(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DMeta> EncontrarByEmpleado(int IdMeta)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListByEmployeeDetail, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", IdMeta);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        idTipoMetrica = reader.GetInt32(1),
                        valorMeta = reader.GetDouble(2),
                        idDepartamento = reader.GetInt32(3),
                        idEmpleado = reader.GetInt32(4),
                        status = reader.GetInt32(5),
                        fechaInicio = reader.GetDateTime(6),
                        fechaFinal = reader.GetDateTime(7),
                        idUsuario = reader.GetInt32(8),
                        nombreMetrica = reader.GetString(9),
                        empleado = reader.GetString(10),
                        departamento = reader.GetString(11)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica; ;
        }


        public List<DMeta> EncontrarByDepartamento(int IdMeta)
        {
            List<DMeta> ListaGenerica = new List<DMeta>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListByDepartmentDetail, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", IdMeta);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DMeta
                    {
                        idMeta = reader.GetInt32(0),
                        idTipoMetrica = reader.GetInt32(1),
                        valorMeta = reader.GetDouble(2),
                        idDepartamento = reader.GetInt32(3),
                        status = reader.GetInt32(4),
                        fechaInicio = reader.GetDateTime(5),
                        fechaFinal = reader.GetDateTime(6),
                        idUsuario = reader.GetInt32(7),
                        nombreMetrica = reader.GetString(8),
                        departamento = reader.GetString(9),
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
