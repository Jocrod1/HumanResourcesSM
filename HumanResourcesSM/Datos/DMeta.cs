using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DMeta:Conexion
    {


        private int _IdMeta;
        public int idMeta
        {
            get { return _IdMeta; }
            set { _IdMeta = value; }
        }

        private string _TipoMetrica;
        public string tipoMetrica
        {
            get { return _TipoMetrica; }
            set { _TipoMetrica = value; }
        }

        private double _ValorMeta;
        public double valorMeta
        {
            get { return _ValorMeta; }
            set { _ValorMeta = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private int _Status;
        public int status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private DateTime _Periodo;
        public DateTime periodo
        {
            get { return _Periodo; }
            set { _Periodo = value; }
        }

        private string _Cedula;
        public string cedula
        {
            get { return _Cedula; }
            set { _Cedula = value; }
        }

        public DMeta()
        {

        }

        public DMeta(int IdMeta, string TipoMetrica, double ValorMeta, int IdEmpleado, int Status, DateTime Periodo, string Cedula)
        {
            this.idMeta = IdMeta;
            this.tipoMetrica = TipoMetrica;
            this.valorMeta = ValorMeta;
            this.idEmpleado = IdEmpleado;
            this.status = Status;
            this.periodo = Periodo;
            this.cedula = Cedula;
        }
    }
}
