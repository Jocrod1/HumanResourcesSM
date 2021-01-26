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

            private int _Repetitivo;
            public int repetitivo
            {
                get { return _Repetitivo; }
                set { _Repetitivo = value; }
            }

            private DateTime _PeriodoPago;
            public DateTime periodoPago
            {
                get { return _PeriodoPago; }
                set { _PeriodoPago = value; }
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

            public DBonificacion(int IdBonificacion, int IdEmpleado, double MontoBonificacion, int Repetitivo, DateTime PeriodoPago, int Status)
            {
                this.idBonificacion = IdBonificacion;
                this.idEmpleado = IdEmpleado;
                this.montoBonificacion = MontoBonificacion;
                this.repetitivo = Repetitivo;
                this.periodoPago = PeriodoPago;
                this.status = Status;
            }
        }
}
