using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DPago:Conexion
    {

        private int _IdPago;
        public int idPago
        {
            get { return _IdPago; }
            set { _IdPago = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private DateTime _FechaPago;
        public DateTime fechaPago
        {
            get { return _FechaPago; }
            set { _FechaPago = value; }
        }

        private string _Banco;
        public string banco
        {
            get { return _Banco; }
            set { _Banco = value; }
        }

        private string _NumeroReferencia;
        public string numeroReferencia
        {
            get { return _NumeroReferencia; }
            set { _NumeroReferencia = value; }
        }

        private int _CantidadHoras;
        public int cantidadHoras
        {
            get { return _CantidadHoras; }
            set { _CantidadHoras = value; }
        }

        private DateTime _PeriodoInicio;
        public DateTime periodoInicio
        {
            get { return _PeriodoInicio; }
            set { _PeriodoInicio = value; }
        }

        private DateTime _PeriodoFinal;
        public DateTime periodoFinal
        {
            get { return _PeriodoFinal; }
            set { _PeriodoFinal = value; }
        }

        public string PeriodoString
        {
            get
            {
                return _PeriodoInicio.ToShortDateString() + " - " + _PeriodoFinal.ToShortDateString();
            }
        }

        private double _MontoTotal;
        public double montoTotal
        {
            get { return _MontoTotal; }
            set { _MontoTotal = value; }
        }

        private string _Cedula;
        public string cedula
        {
            get { return _Cedula; }
            set { _Cedula = value; }
        }

        private string _Nombre;
        public string nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private double _TotalAsignacion;
        public double totalAsignacion
        {
            get { return _TotalAsignacion; }
            set { _TotalAsignacion = value; }
        }

        private double _TotalDeduccion;
        public double totalDeduccion
        {
            get { return _TotalDeduccion; }
            set { _TotalDeduccion = value; }
        }

        private double _TotalSalario;
        public double totalSalario
        {
            get { return _TotalSalario; }
            set { _TotalSalario = value; }
        }

        private int _Estado;
        public int estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        private string _UsuarioEmisor;
        public string usuarioEmisor
        {
            get { return _UsuarioEmisor; }
            set { _UsuarioEmisor = value; }
        }

        public DPago()
        {

        }

        public DPago(int IdPago, int IdEmpleado, DateTime FechaPago, string Banco, string NumeroReferencia, int CantidadHoras, DateTime PeriodoInicio, DateTime PeriodoFinal, double MontoTotal, double TotalAsignacion, double TotalDeduccion, double TotalSalario, int Estado)
        {
            this.idPago = IdPago;
            this.idEmpleado = IdEmpleado;
            this.fechaPago = FechaPago;
            this.banco = Banco;
            this.numeroReferencia = NumeroReferencia;
            this.cantidadHoras = CantidadHoras;
            this.periodoInicio = PeriodoInicio;
            this.periodoFinal = PeriodoFinal;
            this.montoTotal = MontoTotal;
            this.totalAsignacion = TotalAsignacion;
            this.totalDeduccion = TotalDeduccion;
            this.totalSalario = TotalSalario;
            this.estado = Estado;
        }
    }
}
