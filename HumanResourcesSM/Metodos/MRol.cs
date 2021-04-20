using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MRol:DRol
    {
        #region QUERIES
        private string queryList = @"
            SELECT * FROM [Rol];
        ";
        #endregion

        public List<DRol> Mostrar()
        {
            List<DRol> ListaGenerica = new List<DRol>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
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
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
