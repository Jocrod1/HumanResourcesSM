using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DBonificacion:Conexion
    {

            private int _IdBonificacion;
            public int idBonificacion
            {
                get { return _IdBonificacion; }
                set { _IdBonificacion = value; }
            }

            private int _IdEmpleado;
            public int idEmpleado
            {
                get { return _IdEmpleado; }
                set { _IdEmpleado = value; }
            }

            private double _MontoBonificacion;
            public double montoBonificacion
            {
                get { return _MontoBonificacion; }
                set { _MontoBonificacion = value; }
            }

            private int _Pagado;
            public int pagado
            {
                get { return _Pagado; }
                set { _Pagado = value; }
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

            private int _Status;
            public int status
            {
                get { return _Status; }
                set { _Status = value; }
            }



            public DBonificacion()
            {

            }

            public DBonificacion(int IdBonificacion, int IdEmpleado, double MontoBonificacion, int Pagado, string Concepto, int TipoDeuda, int Status)
            {
                this.idBonificacion = IdBonificacion;
                this.idEmpleado = IdEmpleado;
                this.montoBonificacion = MontoBonificacion;
                this.pagado = Pagado;
                this.concepto = Concepto;
                this.tipoDeuda = TipoDeuda;
                this.status = Status;
            }
        }
}
