using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MContrato : DSeleccion
    {
        #region QUERIES
        //asignar
        private string queryUpdateSelection = @"
            UPDATE seleccion SET
                idEntrevistador = @idEntrevistador
            WHERE idSeleccion = @idSeleccion;
	    ";

        //no contratado
        private string queryNotHired = @"
            UPDATE seleccion SET
                status = 2
            WHERE idSeleccion = @idSeleccion;
	    ";

        //insertar
        private string queryInsertContract = @"
            INSERT INTO contrato(
                idEmpleado,
                fechaContratacion,
                nombrePuesto,
                fechaCulminacion,
                sueldo,
                horasSemanales
            ) VALUES(
                @idEmpleado,
                @fechaContratacion,
                @nombrePuesto,
                @fechaCulminacion,
                @sueldo,
                @horasSemanales
            );
	    ";
        #endregion

        public string AsignarEntrevistador(int IdSeleccion, int IdEntrevistador)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEntrevistador", IdEntrevistador);
                comm.Parameters.AddWithValue("@idSeleccion", IdSeleccion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno al entrevistador";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string NoContratado(int IdSeleccion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNotHired, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idSeleccion", IdSeleccion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno cancelo la contratacion del empleado";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Insertar(DContrato Contrato)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertContract, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", Contrato.idEmpleado);
                comm.Parameters.AddWithValue("@fechaContratacion", DateTime.Now);
                comm.Parameters.AddWithValue("@nombrePuesto", Contrato.nombrePuesto);
                comm.Parameters.AddWithValue("@fechaCulminacion", Contrato.fechaCulminacion);
                comm.Parameters.AddWithValue("@sueldo", Contrato.sueldo);
                comm.Parameters.AddWithValue("@horasSemanales", Contrato.horasSemanales);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la educacion";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }
    }
}
