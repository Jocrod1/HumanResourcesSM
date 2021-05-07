using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MEducacion:DEducacion
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [Educacion] (
                idEmpleado,
                nombreCarrera,
                nombreInstitucion,
                fechaIngreso,
                fechaEgreso
            ) VALUES (
                @idEmpleado,
                @nombreCarrera,
                @nombreInstitucion,
                @fechaIngreso,
                @fechaEgreso
            );
	    ";

        private string queryUpdate = @"
            UPDATE [Educacion] SET 
                idEmpleado = @idEmpleado,
                nombreCarrera = @nombreCarrera,
                nombreInstitucion = @nombreInstitucion,
                fechaIngreso = @fechaIngreso,
                fechaEgreso = @fechaEgreso
            WHERE idEducacion = @idEducacion;
	    ";

        private string queryDelete = @"
            DELETE FROM [Educacion] 
            WHERE idEducacion = @idEducacion;
	    ";

        private string queryListName = @"
            SELECT * FROM [Educacion]
            WHERE idEmpleado = @idEmpleado
            ORDER BY nombreCarrera;
        ";

        private string queryListID = @"
            SELECT * FROM [Educacion]
            WHERE idEducacion LIKE @idEducacion + '%'
            ORDER BY idEducacion;
        ";

        #endregion

        public string Insertar(DEducacion Educacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", Educacion.idEmpleado);
                comm.Parameters.AddWithValue("@nombreCarrera", Educacion.nombreCarrera);
                comm.Parameters.AddWithValue("@nombreInstitucion", Educacion.nombreInstitucion);
                comm.Parameters.AddWithValue("@fechaIngreso", Educacion.fechaIngreso);
                comm.Parameters.AddWithValue("@fechaEgreso", Educacion.fechaEgreso);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la Educacion";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DEducacion Educacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", Educacion.idEmpleado);
                comm.Parameters.AddWithValue("@nombreCarrera", Educacion.nombreCarrera);
                comm.Parameters.AddWithValue("@nombreInstitucion", Educacion.nombreInstitucion);
                comm.Parameters.AddWithValue("@fechaIngreso", Educacion.fechaIngreso);
                comm.Parameters.AddWithValue("@fechaEgreso", Educacion.fechaEgreso);
                comm.Parameters.AddWithValue("@idEducacion", Educacion.idEducacion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la educacion";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int IdEducacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEducacion", IdEducacion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la educacion";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DEducacion> Mostrar(int idEmpleado)
        {
            List<DEducacion> ListaGenerica = new List<DEducacion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListName, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEducacion
                    {
                        idEducacion = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        nombreCarrera = reader.GetString(2),
                        nombreInstitucion = reader.GetString(3),
                        fechaIngreso = reader.GetDateTime(4),
                        fechaEgreso = reader.IsDBNull(5) ? null : reader.GetDateTime(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEducacion> Encontrar(int IdEducacion)
        {
            List<DEducacion> ListaGenerica = new List<DEducacion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEducacion", IdEducacion);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DEducacion
                    {
                        idEducacion = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        nombreCarrera = reader.GetString(2),
                        nombreInstitucion = reader.GetString(3),
                        fechaIngreso = reader.GetDateTime(4),
                        fechaEgreso = reader.IsDBNull(5) ? null : reader.GetDateTime(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
