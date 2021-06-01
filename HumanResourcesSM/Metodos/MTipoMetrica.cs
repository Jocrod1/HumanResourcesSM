using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MTipoMetrica:DTipoMetrica
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [TipoMetrica] (
                nombre,
                idDepartamento,
                estado
            ) VALUES (
                @nombre,
                @idDepartamento,
                1
            );
	    ";

        private string queryUpdate = @"
            UPDATE [TipoMetrica] SET 
                nombre = @nombre,
                idDepartamento = @idDepartamento
            WHERE idTipoMetrica = @idTipoMetrica;
	    ";

        private string queryDelete = @"
            UPDATE [TipoMetrica] SET 
                estado = 0
            WHERE idTipoMetrica = @idTipoMetrica;
	    ";

        private string queryList = @"
            SELECT 
                tp.idTipoMetrica, 
                tp.nombre, 
                tp.idDepartamento, 
                d.nombre 
            FROM [TipoMetrica] tp 
                INNER JOIN [Departamento] d ON tp.idDepartamento = d.idDepartamento
        ";

        private string queryListID = @"
            SELECT * FROM [TipoMetrica] 
            WHERE idTipoMetrica = @idTipoMetrica AND estado <> 0;
        ";
        #endregion

        public string Insertar(DTipoMetrica TipoMetrica)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", TipoMetrica.nombre);
                comm.Parameters.AddWithValue("@idDepartamento", TipoMetrica.idDepartamento);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del tipo de métrica";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DTipoMetrica TipoMetrica)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", TipoMetrica.nombre);
                comm.Parameters.AddWithValue("@idDepartamento", TipoMetrica.idDepartamento);
                comm.Parameters.AddWithValue("@idTipoMetrica", TipoMetrica.idTipoMetrica);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Registro del Tipo de Métrica";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int IdTipoMetrica)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idTipoMetrica", IdTipoMetrica);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Eliminó el Registro del Tipo de Métrica";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DTipoMetrica> Mostrar(int Buscar)
        {
            List<DTipoMetrica> ListaGenerica = new List<DTipoMetrica>();
            string buscarByDepartamento = (Buscar) > -1 ? 
                (" WHERE estado <> 0 AND tp.idDepartamento = " + Buscar) : "WHERE estado <> 0";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList + buscarByDepartamento, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {

                    ListaGenerica.Add(new DTipoMetrica
                    {
                        idTipoMetrica = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        idDepartamento = reader.GetInt32(2),
                        nombreDepartamento = reader.GetString(3)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DTipoMetrica> Encontrar(int IdTipoMetrica)
        {
            List<DTipoMetrica> ListaGenerica = new List<DTipoMetrica>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idTipoMetrica", IdTipoMetrica);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DTipoMetrica
                    {
                        idTipoMetrica = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        idDepartamento = reader.GetInt32(2)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
