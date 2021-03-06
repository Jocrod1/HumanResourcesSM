﻿using System;
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

        private string _Nacimiento;
        public string nacimiento
        {
            get { return _Nacimiento; }
            set { _Nacimiento = value; }
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

        private int _Telefono;
        public int telefono
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

        private int _EstadoLegal;
        public int estadoLegal
        {
            get { return _EstadoLegal; }
            set { _EstadoLegal = value; }
        }

        private DateTime _FechaCulminacion;
        public DateTime fechaCulminacion
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

        public DEmpleado()
        {

        }

        public DEmpleado(int IdEmpleado, int IdDepartamento, string Nombre, string Apellido, string Cedula, DateTime FechaNacimiento, string Nacimiento, string Direccion, string Email, int Telefono, string Curriculum, int EstadoLegal, DateTime FechaCulminacion, int Status, DateTime FechaAplicacion, DateTime FechaRevision, string NombrePuesto)
        {
            this.idEmpleado = IdEmpleado;
            this.idDepartamento = IdDepartamento;
            this.nombre = Nombre;
            this.apellido = Apellido;
            this.cedula = Cedula;
            this.fechaNacimiento = FechaNacimiento;
            this.nacimiento = Nacimiento;
            this.direccion = Direccion;
            this.email = Email;
            this.telefono = Telefono;
            this.curriculum = Curriculum;
            this.estadoLegal = EstadoLegal;
            this.fechaCulminacion = FechaCulminacion;
            this.status = Status;
            this.fechaAplicacion = FechaAplicacion;
            this.fechaRevision = FechaRevision;
            this.nombrePuesto = NombrePuesto;
        }
    }
}
