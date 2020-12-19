using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DUsuario:Conexion
    {

        private int _IdUsuario;
        public int idUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        private int _IdRol;
        public int idRol
        {
            get { return _IdRol; }
            set { _IdRol = value; }
        }

        private string _Usuario;
        public string usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }

        private string _Contraseña;
        public string contraseña
        {
            get { return _Contraseña; }
            set { _Contraseña = value; }
        }

        private string _Confirmacion;
        public string confirmacion
        {
            get { return _Confirmacion; }
            set { _Confirmacion = value; }
        }


        public DUsuario()
        {

        }

        public DUsuario(int IdUsuario, int IdRol, string Usuario, string Contraseña, string Confirmacion)
        {
            this.idUsuario = IdUsuario;
            this.idRol = IdRol;
            this.usuario = Usuario;
            this.contraseña = Contraseña;
            this.confirmacion = Confirmacion;
        }
    }
}
