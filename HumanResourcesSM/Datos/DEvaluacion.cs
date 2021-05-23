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

        private DateTime _FechaEvaluacion;
        public DateTime fechaEvaluacion
        {
            get { return _FechaEvaluacion; }
            set { _FechaEvaluacion = value; }
        }

        private double _ValorMeta;
        public double valorMeta
        {
            get { return _ValorMeta; }
            set { _ValorMeta = value; }
        }

        private string _Cedula;
        public string cedula
        {
            get { return _Cedula; }
            set { _Cedula = value; }
        }
        private string _Usuario;
        public string usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }


        private string _Departamento;
        public string departamento
        {
            get { return _Departamento; }
            set { _Departamento = value; }
        }

        private string _Empleado;
        public string empleado
        {
            get { return _Empleado; }
            set { _Empleado = value; }
        }

        private string _NombreMetrica;
        public string nombreMetrica
        {
            get { return _NombreMetrica; }
            set { _NombreMetrica = value; }
        }

        private string _EmpleadoCedula;
        public string empleadoCedula
        {
            get { return _EmpleadoCedula; }
            set { _EmpleadoCedula = value; }
        }

        private DateTime _PeriodoInicial;
        public DateTime periodoInicial
        {
            get { return _PeriodoInicial; }
            set { _PeriodoInicial = value; }
        }

        private DateTime _PeriodoFinal;
        public DateTime periodoFinal
        {
            get { return _PeriodoFinal; }
            set { _PeriodoFinal = value; }
        }

        private string _TipoMetrica;
        public string tipoMetrica
        {
            get { return _TipoMetrica; }
            set { _TipoMetrica = value; }
        }

        public DEvaluacion()
        {

        }


        public DEvaluacion(int IdEvaluacion, int IdUsuario, int IdMeta, float ValorEvaluado, string Observacion, int Status, DateTime FechaEvaluacion)
        {
            this.idEvaluacion = IdEvaluacion;
            this.idUsuario = IdUsuario;
            this.idMeta = IdMeta;
            this.valorEvaluado = ValorEvaluado;
            this.observacion = Observacion;
            this.status = Status;
            this.fechaEvaluacion = FechaEvaluacion;
        }
    }
}
