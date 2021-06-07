using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DEmpleado:Conexion
    {

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

        private string _Nombre;
        public string nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private string _Apellido;
        public string apellido
        {
            get { return _Apellido; }
            set { _Apellido = value; }
        }

        private string _Cedula;
        public string cedula
        {
            get { return _Cedula; }
            set { _Cedula = value; }
        }

        private DateTime _FechaNacimiento;
        public DateTime fechaNacimiento
        {
            get { return _FechaNacimiento; }
            set { _FechaNacimiento = value; }
        }

        private string _Nacionalidad;
        public string nacionalidad
        {
            get { return _Nacionalidad; }
            set { _Nacionalidad = value; }
        }

        private string _Direccion;
        public string direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }

        private string _Email;
        public string email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Telefono;
        public string telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }

        private string _Curriculum;
        public string curriculum
        {
            get { return _Curriculum; }
            set { _Curriculum = value; }
        }

        private string _EstadoLegal;
        public string estadoLegal
        {
            get { return _EstadoLegal; }
            set { _EstadoLegal = value; }
        }

        private DateTime? _FechaCulminacion;
        public DateTime? fechaCulminacion
        {
            get { return _FechaCulminacion; }
            set { _FechaCulminacion = value; }
        }

        private int _Status;
        public int status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _StatusString;
        public string statusString
        {
            get { return _StatusString; }
            set { _StatusString = value; }
        }

        public string StatusString
        {
            get
            {
                switch (_Status)
                {
                    case 0:
                        return "Anulado";
                    case 1:
                        return "Seleccionado";
                    case 3:
                        return "Contratado";
                    case 4:
                        return "No Contratado";
                    case 5:
                        return "Despido";
                    default:
                        return "ERROR";
                }
            }
        }

        public bool StatusEstaenEmpresa
        {
            get
            {
                return _Status >= 1 && _Status <= 3;
            }
        }

        private DateTime _FechaAplicacion;
        public DateTime fechaAplicacion
        {
            get { return _FechaAplicacion; }
            set { _FechaAplicacion = value; }
        }

        private DateTime _FechaRevision;
        public DateTime fechaRevision
        {
            get { return _FechaRevision; }
            set { _FechaRevision = value; }
        }

        private string _NombrePuesto;
        public string nombrePuesto
        {
            get { return _NombrePuesto; }
            set { _NombrePuesto = value; }
        }

        private string _NombreDepartamento;
        public string nombreDepartamento
        {
            get { return _NombreDepartamento; }
            set { _NombreDepartamento = value; }
        }

        private double _Sueldo;
        public double sueldo
        {
            get { return _Sueldo; }
            set { _Sueldo = value; }
        }

        public string SueldoString
        {
            get
            {
                return sueldo + "€/h";
            }
        }

        private double _UltimoPago;
        public double ultimoPago
        {
            get { return _UltimoPago; }
            set { _UltimoPago = value; }
        }

        private string _Periodo;
        public string periodo
        {
            get { return _Periodo; }
            set { _Periodo = value; }
        }


        private string _UltimoPagoFecha;
        public string ultimoPagoFecha
        {
            get { return _UltimoPagoFecha; }
            set { _UltimoPagoFecha = value; }
        }

        private int _NumeroContrataciones;
        public int numeroContrataciones
        {
            get { return _NumeroContrataciones; }
            set { _NumeroContrataciones = value; }
        }

        private int _NumeroSelecciones;
        public int numeroSelecciones
        {
            get { return _NumeroSelecciones; }
            set { _NumeroSelecciones = value; }
        }

        private string _UltimaContratacion;
        public string ultimaContratacion
        {
            get { return _UltimaContratacion; }
            set { _UltimaContratacion = value; }
        }

        private string _CedulaEntrevistado;
        public string cedulaEntrevistado
        {
            get { return _CedulaEntrevistado; }
            set { _CedulaEntrevistado = value; }
        }

        private string _NombreEntrevistado;
        public string nombreEntrevistado
        {
            get { return _NombreEntrevistado; }
            set { _NombreEntrevistado = value; }
        }

        private string _RazonSeleccion;
        public string razonSeleccion
        {
            get { return _RazonSeleccion; }
            set { _RazonSeleccion = value; }
        }

        private string _RazonDespido;
        public string razonDespido
        {
            get { return _RazonDespido; }
            set { _RazonDespido = value; }
        }

        public DEmpleado()
        {

        }

        public DEmpleado(int IdEmpleado, int IdDepartamento, string Nombre, string Apellido, string Cedula, DateTime FechaNacimiento, string Nacionalidad, string Direccion, string Email, string Telefono, string Curriculum, string EstadoLegal, int Status)
        {
            this.idEmpleado = IdEmpleado;
            this.idDepartamento = IdDepartamento;
            this.nombre = Nombre;
            this.apellido = Apellido;
            this.cedula = Cedula;
            this.fechaNacimiento = FechaNacimiento;
            this.nacionalidad = Nacionalidad;
            this.direccion = Direccion;
            this.email = Email;
            this.telefono = Telefono;
            this.curriculum = Curriculum;
            this.estadoLegal = EstadoLegal;
            this.status = Status;
        }
    }
}
