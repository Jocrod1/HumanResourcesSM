using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MUsuario : DUsuario
    { 
        #region QUERIES 
        private string queryInsert = @"
            INSERT INTO [Usuario] (
                idRol,
                usuario,
                contraseña,
                confirmacion,
                entrevistando
            ) VALUES (
                @idRol,
                @usuario,
                @contraseña,
                @confirmacion,
                0
            );
	    ";

        private string queryUpdate = @"
            UPDATE [Usuario] SET 
                idRol = @idRol,
                usuario = @usuario,
                contraseña = @contraseña,
                confirmacion = @confirmacion
            WHERE idUsuario = @idUsuario;
        ";

        private string queryDelete = @"
            DELETE FROM [Usuario] 
            WHERE idUsuario = @idUsuario
        ";

        private string queryList = @"
            SELECT * FROM [Usuario] 
            WHERE usuario LIKE @usuario + '%' AND idUsuario <> 1 
            ORDER BY usuario";

        private string queryLogin = @"
            SELECT * FROM [Usuario] 
            WHERE usuario = @usuario AND contraseña = @contraseña AND idUsuario <> 1
        ";

        private string queryListID = @"
            SELECT * FROM [Usuario] 
            WHERE idUsuario = @idUsuario
        ";

        private string queryInterview = @"
            UPDATE [Usuario] SET
                entrevistando = @entrevistando
            WHERE idUsuario = @idUsuario;
        ";
        #endregion

        public string Insertar(DUsuario Usuario)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                comm.Parameters.AddWithValue("@contraseña", Usuario.contraseña);
                comm.Parameters.AddWithValue("@confirmacion", Usuario.confirmacion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresó el Registro del Usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DUsuario Usuario)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                comm.Parameters.AddWithValue("@contraseña", Usuario.contraseña);
                comm.Parameters.AddWithValue("@confirmacion", Usuario.confirmacion);
                comm.Parameters.AddWithValue("@idUsuario", Usuario.idUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Registro del Usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int IdUsuario)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Eliminó el Registro del Usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DUsuario> Mostrar(string Usuario)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = reader.GetString(3),
                        confirmacion = reader.GetString(4),
                        entrevistando = reader.GetInt32(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DUsuario> Login(string Usuario, string Contraseña)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryLogin, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);
                comm.Parameters.AddWithValue("@contraseña", Contraseña);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = reader.GetString(3),
                        confirmacion = reader.GetString(4),
                        entrevistando = reader.GetInt32(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DUsuario> Encontrar(int IdUsuario)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = reader.GetString(3),
                        confirmacion = reader.GetString(4),
                        entrevistando = reader.GetInt32(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public string Entrevistando(int IdUsuario, bool Entrevistando)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInterview, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@entrevistando", Entrevistando ? 1 : 0);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

    }
}
