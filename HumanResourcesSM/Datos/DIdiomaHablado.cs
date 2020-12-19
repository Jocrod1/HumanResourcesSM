using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DIdiomaHablado : Conexion
    {

        private int _IdIdiomaHablado;
        public int idIdiomaHablado
        {
            get { return _IdIdiomaHablado; }
            set { _IdIdiomaHablado = value; }
        }

        private int _IdIdioma;
        public int idIdioma
        {
            get { return _IdIdioma; }
            set { _IdIdioma = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private int _Nivel;
        public int nivel
        {
            get { return _Nivel; }
            set { _Nivel = value; }
        }


        public DIdiomaHablado()
        {

        }

        public DIdiomaHablado(int IdIdiomaHablado, int IdIdioma, int IdEmpleado, int Nivel)
        {
            this.idIdiomaHablado = IdIdiomaHablado;
            this.idIdioma = IdIdioma;
            this.idEmpleado = IdEmpleado;
            this.nivel = Nivel;
        }
    }
}
