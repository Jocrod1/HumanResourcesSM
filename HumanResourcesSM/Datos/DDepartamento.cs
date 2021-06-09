using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DDepartamento:Conexion
    {

        private int _IdDepartamento;
        public int idDepartamento
        {
            get { return _IdDepartamento; }
            set { _IdDepartamento = value; }
        }

        private string _Codigo;
        public string codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
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


        public DDepartamento()
        {

        }

        public DDepartamento(string Codigo, string Nombre, string Descripcion)
        {
            this.codigo = Codigo;
            this.nombre = Nombre;
            this.descripcion = Descripcion;
        }
    }
}
