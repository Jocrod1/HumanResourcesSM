using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DEvaluacion:Conexion
    {

        private int _IdEvaluacion;
        public int idEvaluacion
        {
            get { return _IdEvaluacion; }
            set { _IdEvaluacion = value; }
        }

        private int _IdUsuario;
        public int idUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        private int _IdMeta;
        public int idMeta
        {
            get { return _IdMeta; }
            set { _IdMeta = value; }
        }

        private double _ValorEvaluado;
        public double valorEvaluado
        {
            get { return _ValorEvaluado; }
            set { _ValorEvaluado = value; }
        }

        private string _Observacion;
        public string observacion
        {
            get { return _Observacion; }
            set { _Observacion = value; }
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

        public DEvaluacion()
        {

        }


        public DEvaluacion(int IdEvaluacion, int IdUsuario, int IdMeta, double ValorEvaluado, string Observacion, int Status, DateTime Periodo, string Cedula)
        {
            this.idEvaluacion = IdEvaluacion;
            this.idUsuario = IdUsuario;
            this.idMeta = IdMeta;
            this.valorEvaluado = ValorEvaluado;
            this.observacion = Observacion;
            this.status = Status;
            this.periodo = Periodo;
            this.cedula = Cedula;
        }
    }
}
