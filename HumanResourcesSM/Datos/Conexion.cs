using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class Conexion
    {
        protected static string conexionRaimon = "DESKTOP-2185U8G\\SQLEXPRESS";
        protected static string conexionJose = "DESKTOP-KOFID31\\SQLEXPRESS01";
        protected static string CadenaConexion = "Data Source= " + conexionRaimon + "; Initial Catalog= dbSwissNet; Integrated Security= true";

        protected static SqlConnection ConexionSql = new SqlConnection(CadenaConexion);
    }
}

