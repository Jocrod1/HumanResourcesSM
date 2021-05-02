using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MTipoTramite:DTipoTramite
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [TipoTramite] (
                nombre,
                statusCambio
            ) VALUES (
                @nombre,
                @statusCambio
            );
	    ";

        private string queryUpdate = @"
            UPDATE [TipoTramite] SET
                nombre = @nombre,
                statusCambio = @statusCambio
            WHERE idTipoTramite = @idTipoTramite;
        ";

        private string queryDelete = @"
            DELETE * FROM [TipoTramite] 
            WHERE idTipoTramite = @idTipoTramite;
        ";

        private string queryListName = @"
            SELECT * FROM [TipoTramite] 
            WHERE nombre LIKE @nombre + '%' 
            ORDER BY nombre;
        ";

        private string queryListStatus = @"
            SELECT * 
            FROM [TipoTramite] 
        ";

        private string queryListID = @"
            SELECT * FROM [TipoTramite] 
            WHERE idTipoTramite = @idTipoTramite;
        ";
        #endregion

        public string Insertar(DTipoTramite TipoTramite)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", TipoTramite.nombre);
                comm.Parameters.AddWithValue("@statusCambio", TipoTramite.statusCambio);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresó el Registro del Tipo de Tramite";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DTipoTramite TipoTramite)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", TipoTramite.nombre);
                comm.Parameters.AddWithValue("@statusCambio", TipoTramite.statusCambio);
                comm.Parameters.AddWithValue("@idTipoTramite", TipoTramite.idTipoTramite);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del tipo de tramite";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(DTipoTramite TipoTramite)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idTipoTramite", TipoTramite.idTipoTramite);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Eliminó el Registro del Tipo de Trámite";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DTipoTramite> Mostrar(string Nombre)
        {
            List<DTipoTramite> ListaGenerica = new List<DTipoTramite>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListName, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", Nombre);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DTipoTramite
                    {
                        idTipoTramite = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        statusCambio = reader.GetString(2)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DTipoTramite> MostrarStatus()
        {
            List<DTipoTramite> ListaGenerica = new List<DTipoTramite>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListStatus, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DTipoTramite
                    {
                        idTipoTramite = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        statusCambio = reader.GetString(2)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DTipoTramite> Encontrar(int IdTipoTramite)
        {
            List<DTipoTramite> ListaGenerica = new List<DTipoTramite>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idTipoTramite", IdTipoTramite);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DTipoTramite
                    {
                        idTipoTramite = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        statusCambio = reader.GetString(2)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
