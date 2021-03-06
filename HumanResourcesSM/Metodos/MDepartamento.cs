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

        public string Insertar(DDepartamento Departamento)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO departamento(
                            nombre,
                            descripcion
                        ) VALUES(
                            @nombre,
                            @descripcion
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", Departamento.nombre);
                    comm.Parameters.AddWithValue("@descripcion", Departamento.descripcion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del departamento";
                    }
                    catch (SqlException e)
                    {
                        respuesta = e.Message;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return respuesta;
                }
            }
        }


        public string Editar(DDepartamento Departamento)
        {
            string respuesta = "";

            string query = @"
                        UPDATE departamento SET
                            nombre = @nombre,
                            descripcion = @descripcion
                            WHERE idDepartamento = @idDepartamento;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", Departamento.nombre);
                    comm.Parameters.AddWithValue("@descripcion", Departamento.descripcion);

                    comm.Parameters.AddWithValue("@idDepartamento", Departamento.idDepartamento);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del departamento";
                    }
                    catch (SqlException e)
                    {
                        respuesta = e.Message;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return respuesta;
                }
            }
        }


        public string Eliminar(DDepartamento Departamento)
        {
            string respuesta = "";

            string query = @"
                        DELETE FROM departamento WHERE idDepartamento=@idDepartamento
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {

                    comm.Parameters.AddWithValue("@idDepartamento", Departamento.idDepartamento);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro del departamento";
                    }
                    catch (SqlException e)
                    {
                        respuesta = e.Message;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return respuesta;
                }
            }
        }


        public List<DDepartamento> Mostrar(string Buscar)
        {
            List<DDepartamento> ListaGenerica = new List<DDepartamento>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [departamento] where nombre like '" + Buscar + "%' order by nombre";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

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
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return ListaGenerica;
                }
            }

        }

        public List<DDepartamento> Encontrar(int Buscar)
        {
            List<DDepartamento> ListaGenerica = new List<DDepartamento>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [departamento] WHERE idDepartamento= " + Buscar + "";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

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
                    }
                    catch (SqlException e)
                    {
                        MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return ListaGenerica;
                }
            }

        }
    }
}
