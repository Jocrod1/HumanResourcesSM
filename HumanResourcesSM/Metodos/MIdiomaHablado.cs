using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MIdiomaHablado : DIdiomaHablado
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [IdiomaHablado] (
                idIdioma,
                idEmpleado,
                nivel
            ) VALUES (
                @idIdioma,
                @idEmpleado,
                @nivel
            );
	    ";

        private string queryUpdate = @"
            UPDATE [IdiomaHablado] SET 
                idIdioma = @idIdioma,
                idEmpleado = @idEmpleado,
                nivel = @nivel
            WHERE idIdiomaHablado = @idIdiomaHablado;
	    ";

        private string queryDelete = @"
            DELETE * FROM [IdiomaHablado] 
            WHERE idIdiomaHablado = @idIdiomaHablado
	    ";

        private string queryListEmployee = @"
            SELECT * FROM [IdiomaHablado]
            WHERE idEmpleado = @idEmpleado 
            ORDER BY idIdioma;
        ";

        private string queryListLanguage = @"
            SELECT * FROM [IdiomaHablado]
            WHERE idIdioma = @idIdioma 
            ORDER BY idIdioma;
        ";

        private string queryListLanguageGeneral = @"
            SELECT * FROM [Idioma] 
            ORDER BY nombre;
        ";

        private string queryListLanguageSpecific = @"
            SELECT * FROM [Idioma]
            WHERE idIdioma = @idIdioma
            ORDER BY nombre;
        ";
        #endregion

        public string Insertar(DIdiomaHablado IdiomaHablado)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idIdioma", IdiomaHablado.idIdioma);
                comm.Parameters.AddWithValue("@idEmpleado", IdiomaHablado.idEmpleado);
                comm.Parameters.AddWithValue("@nivel", IdiomaHablado.nivel);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del idioma hablado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DIdiomaHablado IdiomaHablado)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idIdioma", IdiomaHablado.idIdioma);
                comm.Parameters.AddWithValue("@idEmpleado", IdiomaHablado.idEmpleado);
                comm.Parameters.AddWithValue("@nivel", IdiomaHablado.nivel);
                comm.Parameters.AddWithValue("@idIdiomaHablado", IdiomaHablado.idIdiomaHablado);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del idioma hablado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int IdIdioma)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idIdiomaHablado", IdIdioma);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del idioma hablado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DIdiomaHablado> Mostrar(int IdEmpleado)
        {
            List<DIdiomaHablado> ListaGenerica = new List<DIdiomaHablado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployee, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DIdiomaHablado
                    {
                        idIdiomaHablado = reader.GetInt32(0),
                        idIdioma = reader.GetInt32(1),
                        idEmpleado = reader.GetInt32(2),
                        nivel = reader.GetInt32(3)
                    });
                } 
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DIdiomaHablado> Encontrar(int IdIdioma)
        {
            List<DIdiomaHablado> ListaGenerica = new List<DIdiomaHablado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListLanguage, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idIdioma", IdIdioma);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DIdiomaHablado
                    {
                        idIdiomaHablado = reader.GetInt32(0),
                        idIdioma = reader.GetInt32(1),
                        idEmpleado = reader.GetInt32(2),
                        nivel = reader.GetInt32(3)
                    });
                }

            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        //idiomas
        public List<DIdioma> MostrarIdioma()
        {
            List<DIdioma> ListaGenerica = new List<DIdioma>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListLanguageGeneral, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DIdioma
                    {
                        idIdioma = reader.GetInt32(0),
                        nombre = reader.GetString(1)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DIdioma> EncontrarIdioma(int IdIdioma)
        {
            List<DIdioma> ListaGenerica = new List<DIdioma>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListLanguageSpecific, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idIdioma", IdIdioma);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DIdioma
                    {
                        idIdioma = reader.GetInt32(0),
                        nombre = reader.GetString(1)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
