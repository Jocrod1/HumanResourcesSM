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

        private DateTime _PeriodoPago;
        public DateTime periodoPago
        {
            get { return _PeriodoPago; }
            set { _PeriodoPago = value; }
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

        private int _Estado;
        public int estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        public DPago()
        {

        }

        public DPago(int IdPago, int IdEmpleado, DateTime FechaPago, string Banco, string NumeroReferencia, int CantidadHoras, DateTime PeriodoPago, double MontoTotal, int Estado)
        {
            this.idPago = IdPago;
            this.idEmpleado = IdEmpleado;
            this.fechaPago = FechaPago;
            this.banco = Banco;
            this.numeroReferencia = NumeroReferencia;
            this.cantidadHoras = CantidadHoras;
            this.periodoPago = PeriodoPago;
            this.montoTotal = MontoTotal;
            this.estado = Estado;
        }
    }
}
