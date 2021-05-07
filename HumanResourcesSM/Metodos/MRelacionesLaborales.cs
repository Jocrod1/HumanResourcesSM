using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MRelacionesLaborales : DRelacionesLaborales
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [RelacionesLaborales] (
                idEmpleado,
                idTipoTramite,
                fechaTramite,
                documentoUrl
            ) VALUES (
                @idEmpleado,
                @idTipoTramite,
                @fechaTramite,
                @documentoUrl
            );
	    ";

        private string queryUpdateEmployee = @"
            UPDATE [Empleado] SET
                estadoLegal = ISNULL((
                    SELECT TOP 1
                        t.statusCambio 
                    FROM [TipoTramite] t 
                        INNER JOIN [RelacionesLaborales] r ON t.idTipoTramite = r.idTipoTramite 
                    WHERE t.idTipoTramite = @idTipoTramite
                    ORDER BY r.idRelacionesLaborales DESC
                ), 'Sin Registro')
            WHERE idEmpleado = @idEmpleado;
        ";

        private string queryDelete = @"
            DELETE FROM [RelacionesLaborales] 
            WHERE idRelacionesLaborales = @idRelacionesLaborales;
        ";

        private string queryList = @"
            SELECT * FROM [RelacionesLaborales];
        ";

        private string queryListID = @"
            SELECT * FROM [RelacionesLaborales] 
            WHERE idRelacionesLaborales = @idRelacionesLaborales;
        ";
        #endregion

        public string Insertar(DRelacionesLaborales RelacionesLaborales)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", RelacionesLaborales.idEmpleado);
                comm.Parameters.AddWithValue("@idTipoTramite", RelacionesLaborales.idTipoTramite);
                comm.Parameters.AddWithValue("@fechaTramite", RelacionesLaborales.fechaTramite);
                comm.Parameters.AddWithValue("@documentoUrl", RelacionesLaborales.documentoUrl);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la evaluacion de la relacion laboral";

                if (!respuesta.Equals("OK")) return respuesta;
                return ActualizarEmpleado(RelacionesLaborales);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        public string Eliminar(DRelacionesLaborales RelacionesLaborales)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idRelacionesLaborales", RelacionesLaborales.idRelacionesLaborales);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la relacion laboral";

                if (!respuesta.Equals("OK")) return respuesta;
                return ActualizarEmpleado(RelacionesLaborales);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string ActualizarEmpleado(DRelacionesLaborales RelacionesLaborales)
        {
            try
            {
                using SqlCommand comm = new SqlCommand(queryUpdateEmployee, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idTipoTramite", RelacionesLaborales.idTipoTramite);
                comm.Parameters.AddWithValue("@idEmpleado", RelacionesLaborales.idEmpleado);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DRelacionesLaborales> Mostrar(int IdEmpleado, int IdTramite)
        {
            List<DRelacionesLaborales> ListaGenerica = new List<DRelacionesLaborales>();
            string queryEmpleadoTramite = MostrarEmpleadoTramiteQuery(IdEmpleado, IdTramite);

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand((queryList + queryEmpleadoTramite), Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DRelacionesLaborales
                    {
                        idRelacionesLaborales = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        idTipoTramite = reader.GetInt32(2),
                        fechaTramite = reader.GetDateTime(3),
                        documentoUrl = reader.GetString(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        private string MostrarEmpleadoTramiteQuery(int IdEmpleado, int IdTramite)
        {
            if (IdEmpleado == -1 && IdTramite == -1) return "";

            string query = "WHERE ";
            if (IdEmpleado > -1 && IdTramite > -1)
                query += "idEmpleado= " + IdEmpleado + " AND idTipoTramite= " + IdTramite;
            else if (IdEmpleado > -1)
                query += "idEmpleado= " + IdEmpleado;
            else if (IdTramite > -1)
                query += " idTipoTramite= " + IdTramite;

            return query;
        }


        public List<DRelacionesLaborales> Encontrar(int IdRelacionesLaborales)
        {
            List<DRelacionesLaborales> ListaGenerica = new List<DRelacionesLaborales>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idRelacionesLaborales", IdRelacionesLaborales);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DRelacionesLaborales
                    {
                        idRelacionesLaborales = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        idTipoTramite = reader.GetInt32(2),
                        fechaTramite = reader.GetDateTime(3),
                        documentoUrl = reader.GetString(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
