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

        private int _IdDeuda;
        public int idDeuda
        {
            get { return _IdDeuda; }
            set { _IdDeuda = value; }
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

        private string _NombreDeuda;
        public string nombreDeuda
        {
            get { return _NombreDeuda; }
            set { _NombreDeuda = value; }
        }

        public DDetallePago()
        {

        }

        public DDetallePago(int IdDetallePago, int IdPago, int IdDeuda, string Concepto, double SubTotal)
        {
            this.idDetallePago = IdDetallePago;
            this.idPago = IdPago;
            this.idDeuda = IdDeuda;
            this.concepto = Concepto;
            this.subTotal = SubTotal;
        }
    }
}
