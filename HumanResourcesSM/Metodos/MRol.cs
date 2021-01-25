using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MRol:DUsuario
    {

        //Metodos

        //funcionando
        public List<DRol> Mostrar()
        {
            List<DRol> ListaGenerica = new List<DRol>();

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT * from [rol]";


                    //comm.Parameters.AddWithValue("@textoBuscar", "");

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DRol
                                {
                                    idRol = reader.GetInt32(0),
                                    nombre = reader.GetString(1),
                                    descripcion = reader.GetString(2),
                                    nivel = reader.GetInt32(3)
                                });
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        //error
                        MessageBox.Show(e.Message);
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
