using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DContrato:Conexion
    {

        private int _IdContrato;
        public int idContrato
        {
            get { return _IdContrato; }
            set { _IdContrato = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private DateTime _FechaContratacion;
        public DateTime fechaContratacion
        {
            get { return _FechaContratacion; }
            set { _FechaContratacion = value; }
        }

        private string _NombrePuesto;
        public string nombrePuesto
        {
            get { return _NombrePuesto; }
            set { _NombrePuesto = value; }
        }

        private double _Sueldo;
        public double sueldo
        {
            get { return _Sueldo; }
            set { _Sueldo = value; }
        }

        private int _HorasSemanales;
        public int horasSemanales
        {
            get { return _HorasSemanales; }
            set { _HorasSemanales = value; }
        }

        public DContrato()
        {

        }

        public DContrato(int IdContrato, int IdEmpleado, DateTime FechaContratacion, string NombrePuesto, double Sueldo, int HorasSemanales)
        {
            this.idContrato = IdContrato;
            this.idEmpleado = IdEmpleado;
            this.fechaContratacion = FechaContratacion;
            this.nombrePuesto = NombrePuesto;
            this.sueldo = Sueldo;
            this.horasSemanales = HorasSemanales;
        }
    }
}
