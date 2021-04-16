using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MDepartamento:DDepartamento
    {
        #region QUERIES
        private string queryInsert = @"
            INSERT INTO [Departamento] (
                nombre,
                descripcion
            ) VALUES (
                @nombre,
                @descripcion
            );
	    ";

        private string queryUpdate = @"
            UPDATE [Departamento] SET
                nombre = @nombre,
                descripcion = @descripcion
            WHERE idDepartamento = @idDepartamento;
	    ";

        private string queryDelete = @"
            DELETE FROM [Departamento] 
            WHERE idDepartamento = @idDepartamento;
	    ";

        private string queryListName = @"
            SELECT FROM [Departamento] 
            WHERE nombre LIKE '@nombre%'
            ORDER BY nombre;
        ";

        private string queryListID = @"
            SELECT FROM [Departamento] 
            WHERE idDepartamento = @idDepartamento;
        ";
        #endregion

        public string Insertar(DDepartamento Departamento)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", Departamento.nombre);
                comm.Parameters.AddWithValue("@descripcion", Departamento.descripcion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del Departamento";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DDepartamento Departamento)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", Departamento.nombre);
                comm.Parameters.AddWithValue("@descripcion", Departamento.descripcion);
                comm.Parameters.AddWithValue("@idDepartamento", Departamento.idDepartamento);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del departamento";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eliminar(int IdDepartamento)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDepartamento", IdDepartamento);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del departamento";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DDepartamento> Mostrar(string Nombre)
        {
            List<DDepartamento> ListaGenerica = new List<DDepartamento>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListName, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", Nombre);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DDepartamento
                    {
                        idDepartamento = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        descripcion = reader.GetString(2)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DDepartamento> Encontrar(int IdDepartamento)
        {
            List<DDepartamento> ListaGenerica = new List<DDepartamento>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDepartamento", IdDepartamento);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DDepartamento
                    {
                        idDepartamento = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        descripcion = reader.GetString(2)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
