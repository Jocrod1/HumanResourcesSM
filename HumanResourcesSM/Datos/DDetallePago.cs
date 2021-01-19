using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DDetallePago:Conexion
    {

        private int _IdDetallePago;
        public int idDetallePago
        {
            get { return _IdDetallePago; }
            set { _IdDetallePago = value; }
        }

        private int _IdPago;
        public int idPago
        {
            get { return _IdPago; }
            set { _IdPago = value; }
        }

        private string _Concepto;
        public string concepto
        {
            get { return _Concepto; }
            set { _Concepto = value; }
        }

        private double _SubTotal;
        public double subTotal
        {
            get { return _SubTotal; }
            set { _SubTotal = value; }
        }

        public DDetallePago()
        {

        }

        public DDetallePago(int IdDetallePago, int IdPago, string Concepto, double SubTotal)
        {
            this.idDetallePago = IdDetallePago;
            this.idPago = IdPago;
            this.concepto = Concepto;
            this.subTotal = SubTotal;
        }
    }
}
