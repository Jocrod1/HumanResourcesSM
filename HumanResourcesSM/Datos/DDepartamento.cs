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

        public DDepartamento(int IdDepartamento, string Nombre, string Descripcion)
        {
            this.idDepartamento = IdDepartamento;
            this.nombre = Nombre;
            this.descripcion = Descripcion;
        }
    }
}
