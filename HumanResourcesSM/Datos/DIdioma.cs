using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DIdioma:Conexion
    {
        private int _IdIdioma;
        public int idIdioma
        {
            get { return _IdIdioma; }
            set { _IdIdioma = value; }
        }

        private string _Nombre;
        public string nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        public DIdioma()
        {

        }

        public DIdioma(int IdIdioma, string Nombre)
        {
            this.idIdioma = IdIdioma;
            this.nombre = Nombre;
        }
    }
}
