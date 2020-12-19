using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DEducacion:Conexion
    {
        private int _IdEducacion;
        public int idEducacion
        {
            get { return _IdEducacion; }
            set { _IdEducacion = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private string _NombreCarrera;
        public string nombreCarrera
        {
            get { return _NombreCarrera; }
            set { _NombreCarrera = value; }
        }

        private string _NombreInstitucion;
        public string nombreInstitucion
        {
            get { return _NombreInstitucion; }
            set { _NombreInstitucion = value; }
        }

        private DateTime _FechaIngreso;
        public DateTime fechaIngreso
        {
            get { return _FechaIngreso; }
            set { _FechaIngreso = value; }
        }

        private DateTime _FechaEgreso;
        public DateTime fechaEgreso
        {
            get { return _FechaEgreso; }
            set { _FechaEgreso = value; }
        }


        public DEducacion()
        {

        }

        public DEducacion(int IdEducacion, int IdEmpleado, string NombreCarrera, string NombreInstitucion, DateTime FechaIngreso, DateTime FechaEgreso)
        {
            this.idEducacion = IdEducacion;
            this.idEmpleado = IdEmpleado;
            this.nombreCarrera = NombreCarrera;
            this.nombreInstitucion = NombreInstitucion;
            this.fechaIngreso = FechaIngreso;
            this.fechaEgreso = FechaEgreso;
        }
    }
}
