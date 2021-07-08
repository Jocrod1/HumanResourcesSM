using System;
using System.Collections.Generic;
using System.Text;
using Datos;

using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.IO;
using System.Security.Cryptography;

namespace Metodos
{

    public static class Globals
    {
        public static String USUARIO_SISTEMA = "";
    }

    public class Encripter
    {
        // set permutations
        public const String strPermutation = "ouiveyxaqtd";
        public const Int32 bytePermutation1 = 0x19;
        public const Int32 bytePermutation2 = 0x59;
        public const Int32 bytePermutation3 = 0x17;
        public const Int32 bytePermutation4 = 0x41;

        // encoding
        public static string Encrypt(string strData)
        {

            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
            // reference https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx

        }


        // decoding
        public static string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
            // reference https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx

        }

        // encrypt
        public static byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Encripter.strPermutation,
            new byte[] { Encripter.bytePermutation1,
                         Encripter.bytePermutation2,
                         Encripter.bytePermutation3,
                         Encripter.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        // decrypt
        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Encripter.strPermutation,
            new byte[] { Encripter.bytePermutation1,
                         Encripter.bytePermutation2,
                         Encripter.bytePermutation3,
                         Encripter.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
    }


    public class MUsuario : DUsuario
    { 
        #region QUERIES 
        private string queryInsert = @"
            INSERT INTO [Usuario] (
                idRol,
                usuario,
                contraseña,
                entrevistando,
                estado
            ) OUTPUT Inserted.idUsuario 
            VALUES (
                @idRol,
                @usuario,
                @contraseña,
                0,
                1
            );
	    ";

        private string queryUpdate = @"
            UPDATE [Usuario] SET 
                idRol = @idRol,
                usuario = @usuario,
                contraseña = @contraseña,
                estado = 1
            WHERE idUsuario = @idUsuario;
        ";

        private string queryDelete = @"
            UPDATE [Usuario] SET 
                estado = 0
            WHERE idUsuario = @idUsuario;
        ";

        private string queryActivate = @"
            UPDATE [Usuario] SET 
                estado = 1
            WHERE idUsuario = @idUsuario;
        ";

        private string queryList = @"
            SELECT * FROM [Usuario] 
            WHERE usuario LIKE @usuario + '%' AND idUsuario <> 1 AND estado <> 0
            ORDER BY usuario;
        ";

        private string queryListSecurity = @"
            SELECT * FROM [Seguridad] 
            WHERE idUsuario = @idUsuario;
        ";

        private string queryLogin = @"
            SELECT * FROM [Usuario] 
            WHERE usuario = @usuario AND contraseña = @contraseña AND idUsuario <> 1 AND estado <> 0
        ";

        private string queryFormSecurity = @"
            SELECT 
	            u.usuario,
	            s.pregunta,
	            s.respuesta
            FROM [seguridad] s
	            INNER JOIN [Usuario] u ON u.idUsuario = s.idUsuario
            WHERE u.usuario = @usuario AND estado <> 0;
        ";

        private string queryListID = @"
            SELECT * FROM [Usuario] 
            WHERE idUsuario = @idUsuario AND estado <> 0
        ";

        private string queryInterview = @"
            UPDATE [Usuario] SET
                entrevistando = @entrevistando
            WHERE idUsuario = @idUsuario AND estado <> 0;
        ";
        string queryUpdatePassword = @"
             UPDATE [Usuario] SET
                contraseña = @contraseña
            WHERE usuario = @usuario AND estado <> 0;
	    ";

        private string queryInsertSecurity = @"
            INSERT INTO [Seguridad] (
                idUsuario,
                pregunta,
                respuesta
            ) VALUES (
                @idUsuario,
                @pregunta,
                @respuesta
            );
        ";

        private string queryDeleteSecurity = @"
            DELETE FROM [Seguridad]
            WHERE idUsuario = @idUsuario
	    ";




        private string queryUserNullRepeated = @"
            SELECT idUsuario FROM [Usuario] 
            WHERE usuario = @usuario AND estado = 0;
        ";
        #endregion

        public string Insertar(DUsuario Usuario, List<DSeguridad> Seguridad)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInsert, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                comm.Parameters.AddWithValue("@contraseña", Encripter.Encrypt(Usuario.contraseña));
                Usuario.idUsuario = (int)comm.ExecuteScalar();


                string respuesta = "";

                if (Usuario.idUsuario > 0)
                    respuesta = InsertarSeguridad(Seguridad, Usuario.idUsuario);
                else
                {
                    respuesta = "ERROR";
                }

                return respuesta;
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public string Editar(DUsuario Usuario, List<DSeguridad> Seguridad)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idRol", Usuario.idRol);
                comm.Parameters.AddWithValue("@usuario", Usuario.usuario);
                comm.Parameters.AddWithValue("@contraseña", Encripter.Encrypt(Usuario.contraseña));
                comm.Parameters.AddWithValue("@idUsuario", Usuario.idUsuario);

                var rs = comm.ExecuteNonQuery();

                string respuesta = rs == 1 ? "OK" : "No se Actualizó el Registro del Usuario";
                if (respuesta.Equals("OK"))
                    respuesta = BorrarSeguridad(Usuario, Seguridad);

                return respuesta;
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        public string EditarContraseña(string Usuario, string Contraseña)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUpdatePassword, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);
                comm.Parameters.AddWithValue("@contraseña", Encripter.Encrypt(Contraseña));

                string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Actualizó el Registro del Usuario";

                return respuesta;
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        private string InsertarSeguridad(List<DSeguridad> Detalle, int IdUsuario)
        {
            int i = 0;
            string respuesta = "";

            foreach (DSeguridad det in Detalle)
            {
                using SqlCommand comm = new SqlCommand(queryInsertSecurity, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);
                comm.Parameters.AddWithValue("@pregunta", Encripter.Encrypt(Detalle[i].pregunta));
                comm.Parameters.AddWithValue("@respuesta", Encripter.Encrypt(Detalle[i].respuesta));

                respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "No se Ingresó el Registro de la Seguridad";
                if (!respuesta.Equals("OK")) break;

                i++;
            }

            return respuesta;
        }

        private string BorrarSeguridad(DUsuario Usuario, List<DSeguridad> Seguridad)
        {
            using SqlCommand comm = new SqlCommand(queryDeleteSecurity, Conexion.ConexionSql);
            comm.Parameters.AddWithValue("@idUsuario", Usuario.idUsuario);

            string respuesta = comm.ExecuteNonQuery() == 1 ? "OK" : "OK";

            if (respuesta.Equals("OK"))
                InsertarSeguridad(Seguridad, Usuario.idUsuario);

            return respuesta;
        }


        public List<DUsuario> Mostrar(string Usuario)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryList, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = Encripter.Decrypt(reader.GetString(3)),
                        entrevistando = reader.GetInt32(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DSeguridad> EncontrarSeguridad(int idUsuario)
        {
            List<DSeguridad> ListaGenerica = new List<DSeguridad>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListSecurity, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", idUsuario);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DSeguridad
                        (
                            reader.GetInt32(1),
                            Encripter.Decrypt(reader.GetString(2)),
                            Encripter.Decrypt(reader.GetString(3))
                        ));
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DUsuario> Login(string Usuario, string Contraseña)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();


            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryLogin, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);
                comm.Parameters.AddWithValue("@contraseña", Encripter.Encrypt(Contraseña));

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = Encripter.Decrypt(reader.GetString(3)),
                        entrevistando = reader.GetInt32(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }

        public List<DSeguridad> Seguridad(string user)
        {
            List<DSeguridad> ListaGenerica = new List<DSeguridad>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryFormSecurity, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", user);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DSeguridad
                    {
                        usuario = reader.GetString(0),
                        pregunta = Encripter.Decrypt(reader.GetString(1)),
                        respuesta = Encripter.Decrypt(reader.GetString(2))
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DUsuario> Encontrar(int IdUsuario)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListID, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = Encripter.Decrypt(reader.GetString(3)),
                        entrevistando = reader.GetInt32(4)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public string Entrevistando(int IdUsuario, bool Entrevistando)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryInterview, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@entrevistando", Entrevistando ? 1 : 0);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se actualizo el Registro del usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }


        public static string Backup(string Path)
        {
            try
            {
                Conexion.ConexionSql.Open();
                string database = Conexion.ConexionSql.Database.ToString();

                if (Path == string.Empty)
                    MessageBox.Show("Se tiene que seleccionar una ruta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    string cmd = "BACKUP DATABASE [" + database + "] TO DISK='" + Path + "\\" + "dbSwissNet" + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".bak'";

                    using (SqlCommand command = new SqlCommand(cmd, Conexion.ConexionSql))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Backup almacenado de manera satisfactoria!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                return "ERROR";
            }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return "OK";
        }

        public static string Restore(string Path)
        {
            try
            {
                Conexion.ConexionSql.Open();
                string database = Conexion.ConexionSql.Database.ToString();

                if (Path == string.Empty)
                    MessageBox.Show("ERROR! Necesita seleccionar un respaldo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    string sqlStmt2 = string.Format("ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    SqlCommand bu2 = new SqlCommand(sqlStmt2, Conexion.ConexionSql);
                    bu2.ExecuteNonQuery();

                    string sqlStmt3 = "USE MASTER RESTORE DATABASE [" + database + "] FROM DISK='" + Path + "'WITH REPLACE;";
                    SqlCommand bu3 = new SqlCommand(sqlStmt3, Conexion.ConexionSql);
                    bu3.ExecuteNonQuery();

                    string sqlStmt4 = string.Format("ALTER DATABASE [" + database + "] SET MULTI_USER");
                    SqlCommand bu4 = new SqlCommand(sqlStmt4, Conexion.ConexionSql);
                    bu4.ExecuteNonQuery();

                    MessageBox.Show("Base de Datos Restaurada Correctamente", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                return "ERROR";
            }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return "OK";
        }

        public string Desactivar(int IdUsuario)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryDelete, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Desactivó el Usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }

        public string Activar(int IdUsuario)
        {
            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryActivate, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                return comm.ExecuteNonQuery() == 1 ? "OK" : "No se Reactivó el Usuario";
            }
            catch (SqlException e) { return e.Message; }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }
        }



        public List<DUsuario> EncontrarByUsuario(string Usuario, int IdUsuario)
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            string queryUserRepeated = @"
                SELECT * FROM [Usuario] 
                WHERE usuario = @usuario
                    AND (idUsuario <> @idUsuario OR (idUsuario = @idUsuario AND estado = 0));
            ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryUserRepeated, Conexion.ConexionSql);
                comm.Parameters.AddWithValue("@usuario", Usuario);
                comm.Parameters.AddWithValue("@idUsuario", IdUsuario);

                using SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        contraseña = Encripter.Decrypt(reader.GetString(3)),
                        entrevistando = reader.GetInt32(4),
                        estado = reader.GetInt32(5)
                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }


        public List<DUsuario> ListadoUsuarioEntrevistador()
        {
            List<DUsuario> ListaGenerica = new List<DUsuario>();

            string queryListUserInterview = @"
                SELECT 
                    *
			    FROM [Usuario]
                WHERE estado = 1 AND entrevistando = 1 and idUsuario <> 1
            ";

            try
            {
                Conexion.ConexionSql.Open();

                using SqlCommand comm = new SqlCommand(queryListUserInterview, Conexion.ConexionSql);

                using SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListaGenerica.Add(new DUsuario
                    {
                        idUsuario = reader.GetInt32(0),
                        idRol = reader.GetInt32(1),
                        usuario = reader.GetString(2),
                        entrevistando = reader.GetInt32(4)

                    });
                }
            }
            catch (SqlException e) { MessageBox.Show(e.Message, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error); }
            finally { if (Conexion.ConexionSql.State == ConnectionState.Open) Conexion.ConexionSql.Close(); }

            return ListaGenerica;
        }
    }
}
