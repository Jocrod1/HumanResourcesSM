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

        private int _IdTipoMetrica;
        public int idTipoMetrica
        {
            get { return _IdTipoMetrica; }
            set { _IdTipoMetrica = value; }
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

        private int _IdDepartamento;
        public int idDepartamento
        {
            get { return _IdDepartamento; }
            set { _IdDepartamento = value; }
        }

        private int _Status;
        public int status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private DateTime _FechaInicio;
        public DateTime fechaInicio
        {
            get { return _FechaInicio; }
            set { _FechaInicio = value; }
        }

        private DateTime _FechaFinal;
        public DateTime fechaFinal
        {
            get { return _FechaFinal; }
            set { _FechaFinal = value; }
        }

        private string _Cedula;
        public string cedula
        {
            get { return _Cedula; }
            set { _Cedula = value; }
        }

        private int _IdUsuario;
        public int idUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }


        //valores no registro

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

        private string _NombreMetrica;
        public string nombreMetrica
        {
            get { return _NombreMetrica; }
            set { _NombreMetrica = value; }
        }

        public DMeta()
        {

        }

        public DMeta(int IdMeta, int IdTipoMetrica, double ValorMeta, int IdEmpleado, int IdDepartamento, int Status, DateTime FechaInicio, DateTime FechaFinal, string Cedula, int IdUsuario, string Usuario, string Departamento, string NombreMetrica)
        {
            this.idMeta = IdMeta;
            this.idTipoMetrica = IdTipoMetrica;
            this.valorMeta = ValorMeta;
            this.idEmpleado = IdEmpleado;
            this.idDepartamento = IdDepartamento;
            this.status = Status;
            this.fechaInicio = FechaInicio;
            this.fechaFinal = FechaFinal;
            this.cedula = Cedula;
            this.idUsuario = IdUsuario;
            this.usuario = Usuario;
            this.departamento = Departamento;
            this.nombreMetrica = NombreMetrica;
        }
    }
}
