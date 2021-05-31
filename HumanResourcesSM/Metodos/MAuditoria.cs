using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MAuditoria : DAuditoria
    {
        #region QUERIES
        

        private string queryList = @"
            SELECT
				a.idAuditoria,
				a.fecha,
				a.accion,
				a.descripcion,
				u.usuario,
				r.nombre
			FROM [auditoria] a
				INNER JOIN [Usuario] u on u.idUsuario = a.idUsuario
				INNER JOIN [Rol] r on r.idRol = u.idRol
            WHERE u.usuario LIKE @usuario + '%' 
				AND a.accion LIKE @accion + '%'";

        private string queryListActions = @"
            SELECT
				a.accion
			FROM [auditoria] a
            GROUP BY a.accion;
        ";

        #endregion


        public static string Insertar(DAuditoria Auditoria)
        {
            string queryInsert = @"
            INSERT INTO [auditoria] (
                idUsuario,
                accion,
                descripcion,
                fecha
            ) VALUES (
                @idUsuario,
                @accion,
                @descripcion,
                @fecha
            );
	        ";

            try
            {
                if (Conexion.ConexionSql.State == ConnectionState.Closed)
                    Conexion.ConexionSql.Open();



                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", Auditoria.idTrabajador);
                comm.Parameters.AddWithValue("@accion", Auditoria.accion);
                comm.Parameters.AddWithValue("@descripcion", Auditoria.descripcion);
                comm.Parameters.AddWithValue("@fecha", DateTime.Now);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "Error en el proceso de Auditoria";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DAuditoria> Mostrar(DateTime? FechaInicio, DateTime? FechaFinal, string Usuario, string Accion)
        {
            List<DAuditoria> ListaGenerica = new List<DAuditoria>();

            string searcher = "";

            if (FechaInicio != null && FechaFinal != null)
            {
                searcher += " AND a.fecha >= '" + FechaInicio?.ToString("s") + "' AND a.fecha <= '" + FechaFinal?.AddDays(1).ToString("s") + "'";
            }

            searcher += "ORDER BY a.idAuditoria DESC;";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList + searcher, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);
                comm.Parameters.AddWithValue("@accion", Accion);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DAuditoria
                    {
                        idAuditoria = reader.GetInt32(0),
                        fecha = reader.GetDateTime(1),
                        accion = reader.GetString(2),
                        descripcion = reader.GetString(3),
                        usuario = reader.GetString(4),
                        rolString = reader.GetString(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DAuditoria> MostrarAcciones()
        {
            List<DAuditoria> ListaGenerica = new List<DAuditoria>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListActions, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DAuditoria
                    {
                        accion = reader.GetString(0)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
