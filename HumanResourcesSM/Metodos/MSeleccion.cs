using System;
using System.Collections.Generic;
using System.Text;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Metodos
{
    public class MSeleccion : DSeleccion
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
            UPDATE [Empleado] SET 
                idDepartamento = @idDepartamento,
                nombre = @nombre,
                apellido = @apellido,
                cedula = @cedula,
                fechaNacimiento = @fechaNacimiento,
                nacionalidad = @nacionalidad,
                direccion = @direccion,
                email = @email,
                telefono = @telefono,
                curriculoUrl = @curriculum,
                estadoLegal = @estadoLegal
            WHERE idEmpleado = @idEmpleado;
        ";

        private string queryUpdateSelection = @"
            UPDATE [Seleccion] SET 
                fechaAplicacion = @fechaAplicacion,
                nombrePuesto = @nombrePuesto
            WHERE idSeleccion = @idSeleccion;
	    ";

        //anular
        private string queryNullEmployee = @"
            UPDATE [Empleado] SET
                status = 0
            WHERE idEmpleado = @idEmpleado;
	    ";

        //modificar status
        private string queryChangeStatusSelection = @"
            UPDATE [Seleccion] SET
                status = @status
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
                e.razonDespido,
                s.nombrePuesto, 
                s.fechaAplicacion, 
                s.fechaRevision,
                s.razon
            FROM [Empleado] e 
                INNER JOIN [Seleccion] s ON e.idEmpleado = s.idEmpleado 
            WHERE s.idEntrevistaodor = @idEntrevistador AND e.idEmpleado <> 1
            ORDER BY e.cedula;
        ";

        //mostrar empleado
        private string queryListEmployeeName = @"
            SELECT * FROM [Empleado] 
            WHERE nombre + ' ' + apellido LIKE @nombreCompleto + '%' AND idEmpleado <> 1 AND status = 3
        ";


        private string queryListEmployeID = @"
            SELECT TOP 1 * FROM [Empleado] 
            WHERE idEmpleado = @idEmpleado
            ORDER BY idEmpleado DESC;
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
                em.nacionalidad, 
                d.nombre,
                em.status
            FROM [Empleado] em 
                INNER JOIN [Departamento] d ON em.idDepartamento = d.idDepartamento 
                INNER JOIN [Paises] p ON em.nacionalidad = p.codigo 
            WHERE em.nombre + ' ' + em.apellido LIKE @nombreCompleto + '%' and em.status <> 0 and em.status <> 4 and em.status <> 5
        ";


        private string queryListToFireDG = @"
            SELECT 
                em.idEmpleado, 
                (em.nombre + ' ' + em.apellido) AS nombreCompleto, 
                em.cedula, 
                p.pais, 
                d.nombre 
            FROM [Empleado] em 
                INNER JOIN [Departamento] d ON em.idDepartamento = d.idDepartamento 
                INNER JOIN [Paises] p ON em.nacionalidad = p.codigo 
            WHERE em.nombre + ' ' + em.apellido LIKE @nombreCompleto + '%' and em.status = 3
        ";


        //mostrar seleccion
        private string queryListSelection = @"
            SELECT TOP 1 * FROM [Seleccion] 
            WHERE idEmpleado = @idEmpleado
            ORDER BY idSeleccion DESC;
        ";

        //mostrar paises
        private string queryListCountry = @"
            SELECT * FROM [Paises] 
            ORDER BY pais;
        ";

        //despido
        private string queryUpdateEmployeeFire = @"
            UPDATE [Empleado] SET
                status = 5,
                fechaCulminacion = @fechaCulminacion,
                razonDespido = @razonDespido
            WHERE idEmpleado = @idEmpleado;
	    ";

        private string queryUpdateEmployeeContract = @"
            UPDATE [Empleado] SET
                status = 3
            WHERE idEmpleado = @idEmpleado;
	    ";


        //repetido
        private string queryUpdateEmployeeSelection = @"
            UPDATE [Empleado] SET
                status = 1
            WHERE idEmpleado = @idEmpleado;
	    ";

        private string queryIDCardRepeated = @"
            SELECT status, idEmpleado, (nombre + ' ' + apellido) as nombre FROM [Empleado] 
            WHERE cedula = @cedula;
        ";


        //reportes
        private string queryListInterviewPerUser = @"
            SELECT 
				e.cedula,
                (e.nombre + ' ' + e.apellido) as nombreCompleto,
				e.status,
				s.fechaRevision,
				u.usuario
            FROM [Seleccion] s
				INNER JOIN [Empleado] e ON e.idEmpleado=s.idEmpleado
                INNER JOIN [Usuario] u ON u.idUsuario=s.idEntrevistador
			WHERE u.idUsuario = @idUsuario and s.fechaRevision >= @fechaInicio and s.fechaRevision <= @fechaFinal";

        private string queryListSelectionPerUser = @"
            SELECT 
				e.cedula,
                (e.nombre + ' ' + e.apellido) as nombreCompleto,
				e.status,s
				s.fechaAplicacion,
				u.usuario
            FROM [Seleccion] s
				INNER JOIN [Empleado] e ON e.idEmpleado=s.idEmpleado
                INNER JOIN [Usuario] u ON u.idUsuario=s.idSeleccionador
			WHERE u.idUsuario = @idUsuario and s.fechaAplicacion >= @fechaInicio and s.fechaAplicacion <= @fechaFinal";

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

                if (!respuesta.Equals("OK")) 
                    return respuesta;

                respuesta = InsertarSeleccion(idEmpleado, Seleccion);
                if (!respuesta.Equals("OK"))
                    return respuesta;

                respuesta = InsertarIdiomaHablado(idEmpleado, Idioma);
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return InsertarEducacion(idEmpleado, Educacion);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarSeleccion(int IdEmpleado, DSeleccion Seleccion)
        {
            try
            {
                if (Conexion.ConexionSql.State == ConnectionState.Closed)
                    Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsertSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);
                comm.Parameters.AddWithValue("@idSeleccionador", Seleccion.idSeleccionador);
                comm.Parameters.AddWithValue("@fechaAplicacion", Seleccion.fechaAplicacion);
                comm.Parameters.AddWithValue("@fechaRevision", DateTime.Now);
                comm.Parameters.AddWithValue("@nombrePuesto", Seleccion.nombrePuesto);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresó el Registro de la Selección";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarIdiomaHablado(int IdEmpleado, List<DIdiomaHablado> Idioma)
        {
            int i = 0;
            string respuesta = "";

            if (Conexion.ConexionSql.State == ConnectionState.Closed)
                Conexion.ConexionSql.Open();

            try
            {
                foreach (DIdiomaHablado det in Idioma)
                {
                    using SqlCommand comm = new SqlCommand(queryInsertLanguage, Conexion.ConexionSql);
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

                foreach (DEducacion det in Educacion)
                {
                    using SqlCommand comm = new SqlCommand(queryInsertEducation, Conexion.ConexionSql);


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
                using SqlCommand comm = new SqlCommand(queryUpdateSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@fechaAplicacion", Seleccion.fechaAplicacion);
                comm.Parameters.AddWithValue("@nombrePuesto", Seleccion.nombrePuesto);
                comm.Parameters.AddWithValue("@idSeleccion", Seleccion.idSeleccion);
                comm.Parameters.AddWithValue("@razon", Seleccion.razon);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro de la Selección";
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
                        razonDespido = reader.GetString(7),
                        nombrePuesto = reader.GetString(8),
                        fechaAplicacion = reader.GetDateTime(9),
                        fechaRevision = reader.GetDateTime(10),
                        razonSeleccion = reader.GetString(11)
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
                        nombre = reader.GetString(2)
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
                        nombreDepartamento = reader.GetString(4),
                        status = reader.GetInt32(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DEmpleado> MostrarDespedirDG(string NombreCompleto)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListToFireDG, Conexion.ConexionSql);
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


        public List<DEmpleado> EmpleadoEntrevista(int IdEmpleado)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            string queryListEmployeeActive = @"
                SELECT * FROM [Empleado] 
                WHERE idEmpleado <> 1 AND idEmpleado = @idEmpleado
            ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeActive, Conexion.ConexionSql);
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


        public List<DSeleccion> EncontrarSeleccion(int IdEmpleado)
        {
            List<DSeleccion> ListaGenerica = new List<DSeleccion>();

            try
            {
                if(Conexion.ConexionSql.State == ConnectionState.Closed)
                    Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListSelection, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
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
                        nombrePuesto = reader.GetString(7),
                        razon = !reader.IsDBNull(8) ? reader.GetString(8) : ""
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
                if (reader.Read())
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
                        status = reader.GetInt32(13),
                        razonDespido = !reader.IsDBNull(14) ? reader.GetString(14) : ""
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

        public string AnularEmpleado(int IdEmpleado)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryNullEmployee, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Anuló el Empleado";
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return CambiarStatus(IdEmpleado, 0);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Despido(int IdEmpleado, string Razon)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateEmployeeFire, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@fechaCulminacion", DateTime.Now);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);
                comm.Parameters.AddWithValue("@razonDespido", Razon);


                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Realizó el Despido al Trabajador";
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return CambiarStatus(IdEmpleado, 5);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        public string Contratado(int IdEmpleado, int idEntrevistador, string Razon)
        {
            try
            {
                if (Conexion.ConexionSql.State == ConnectionState.Closed)
                    Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdateEmployeeContract, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Realizó el Despido al Trabajador";
                if (!respuesta.Equals("OK"))
                    return respuesta;

                return CambiarStatus(IdEmpleado, 3, idEntrevistador, Razon);
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string CambiarStatus(int IdEmpleado, int Status, int idEntrevistador = 0, string Razon = "")
        {
            var resp = EncontrarSeleccion(IdEmpleado);

            if (resp.Count < 1)
                return "IdEmpleado no Encontrado";

            if (Conexion.ConexionSql.State == ConnectionState.Closed)
                Conexion.ConexionSql.Open();

            string queryChangeStatusWhithEntrevistador = "";

            if ((Status == 3 || Status == 4) & idEntrevistador > 0)
            {
                queryChangeStatusWhithEntrevistador = @"
                    UPDATE [Seleccion] SET
                        status = @status,
                        idEntrevistador = @idEntrevistador,
                        razon = @razon
                    WHERE idSeleccion = @idSeleccion;
	            ";
            }
            else
            {
                queryChangeStatusWhithEntrevistador = queryChangeStatusSelection;
            }


            using SqlCommand comm = new SqlCommand(queryChangeStatusWhithEntrevistador, Conexion.ConexionSql);
            comm.Parameters.AddWithValue("@idSeleccion", resp[0].idSeleccion);
            comm.Parameters.AddWithValue("@status", Status);
            if ((Status == 3 || Status == 4) && idEntrevistador > 0)
            {
                comm.Parameters.AddWithValue("@idEntrevistador", idEntrevistador);
                comm.Parameters.AddWithValue("@razon", Razon);
            }


            return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Estatus de Seleccion";
        }

        public string CedulaRepetida(string Cedula)
        {
            List<DSeleccion> ListaGenerica = new List<DSeleccion>();
            string respuesta = "";
            string estado = "";
            int idEmpleado = 0;

            string nombreCompleto = "";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryIDCardRepeated, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@cedula", Cedula);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    estado = EstadoString(reader.GetInt32(0));
                    idEmpleado = reader.GetInt32(1);
                    nombreCompleto = reader.GetString(2);
                }

                Conexion.ConexionSql.Close();

                if (estado == "Seleccionado" || estado == "Contratado")
                    MessageBox.Show("Este Documento ya existe y dicho Empleado está " + estado, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                else if(estado != "")
                {
                    var resp = MessageBox.Show("El Empleado está Registrado como " + estado + Environment.NewLine + "¿Desea Agregar al Empleado a Selección?" + Environment.NewLine +
                                               "Nombre Empleado: " + nombreCompleto, "SwissNet", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (resp == MessageBoxResult.Yes)
                    {
                        ListaGenerica = EncontrarSeleccion(idEmpleado);
                        status = 1;

                        respuesta = InsertarSeleccion(idEmpleado, ListaGenerica[0]);
                        if (!respuesta.Equals("OK")) return respuesta;

                        return ActivarEmpleado(idEmpleado);
                    }
                }
                return respuesta;
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string EstadoString(int Estado)
        {
            if (Estado == 1)
                return "Seleccionado";
            else if (Estado == 3)
                return "Contratado";
            else if (Estado == 0)
                return "Anulado";
            else if (Estado == 4)
                return "No Contratado";
            else if (Estado == 5)
                return "Despedido";

            throw new Exception("Error en el Tipo de Estado");
        }

        private string ActivarEmpleado(int IdEmpleado)
        {

            if (Conexion.ConexionSql.State == ConnectionState.Closed)
                Conexion.ConexionSql.Open();

            using SqlCommand comm = new SqlCommand(queryUpdateEmployeeSelection, Conexion.ConexionSql);
            comm.Parameters.AddWithValue("@idEmpleado", IdEmpleado);

            return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Estado del Trabajador";
        }



        public List<DEmpleado> EntrevistadosPorUsuario(int IdEntrevistador, DateTime FechaInicio, DateTime FechaFinal)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListInterviewPerUser, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdEntrevistador);
                comm.Parameters.AddWithValue("@fechaInicio", FechaInicio.ToString("s"));
                comm.Parameters.AddWithValue("@fechaFinal", FechaFinal.ToString("s"));

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        cedulaEntrevistado = reader.GetString(0),
                        nombreEntrevistado = reader.GetString(1),
                        statusString = EstadoString(reader.GetInt32(2)),
                        fechaRevision = reader.GetDateTime(3),
                        nombre = reader.GetString(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DEmpleado> SeleccionadosPorUsuario(int IdSeleccionador, DateTime FechaInicio, DateTime FechaFinal)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListSelectionPerUser, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdSeleccionador);
                comm.Parameters.AddWithValue("@fechaInicio", FechaInicio.ToString("s"));
                comm.Parameters.AddWithValue("@fechaFinal", FechaFinal.ToString("s"));

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        cedulaEntrevistado = reader.GetString(0),
                        nombreEntrevistado = reader.GetString(1),
                        statusString = EstadoString(reader.GetInt32(2)),
                        fechaAplicacion = reader.GetDateTime(3),
                        nombre = reader.GetString(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }



        public List<DEmpleado> ListadoEmpleadoContrato(string Nombre)
        {
            List<DEmpleado> ListaGenerica = new List<DEmpleado>();

            string queryListEmployeeContract = @"
                SELECT 
				    e.idEmpleado,
				    e.cedula,
				    e.nombre + ' ' + e.apellido,
				    s.fechaRevision,
				    s.nombrePuesto
			    FROM [Empleado] e
				    INNER JOIN [Seleccion] s ON s.idEmpleado = e.idEmpleado
                WHERE (e.status = 1 OR e.status = 4) AND e.idEmpleado <> 1 AND CONCAT(e.nombre, ' ', e.apellido) LIKE @nombre + '%'
            ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListEmployeeContract, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@nombre", Nombre);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DEmpleado
                    {
                        idEmpleado = reader.GetInt32(0),
                        cedula = reader.GetString(1),
                        nombre = reader.GetString(2),
                        fechaRevisionString = reader.GetDateTime(3).ToString("dd-MM-yyyy"),
                        nombrePuesto = reader.GetString(4)
                       
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }




        public List<DEmpleado> MostrarEmpleado()
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
