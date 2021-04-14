using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MEvaluacion:DEvaluacion
    {
        #region QUERIES
        //insertar
        private string queryInsert = @"
            INSERT INTO evaluacion(
                idUsuario,
                idMeta,
                valorEvaluado,
                observacion,
                status,
                fechaEvaluacion
            ) VALUES(
                @idUsuario,
                @idMeta,
                @valorEvaluado,
                @observacion,
                @status,
                @fechaEvaluacion
            );
	    ";

        private string queryUpdateGoal = @"
            UPDATE meta SET
                status = 2
            WHERE idMeta = @idMeta
        ";

        //editar
        private string queryUpdate = @"
            UPDATE evaluacion SET
                idUsuario = @idUsuario,
                idMeta = @idMeta,
                valorEvaluado = @valorEvaluado,
                observacion = @observacion,
                status = @status,
                fechaEvaluacion = @fechaEvaluacion
            WHERE idEvaluacion = @idEvaluacion
	    ";

        //eliminar
        private string queryDelete = @"
            DELETE FROM evaluacion 
            WHERE idEvaluacion = @idEvaluacion
	    ";

        //mostrar
        private string queryList = @"
            SELECT 
                ev.idEvaluacion, 
                ev.idMeta, 
                e.cedula, 
                ev.valorEvaluado, 
                ev.observacion, 
                ev.fechaEvaluacion, 
                ev.status 
            FROM evaluacion ev 
                INNER JOIN [empleado] e ON ev.idUsuario = e.idEmpleado 
            WHERE e.cedula LIKE @cedula 
            ORDER BY e.cedula
        ";
        
        #endregion

        public string Insertar(DEvaluacion Evaluacion)
        {
            string respuesta = "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", Evaluacion.idUsuario);
                comm.Parameters.AddWithValue("@idMeta", Evaluacion.idMeta);
                comm.Parameters.AddWithValue("@valorEvaluado", Evaluacion.valorEvaluado);
                comm.Parameters.AddWithValue("@observacion", Evaluacion.observacion);
                comm.Parameters.AddWithValue("@status", Evaluacion.status);
                comm.Parameters.AddWithValue("@fechaEvaluacion", Evaluacion.fechaEvaluacion);

                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la evaluacion del empleado";

                if (!respuesta.Equals("OK")) return respuesta;
                return ActualizarMeta(Evaluacion.idMeta);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string ActualizarMeta(int IdMeta)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateGoal, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idMeta", IdMeta);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Registro de la Meta";

            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DEvaluacion Evaluacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", Evaluacion.idUsuario);
                comm.Parameters.AddWithValue("@idMeta", Evaluacion.idMeta);
                comm.Parameters.AddWithValue("@valorEvaluado", Evaluacion.valorEvaluado);
                comm.Parameters.AddWithValue("@observacion", Evaluacion.observacion);
                comm.Parameters.AddWithValue("@status", Evaluacion.status);
                comm.Parameters.AddWithValue("@fechaEvaluacion", Evaluacion.fechaEvaluacion);
                comm.Parameters.AddWithValue("@idEvaluacion", Evaluacion.idEvaluacion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la evaluacion del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Eiminar(DEvaluacion Evaluacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEvaluacion", Evaluacion.idEvaluacion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se elimino el Registro de la evaluacion del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DEvaluacion> Mostrar(string Cedula)
        {
            List<DEvaluacion> ListaGenerica = new List<DEvaluacion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@cedula", Cedula);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEvaluacion
                    {
                        idEvaluacion = reader.GetInt32(0),
                        idMeta = reader.GetInt32(1),
                        cedula = reader.GetString(2),
                        valorEvaluado = reader.GetDouble(3),
                        observacion = reader.GetString(4),
                        fechaEvaluacion = reader.GetDateTime(5),
                        status = reader.GetInt32(6)
                    });
                }
                
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
