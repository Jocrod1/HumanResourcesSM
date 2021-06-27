using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DSeleccion:Conexion
    {

        private int _IdSeleccion;
        public int idSeleccion
        {
            get { return _IdSeleccion; }
            set { _IdSeleccion = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private int _IdEntrevistador;
        public int idEntrevistador
        {
            get { return _IdEntrevistador; }
            set { _IdEntrevistador = value; }
        }

        private int _IdSeleccionador;
        public int idSeleccionador
        {
            get { return _IdSeleccionador; }
            set { _IdSeleccionador = value; }
        }

        private DateTime _FechaAplicacion;
        public DateTime fechaAplicacion
        {
            get { return _FechaAplicacion; }
            set { _FechaAplicacion = value; }
        }

        private int _Status;
        public int status
        {
            get { return _Status; }
            set { _Status = value; }
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

        private string _Razon;
        public string razon
        {
            get { return _Razon; }
            set { _Razon = value; }
        }

        private string _RazonFinal;
        public string razonFinal
        {
            get { return _RazonFinal; }
            set { _RazonFinal = value; }
        }


        public DSeleccion()
        {

        }

        public DSeleccion(int IdSeleccion, int IdEmpleado, int IdEntrevistador, int IdSeleccionador, DateTime FechaAplicacion, int Status, DateTime FechaRevision, string NombrePuesto)
        {
            this.idSeleccion = IdSeleccion;
            this.idEmpleado = IdEmpleado;
            this.idEntrevistador = IdEntrevistador;
            this.idSeleccionador = IdSeleccionador;
            this.fechaAplicacion = FechaAplicacion;
            this.status = Status;
            this.fechaRevision = FechaRevision;
            this.nombrePuesto = NombrePuesto;
        }

    }
}
