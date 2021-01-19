using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;

namespace Metodos
{
    public class MSeleccion:DEmpleado
    {


        public string Insertar(DEmpleado Empleado,DSeleccion Seleccion, List<DIdiomaHablado> Idioma, List<DEducacion> Educacion)
        {
            string respuesta = "";

            string query = @"
                        INSERT INTO empleado(
                            idDepartamento,
                            nombre,
                            apellido,
                            cedula,
                            fechaNacimiento,
                            nacimiento,
                            direccion,
                            ciudad,
                            estado,
                            email,
                            telefono,
                            curriculum,
                            estadoLegal,
                            fechaCulminacion,
                            status
                        ) VALUES(
                            @idDepartamento,
                            @nombre,
                            @apellido,
                            @cedula,
                            @fechaNacimiento,
                            @nacimiento,
                            @direccion,
                            @ciudad,
                            @estado,
                            @email,
                            @telefono,
                            @curriculum,
                            @estadoLegal,
                            @fechaCulminacion,
                            @status
                        );
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@idDepartamento", Empleado.idDepartamento);
                    comm.Parameters.AddWithValue("@nombre", Empleado.nombre);
                    comm.Parameters.AddWithValue("@apellido", Empleado.apellido);
                    comm.Parameters.AddWithValue("@cedula", Empleado.cedula);
                    comm.Parameters.AddWithValue("@fechaNacimiento", Empleado.fechaNacimiento);
                    comm.Parameters.AddWithValue("@nacimiento", Empleado.nacimiento);
                    comm.Parameters.AddWithValue("@direccion", Empleado.direccion);
                    comm.Parameters.AddWithValue("@ciudad", Empleado.ciudad);
                    comm.Parameters.AddWithValue("@estado", Empleado.estado);
                    comm.Parameters.AddWithValue("@email", Empleado.email);
                    comm.Parameters.AddWithValue("@telefono", Empleado.telefono);
                    comm.Parameters.AddWithValue("@curriculum", Empleado.curriculum);
                    comm.Parameters.AddWithValue("@estadoLegal", Empleado.estadoLegal);
                    comm.Parameters.AddWithValue("@fechaCulminacion", Empleado.fechaCulminacion);
                    comm.Parameters.AddWithValue("@status", Empleado.status);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la seleccion";

                        this.idEmpleado = Convert.ToInt32(comm.Parameters["@idEmpleado"].Value);

                        if (respuesta.Equals("OK"))
                        {

                            string query2 = @"
                                        INSERT INTO seleccion(
                                            idEmpleado,
                                            idSeleccionador,
                                            fechaAplicacion,
                                            status,
                                            fechaRevision
                                        ) VALUES(
                                            @idEmpleado,
                                            @idSeleccionador,
                                            @fechaAplicacion,
                                            @status,
                                            @fechaRevision
                                        );
	                        ";

                            using (SqlCommand comm2 = new SqlCommand(query2, conn))
                            {
                                comm2.Parameters.AddWithValue("@idEmpleado", this.idEmpleado);
                                comm2.Parameters.AddWithValue("@idSeleccionador", Seleccion.idSeleccionador);
                                comm2.Parameters.AddWithValue("@fechaAplicacion", Seleccion.fechaAplicacion);
                                comm2.Parameters.AddWithValue("@status", Seleccion.status);
                                comm2.Parameters.AddWithValue("@fechaRevision", DateTime.Now);

                                respuesta = comm2.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la seleccion";

                                if (respuesta.Equals("OK"))
                                {
                                    int i = 0;
                                    foreach (DIdiomaHablado det in Idioma)
                                    {

                                        string query3 = @"
                                                    INSERT INTO idiomaHablado(
                                                        idIdioma,
                                                        idEmpleado,
                                                        nivel
                                                    ) VALUES(
                                                        @idIdioma,
                                                        @idEmpleado,
                                                        @nivel
                                                    );
	                                    ";

                                        using (SqlCommand comm3 = new SqlCommand(query3, conn))
                                        {
                                            comm3.Parameters.AddWithValue("@idIdioma", Idioma[i].idIdioma);
                                            comm3.Parameters.AddWithValue("@idEmpleado", this.idEmpleado);
                                            comm3.Parameters.AddWithValue("@nivel", Idioma[i].nivel);


                                            respuesta = comm3.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro del idioma";
                                            i++;
                                        }

                                        if (!respuesta.Equals("OK"))
                                        {
                                            break;
                                        }
                                    }


                                    if (respuesta.Equals("OK"))
                                    {
                                        int x = 0;
                                        foreach (DEducacion det in Educacion)
                                        {

                                            string query4 = @"
                                                        INSERT INTO educacion(
                                                            idEmpleado,
                                                            nombreCarrera,
                                                            nombreInstitucion,
                                                            fechaIngreso,
                                                            fechaEgreso
                                                        ) VALUES(
                                                            @idEmpleado,
                                                            @nombreCarrera,
                                                            @nombreInstitucion,
                                                            @fechaIngreso,
                                                            @fechaEgreso
                                                        );
	                                        ";

                                            using (SqlCommand comm4 = new SqlCommand(query4, conn))
                                            {
                                                comm4.Parameters.AddWithValue("@idEmpleado", this.idEmpleado);
                                                comm4.Parameters.AddWithValue("@nombreCarrera", Educacion[x].nombreCarrera);
                                                comm4.Parameters.AddWithValue("@nombreInstitucion", Educacion[x].nombreInstitucion);
                                                comm4.Parameters.AddWithValue("@fechaIngreso", Educacion[x].fechaIngreso);
                                                comm4.Parameters.AddWithValue("@fechaEgreso", Educacion[x].fechaEgreso);

                                                respuesta = comm4.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la educacion";
                                                x++;
                                            }

                                            if (!respuesta.Equals("OK"))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
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





        public string Anular(int IdSeleccion)
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
                    comm.Parameters.AddWithValue("@status",0);
                    comm.Parameters.AddWithValue("@idSeleccion", IdSeleccion);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se anulo la seleccion";
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


        public List<DEmpleado> Mostrar(string Buscar)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();


            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;

                    comm.CommandText = "SELECT e.cedula, e.apellido, e.nombre, e.estado, e.ciudad, e.email, e.telefono, e.curriculum, e.estadoLegal, s.fechaAplicacion, s.fechaRevision from [empleado] e inner join [seleccion] s on e.idEmpleado=s.idEmpleado where s.idEntrevistaodor = " + Buscar + " order by e.cedula";

                    try
                    {

                        conn.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ListaGenerica.Add(new DEmpleado
                                {
                                    cedula = reader.GetString(0),
                                    apellido = reader.GetString(1),
                                    nombre = reader.GetString(2),
                                    estado = reader.GetString(3),
                                    ciudad = reader.GetString(4),
                                    email = reader.GetString(5),
                                    telefono = reader.GetInt32(7),
                                    curriculum = reader.GetString(8),
                                    estadoLegal = reader.GetInt32(9),
                                    fechaAplicacion = reader.GetDateTime(10),
                                    fechaRevision = reader.GetDateTime(11)
                                });
                            }
                        }
                    }
                    catch
                    {
                        //error
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




        public string Despido(int IdEmpleado)
        {
            string respuesta = "";

            string query = @"
                        UPDATE empleado SET
                            status = @status,
                            fechaCulminacion = @fechaCulminacion
                        WHERE idEmpleado = @idEmpleado;
	        ";

            using (SqlConnection conn = new SqlConnection(Conexion.CadenaConexion))
            {

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@status", 3);
                    comm.Parameters.AddWithValue("@fechaCulminacion", DateTime.Now);

                    try
                    {
                        conn.Open();
                        respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se realizo el despido";
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
