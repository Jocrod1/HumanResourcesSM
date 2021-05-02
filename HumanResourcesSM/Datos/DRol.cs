using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DRol:Conexion
    {
        private int _IdRol;
        public int idRol
        {
            get { return _IdRol; }
            set { _IdRol = value; }
        }
        private string _Nombre;
        public string nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        private string _Descripcion;
        public string descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        public DRol()
        {

        }

        public DRol(int idRol, string nombre, string descripcion)
        {
            this.idRol = idRol;
            this.nombre = nombre;
            this.descripcion = descripcion;
        }
    }
}
