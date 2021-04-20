using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MSeleccion:DEmpleado
    {
        #region QUERIES
        //seleccion
        private string queryInsertEmployee = @"
            INSERT INTO [Empleado] (
                idDepartamento,
                nombre,
                apellido,
                cedula,
                fechaNacimiento,
                nacionalidad,
                direccion,
                email,
                telefono,
                curriculoUrl,
                estadoLegal,
                status
            ) OUTPUT Inserted.idEmpleado
            VALUES (
                @idDepartamento,
                @nombre,
                @apellido,
                @cedula,
                @fechaNacimiento,
                @nacionalidad,
                @direccion,
                @email,
                @telefono,
                @curriculum,
                @estadoLegal,
                1
            );
        ";

        private string queryInsertSelection = @"
            INSERT INTO [Seleccion] (
                idEmpleado,
                idSeleccionador,
                idEntrevistador,
                fechaAplicacion,
                status,
                fechaRevision,
                nombrePuesto
            ) VALUES (
                @idEmpleado,
                @idSeleccionador,
                1,
                @fechaAplicacion,
                1,
                @fechaRevision,
                @nombrePuesto
            );
        ";

        private string queryInsertLanguage = @"
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

        private string queryInsertEducation = @"
            INSERT INTO [Educacion] (
                idEmpleado,
                nombreCarrera,
                nombreInstitucion,
                fechaIngreso,
                fechaEgreso
            ) VALUES (
                @idEmpleado,
                @nombreCarrera,
                @nombreInstitucion,
                @fechaIngreso,
                @fechaEgreso
            );
        ";

        //editar
        private string queryUpdateEmployee = @"
            UPDATE [Empleado] SET (
                idDepartamento,
                nombre,
                apellido,
                cedula,
                fechaNacimiento,
                nacionalidad,
                direccion,
                email,
                telefono,
                curriculoUrl,
                estadoLegal,
                status
            ) VALUES (
                @idDepartamento,
                @nombre,
                @apellido,
                @cedula,
                @fechaNacimiento,
                @nacionalidad,
                @direccion,
                @email,
                @telefono,
                @curriculoUrl,
                @estadoLegal,
                @status
            ) WHERE idEmpleado = @idEmpleado;
        ";

        private string queryUpdateSelection = @"
            UPDATE [Seleccion] SET (
                fechaAplicacion,
                nombrePuesto
            ) VALUES (
                @fechaAplicacion,
                @nombrePuesto
            ) WHERE idSeleccion = @idSeleccion;
	    ";

        //anular
        private string queryNull = @"
            UPDATE [Seleccion] SET
                status = 0
            WHERE idSeleccion = @idSeleccion;
	    ";

        //mostrar
        private string queryList = @"
            SELECT 
                e.cedula, 
                e.apellido, 
                e.nombre, 
                e.email, 
                e.telefono, 
                e.curriculum, 
                e.estadoLegal, 
                s.nombrePuesto, 
                s.fechaAplicacion, 
                s.fechaRevision 
            FROM [Empleado] e 
                INNER JOIN [Seleccion] s ON e.idEmpleado = s.idEmpleado 
            WHERE s.idEntrevistaodor = @idEntrevistador 
            ORDER BY e.cedula;
        ";

        //mostrar empleado
        private string queryListEmployeeName = @"
            SELECT * FROM [Empleado] 
            WHERE nombre + ' ' + apellido LIKE @nombreCompleto + '%';
        ";

        private string queryListEmployeID = @"
            SELECT * FROM [Empleado] 
            WHERE idEmpleado = @idEmpleado;
        ";

        private string queryListEmployeeDepartment = @"
            SELECT 
                idEmpleado, 
                idDepartamento, 
                (nombre + ' ' + apellido) AS nombreCompleto
            FROM [Empleado] 
            WHERE idDepartamento = @idDepartamento;
        ";

        private string queryListEmployeeDG = @"
            SELECT 
                em.idEmpleado, 
                (em.nombre + ' ' + em.apellido) AS nombreCompleto, 
                em.cedula, 
                p.pais, 
                d.nombre 
            FROM [Empleado] em 
                INNER JOIN [Departamento] d ON em.idDepartamento = d.idDepartamento 
                INNER JOIN [Paises] p ON em.nacionalidad = p.codigo 
            WHERE em.nombre + ' ' + em.apellido LIKE @nombreCompleto + '%';
        ";

        private string queryListEmployeeActive = @"
            SELECT * FROM [Empleado] 
            WHERE status = 1;
        ";

        //mostrar seleccion
        private string queryListSelection = @"
            SELECT * FROM [Seleccion] 
            WHERE idEmpleado = @idEmpleado;
        ";

        //mostrar paises
        private string queryListCountry = @"
            SELECT * FROM [Paises] 
            ORDER BY pais;
        ";

        //despido
        private string queryUpdateEmployeeFire = @"
            UPDATE [Empleado] SET
                status = 3,
                fechaCulminacion = @fechaCulminacion
            WHERE idEmpleado = @idEmpleado;
	    ";
        #endregion

        public string InsertarEmpleado(DEmpleado Empleado, DSeleccion Seleccion, List<DIdiomaHablado> Idioma, List<DEducacion> Educacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertEmployee, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDepartamento", Empleado.idDepartamento);
                comm.Parameters.AddWithValue("@nombre", Empleado.nombre);
                comm.Parameters.AddWithValue("@apellido", Empleado.apellido);
                comm.Parameters.AddWithValue("@cedula", Empleado.cedula);
                comm.Parameters.AddWithValue("@fechaNacimiento", Empleado.fechaNacimiento);
                comm.Parameters.AddWithValue("@nacionalidad", Empleado.nacionalidad);
                comm.Parameters.AddWithValue("@direccion", Empleado.direccion);
                comm.Parameters.AddWithValue("@email", Empleado.email);
                comm.Parameters.AddWithValue("@telefono", Empleado.telefono);
                comm.Parameters.AddWithValue("@curriculum", Empleado.curriculum);
                comm.Parameters.AddWithValue("@estadoLegal", Empleado.estadoLegal);
                int idEmpleado = (int)comm.ExecuteScalar();

                string respuesta = !String.IsNullOrEmpty(idEmpleado.ToString()) ? "OK" : "No se Ingresó el Registro del Empleado";

                if (!respuesta.Equals("OK")) return respuesta;
                return InsertarSeleccion(idEmpleado, Seleccion, Idioma, Educacion);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarSeleccion(int IdEmpleado, DSeleccion Seleccion, List<DIdiomaHablado> Idioma, List<DEducacion> Educacion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);
                comm.Parameters.AddWithValue("@idSeleccionador", Seleccion.idSeleccionador);
                comm.Parameters.AddWithValue("@fechaAplicacion", Seleccion.fechaAplicacion);
                comm.Parameters.AddWithValue("@fechaRevision", DateTime.Now);
                comm.Parameters.AddWithValue("@nombrePuesto", Seleccion.nombrePuesto);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresó el Registro de la Selección";

                if (!respuesta.Equals("OK")) return respuesta;

                if (!InsertarIdiomaHablado(IdEmpleado, Idioma).Equals("OK")) return InsertarIdiomaHablado(IdEmpleado, Idioma);
                return InsertarEducacion(IdEmpleado, Educacion);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarIdiomaHablado(int IdEmpleado, List<DIdiomaHablado> Idioma)
        {
            int i = 0;
            string respuesta = "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertLanguage, Conexion.ConexionSql);

                foreach (DIdiomaHablado det in Idioma)
                {
                    comm.Parameters.AddWithValue("@idIdioma", Idioma[i].idIdioma);
                    comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);
                    comm.Parameters.AddWithValue("@nivel", Idioma[i].nivel);

                    respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresó el Registro del Idioma";

                    if (!respuesta.Equals("OK")) break;
                    i++;
                }
                return respuesta;
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarEducacion(int IdEmpleado, List<DEducacion> Educacion)
        {
            int i = 0;
            string respuesta = "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertEducation, Conexion.ConexionSql);

                foreach (DEducacion det in Educacion)
                {
                    comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);
                    comm.Parameters.AddWithValue("@nombreCarrera", Educacion[i].nombreCarrera);
                    comm.Parameters.AddWithValue("@nombreInstitucion", Educacion[i].nombreInstitucion);
                    comm.Parameters.AddWithValue("@fechaIngreso", Educacion[i].fechaIngreso);
                    comm.Parameters.AddWithValue("@fechaEgreso", Educacion[i].fechaEgreso != null ? Educacion[i].fechaEgreso : DBNull.Value);

                    respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se ingreso el Registro de la educacion";
                    if (!respuesta.Equals("OK")) break;

                    i++;
                }
                return respuesta;
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string EditarEmpleado(DSeleccion Seleccion, DEmpleado Empleado)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateEmployee, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDepartamento", Empleado.idDepartamento);
                comm.Parameters.AddWithValue("@nombre", Empleado.nombre);
                comm.Parameters.AddWithValue("@apellido", Empleado.apellido);
                comm.Parameters.AddWithValue("@cedula", Empleado.cedula);
                comm.Parameters.AddWithValue("@fechaNacimiento", Empleado.fechaNacimiento);
                comm.Parameters.AddWithValue("@nacionalidad", Empleado.nacionalidad);
                comm.Parameters.AddWithValue("@direccion", Empleado.direccion);
                comm.Parameters.AddWithValue("@email", Empleado.email);
                comm.Parameters.AddWithValue("@telefono", Empleado.telefono);
                comm.Parameters.AddWithValue("@curriculum", Empleado.curriculum);
                comm.Parameters.AddWithValue("@estadoLegal", Empleado.estadoLegal);
                comm.Parameters.AddWithValue("@status", Empleado.status);
                comm.Parameters.AddWithValue("@idEmpleado", Empleado.idEmpleado);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Empleado";

                if (!respuesta.Equals("OK")) return respuesta;
                return EditarSeleccion(Seleccion);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string EditarSeleccion(DSeleccion Seleccion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@fechaAplicacion", Seleccion.fechaAplicacion);
                comm.Parameters.AddWithValue("@nombrePuesto", Seleccion.nombrePuesto);
                comm.Parameters.AddWithValue("@idSeleccion", Seleccion.idSeleccion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la Selección";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Anular(int IdSeleccion)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNull, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idSeleccion", IdSeleccion);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Anuló la Selección";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public List<DEmpleado> Mostrar(int IdEntrevistador)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEntrevistador", IdEntrevistador);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        cedula = reader.GetString(0),
                        apellido = reader.GetString(1),
                        nombre = reader.GetString(2),
                        email = reader.GetString(3),
                        telefono = reader.GetString(4),
                        curriculum = reader.GetString(5),
                        estadoLegal = reader.GetString(6),
                        nombrePuesto = reader.GetString(7),
                        fechaAplicacion = reader.GetDateTime(8),
                        fechaRevision = reader.GetDateTime(9)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> MostrarEmpleado(string NombreCompleto)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeName, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombreCompleto", NombreCompleto);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        idDepartamento = reader.GetInt32(1),
                        nombre = reader.GetString(2),
                        apellido = reader.GetString(3),
                        cedula = reader.GetString(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> MostrarEmpleadoByDepartamento(int IdDepartamento)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeDepartment, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idDepartamento", IdDepartamento);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        idDepartamento = reader.GetInt32(1),
                        nombre = reader.GetString(2),
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> MostrarEmpleadoDG(string NombreCompleto)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeDG, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombreCompleto", NombreCompleto);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        cedula = reader.GetString(2),
                        nacionalidad = reader.GetString(3),
                        nombreDepartamento = reader.GetString(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> EmpleadoEntrevista()
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeActive, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        idDepartamento = reader.GetInt32(1),
                        nombre = reader.GetString(2),
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
                        status = reader.GetInt32(13)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DSeleccion> EncontrarSeleccion(int IdEmpleado)
        {
            List<DSeleccion> ListaGenerica = new List<DSeleccion>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DSeleccion
                    {
                        idSeleccion = reader.GetInt32(0),
                        idEmpleado = reader.GetInt32(1),
                        idSeleccionador = reader.GetInt32(2),
                        idEntrevistador = reader.GetInt32(3),
                        fechaAplicacion = reader.GetDateTime(4),
                        status = reader.GetInt32(5),
                        fechaRevision = reader.GetDateTime(6),
                        nombrePuesto = reader.GetString(7)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> EncontrarEmpleado(int IdEmpleado)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        idDepartamento = reader.GetInt32(1),
                        nombre = reader.GetString(2),
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
                        status = reader.GetInt32(13)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DPais> MostrarPaises()
        {
            List<DPais> ListaGenerica = new List<DPais>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListCountry, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DPais
                    {
                        Codigo = reader.GetString(0),
                        Pais = reader.GetString(1)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public string Despido(int IdEmpleado)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateEmployeeFire, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@fechaCulminacion", DateTime.Now);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Realizó el Despido al Trabajador";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }
    }
}
