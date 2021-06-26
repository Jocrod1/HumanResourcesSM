using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DPrestacion:Conexion
    {

        private int _IdPrestacion;
        public int idPrestacion
        {
            get { return _IdPrestacion; }
            set { _IdPrestacion = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private double _MontoPresupuesto;
        public double montoPresupuesto
        {
            get { return _MontoPresupuesto; }
            set { _MontoPresupuesto = value; }
        }

        private double _PorcentajeOtorgado;
        public double porcentajeOtorgado
        {
            get { return _PorcentajeOtorgado; }
            set { _PorcentajeOtorgado = value; }
        }

        private double _MontoOtorgado;
        public double montoOtorgado
        {
            get { return _MontoOtorgado; }
            set { _MontoOtorgado = value; }
        }

        private string _Razon;
        public string razon
        {
            get { return _Razon; }
            set { _Razon = value; }
        }

        private string _RazonDecision;
        public string razonDecision
        {
            get { return _RazonDecision; }
            set { _RazonDecision = value; }
        }

        private DateTime _FechaSolicitud;
        public DateTime fechaSolicitud
        {
            get { return _FechaSolicitud; }
            set { _FechaSolicitud = value; }
        }

        private string _FechaSolicitudString;
        public string fechaSolicitudString
        {
            get { return _FechaSolicitudString; }
            set { _FechaSolicitudString = value; }
        }

        private DateTime _FechaContratacion;
        public DateTime fechaContratacion
        {
            get { return _FechaContratacion; }
            set { _FechaContratacion = value; }
        }

        private string _FechaContratacionString;
        public string fechaContratacionString
        {
            get { return _FechaContratacionString; }
            set { _FechaContratacionString = value; }
        }

        private int _Estado;
        public int estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        private string _NombreEmpleado;
        public string nombreEmpleado
        {
            get { return _NombreEmpleado; }
            set { _NombreEmpleado = value; }
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

        public DPrestacion()
        {

        }

        public DPrestacion(int IdPrestacion, int IdEmpleado, double MontoPresupuesto, double PorcentajeOtorgado, double MontoOtorgado, string Razon, string RazonDecision, DateTime FechaSolicitud, int Estado)
        {
            this.idPrestacion = IdPrestacion;
            this.idEmpleado = IdEmpleado;
            this.montoPresupuesto = MontoPresupuesto;
            this.porcentajeOtorgado = PorcentajeOtorgado;
            this.montoOtorgado = MontoOtorgado;
            this.razon = Razon;
            this.razonDecision = RazonDecision;
            this.fechaSolicitud = FechaSolicitud;
            this.estado = Estado;
        }
    }
}
