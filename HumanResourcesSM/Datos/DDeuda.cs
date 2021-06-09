using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DDeuda : Conexion
    {

        private int _IdDeuda;
        public int idDeuda
        {
            get { return _IdDeuda; }
            set { _IdDeuda = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private double _Monto;
        public double monto
        {
            get { return _Monto; }
            set { _Monto = value; }
        }

        private string _Concepto;
        public string concepto
        {
            get { return _Concepto; }
            set { _Concepto = value; }
        }

        private int _TipoDeuda;
        public int tipoDeuda
        {
            get { return _TipoDeuda; }
            set { _TipoDeuda = value; }
        }

        private string _TipoDeudaString;
        public string tipoDeudaString
        {
            get { return _TipoDeudaString; }
            set { _TipoDeudaString = value; }
        }

        public string tipoDeudaStringz
        {
            get
            {
                if (tipoDeuda == 0)
                {
                    return "Bonificación";
                }
                else if (tipoDeuda == 1)
                {
                    return "Deducción";
                }
                else return "ERROR";
            }
        }


        private int _Status;
        public int status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string statusString
        {
            get {
                if (status == 1)
                    return "No Pagado";
                else if (status == 2)
                    return "Pagado";
                else if (status == 0)
                    return "Anulado";
                else return "ERROR";
            }
        }


        private int _Repetitivo;
        public int repetitivo
        {
            get { return _Repetitivo; }
            set { _Repetitivo = value; }
        }

        private int _TipoPago;
        public int tipoPago
        {
            get { return _TipoPago; }
            set { _TipoPago = value; }
        }

        private string _TipoPagoString;
        public string tipoPagoString
        {
            get { return _TipoPagoString; }
            set { _TipoPagoString = value; }
        }

        public string tipoPagoStringz
        {
            get
            {
                if (tipoPago == 0)
                {
                    return "Porcentual";
                }
                else if (tipoPago == 1)
                {
                    return "Monto";
                }
                else return "ERROR";
            }
        }

        private string _Cedula;
        public string cedula
        {
            get { return _Cedula; }
            set { _Cedula = value; }
        }

        private string _NombreCompleto;
        public string nombreCompleto
        {
            get { return _NombreCompleto; }
            set { _NombreCompleto = value; }
        }

        private string _Departamento;
        public string departamento
        {
            get { return _Departamento; }
            set { _Departamento = value; }
        }



        public DDeuda()
        {

        }

        public DDeuda(int IdDeuda, int IdEmpleado, double Monto, string Concepto, int TipoDeuda, int Repetitivo, int TipoPago, int Status)
        {
            this.idDeuda = IdDeuda;
            this.idEmpleado = IdEmpleado;
            this.monto = Monto;
            this.concepto = Concepto;
            this.tipoDeuda = TipoDeuda;
            this.status = Status;
            this.repetitivo = Repetitivo;
            this.tipoPago = TipoPago;
        }
    }
}
