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

        private string _Cantidad;
        public string cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }

        private string _Salario;
        public string salario
        {
            get { return _Salario; }
            set { _Salario = value; }
        }

        private string _Asignacion;
        public string asignacion
        {
            get { return _Asignacion; }
            set { _Asignacion = value; }
        }

        private string _Deduccion;
        public string deduccion
        {
            get { return _Deduccion; }
            set { _Deduccion = value; }
        }

        public DDetallePago()
        {

        }

        public DDetallePago(int IdDetallePago, int IdPago, string Concepto, string Cantidad, string Salario, string Asignacion, string Deduccion)
        {
            this.idDetallePago = IdDetallePago;
            this.idPago = IdPago;
            this.concepto = Concepto;
            this.cantidad = Cantidad;
            this.salario = Salario;
            this.asignacion = Asignacion;
            this.deduccion = Deduccion;
        }
    }
}
