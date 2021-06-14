using System;
using System.Collections.Generic;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MPrestacion:DPrestacion
    {

        public string Insertar(DPrestacion Prestacion)
        {
            string queryInsert = @"
                INSERT INTO [Prestacion] (
                    idEmpleado,
                    montoPresupuesto,
                    porcentajeOtorgado,
                    montoOtorgado,
                    razon,
                    fechaSolicitud,
                    estado
                ) VALUES (
                    @idEmpleado,
                    @montoPresupuesto,
                    @porcentajeOtorgado,
                    @montoOtorgado,
                    @razon,
                    @fechaSolicitud,
                    1
                );
	        ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", Prestacion.idEmpleado);
                comm.Parameters.AddWithValue("@montoPresupuesto", Prestacion.montoPresupuesto);
                comm.Parameters.AddWithValue("@porcentajeOtorgado", Prestacion.porcentajeOtorgado);
                comm.Parameters.AddWithValue("@montoOtorgado", Prestacion.montoOtorgado);
                comm.Parameters.AddWithValue("@razon", Prestacion.razon);
                comm.Parameters.AddWithValue("@fechaSolicitud", Prestacion.fechaSolicitud);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingreso el Registro del Presupuesto";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string AsignarPrestacion(DPrestacion Prestacion)
        {
            string queryUpdate = @"
                UPDATE [Prestacion] SET
                    porcentajeOtorgado = @porcentajeOtorgado,
                    montoOtorgado = @montoOtorgado,
                    estado = @estado
                WHERE idPrestacion = @idPrestacion;
	        ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idPrestacion", Prestacion.idPrestacion);
                comm.Parameters.AddWithValue("@porcentajeOtorgado", Prestacion.porcentajeOtorgado);
                comm.Parameters.AddWithValue("@montoOtorgado", Prestacion.montoOtorgado);
                comm.Parameters.AddWithValue("@estado", Prestacion.estado);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Asignó el Monto de Prestacion";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DPrestacion> Mostrar(int BuscarEmpleado, int Estado)
        {
            List<DPrestacion> ListaGenerica = new List<DPrestacion>();

            string queryList = @"
                SELECT 
                    p.idPrestacion, 
                    CONCAT(e.nombre, ' ', e.apellido) AS nombreCompleto, 
                    p.montoPresupuesto, 
                    p.porcentajeOtorgado, 
                    p.montoOtorgado, 
                    p.razon, 
                    p.fechaSolicitud,
                    p.estado
                FROM [Prestacion] p
                    INNER JOIN [Empleado] e ON p.idEmpleado = e.idEmpleado 
                WHERE p.estado = @estado";

            string searcher = (BuscarEmpleado > 0) ? (" and e.idEmpleado =" + BuscarEmpleado) : "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList + searcher, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@estado", Estado);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DPrestacion
                    {
                        idPrestacion = reader.GetInt32(0),
                        nombreEmpleado = reader.GetString(1),
                        montoPresupuesto = reader.GetDouble(2),
                        porcentajeOtorgado = reader.GetDouble(3),
                        montoOtorgado = reader.GetDouble(4),
                        razon = reader.GetString(5),
                        fechaSolicitudString = reader.GetDateTime(6).ToString("dd/MM/yyyy"),
                        estado = reader.GetInt32(7)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DPrestacion> Encontrar(int IdPrestacion)
        {
            List<DPrestacion> ListaGenerica = new List<DPrestacion>();

            string queryList = @"
                SELECT 
                    p.idPrestacion, 
                    CONCAT(e.nombre, ' ', e.apellido) AS nombreCompleto, 
                    s.nombrePuesto, 
                    p.fechaSolicitud,
                    c.fechaContratacion,
                    c.sueldo,
                    p.montoPresupuesto,
                    p.razon
                FROM [Prestacion] p
                    INNER JOIN [Empleado] e ON p.idEmpleado = e.idEmpleado 
                    INNER JOIN [Seleccion] s ON s.idEmpleado = e.idEmpleado
                    INNER JOIN [Contrato] c ON c.idEmpleado = e.idEmpleado
                WHERE p.idPrestacion = @idPrestacion;
            ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idPrestacion", IdPrestacion);


                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DPrestacion
                    {
                        idPrestacion = reader.GetInt32(0),
                        nombreEmpleado = reader.GetString(1),
                        nombrePuesto = reader.GetString(2),
                        fechaSolicitud = reader.GetDateTime(3),
                        fechaSolicitudString = reader.GetDateTime(3).ToString("dd/MM/yyyy"),
                        fechaContratacion = reader.GetDateTime(4),
                        fechaContratacionString = reader.GetDateTime(4).ToString("dd/MM/yyyy"),
                        sueldo = (double)reader.GetDecimal(5),
                        montoPresupuesto = reader.GetDouble(6),
                        razon = reader.GetString(7)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DEmpleado> MostrarEmpleadoByPrestaciones()
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            string queryListEmployeeContract = @"
                SELECT * FROM [Empleado] 
                WHERE status = 3
                ORDER BY idEmpleado DESC;
            ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeContract, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        idDepartamento = reader.GetInt32(1),
                        nombre = reader.GetString(2) + " " + reader.GetString(3),
                        apellido = reader.GetString(3),
                        cedula = reader.GetString(4),
                        fechaNacimiento = reader.GetDateTime(5),
                        nacionalidad = reader.GetString(6),
                        direccion = reader.GetString(7),
                        email = reader.GetString(8),
                        telefono = reader.GetString(9),
                        curriculum = reader.GetString(10),
                        estadoLegal = reader.GetString(11),
                        fechaCulminacion = !reader.IsDBNull(12) ? reader.GetDateTime(12) : null,
                        status = reader.GetInt32(13),
                        razonDespido = !reader.IsDBNull(14) ? reader.GetString(14) : ""
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
