using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DTipoMetrica:Conexion
    {
        private int _IdTipoMetrica;
        public int idTipoMetrica
        {
            get { return _IdTipoMetrica; }
            set { _IdTipoMetrica = value; }
        }

        private string _Nombre;
        public string nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private int _IdDepartamento;
        public int idDepartamento
        {
            get { return _IdDepartamento; }
            set { _IdDepartamento = value; }
        }


        public DTipoMetrica()
        {

        }

        public DTipoMetrica(int IdTipoMetrica, string Nombre, int IdDepartamento)
        {
            this.idTipoMetrica = IdTipoMetrica;
            this.nombre = Nombre;
            this.idDepartamento = IdDepartamento;
        }
    }
}
