using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MContrato:DSeleccion
    {


        public string AsignarEntrevistador(int IdSeleccion, int IdEntrevistador)
        {
            string respuesta = "";

            string query = @"
                        UPDATE seleccion SET
                            idEntrevistador = @idEntrevistador
                        WHERE idSeleccion = @idSeleccion;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idEntrevistador", IdEntrevistador);
                    comm.Parameters.AddWithValue("@idSeleccion", IdSeleccion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno al entrevistador";
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


        public string NoContratado(int IdSeleccion)
        {
            string respuesta = "";

            string query = @"
                        UPDATE seleccion SET
                            status = @status
                        WHERE idSeleccion = @idSeleccion;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    //pd: no se si el 2 sea no contratado
                    comm.Parameters.AddWithValue("@status", 2);
                    comm.Parameters.AddWithValue("@idSeleccion", IdSeleccion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se asigno cancelo la contratacion del empleado";
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


         
        public string Insertar(DContrato Contrato)
        {
            string respuesta = "";

            string query = @"
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

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idEmpleado", Contrato.idEmpleado);
                    comm.Parameters.AddWithValue("@fechaContratacion", DateTime.Now);
                    comm.Parameters.AddWithValue("@nombrePuesto", Contrato.nombrePuesto);
                    comm.Parameters.AddWithValue("@fechaCulminacion", Contrato.fechaCulminacion);
                    comm.Parameters.AddWithValue("@sueldo", Contrato.sueldo);
                    comm.Parameters.AddWithValue("@horasSemanales", Contrato.horasSemanales);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la educacion";
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

    }
}
