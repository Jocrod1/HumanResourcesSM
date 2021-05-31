using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DAuditoria : Conexion
    {

        private int _IdAuditoria;
        public int idAuditoria
        {
            get { return _IdAuditoria; }
            set { _IdAuditoria = value; }
        }

        private int _IdTrabajador;
        public int idTrabajador
        {
            get { return _IdTrabajador; }
            set { _IdTrabajador = value; }
        }

        private string _Accion;
        public string accion
        {
            get { return _Accion; }
            set { _Accion = value; }
        }

        private string _Descripcion;
        public string descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        private DateTime _Fecha;
        public DateTime fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }

        private string _Usuario;
        public string usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }

        private string _RolString;
        public string rolString
        {
            get { return _RolString; }
            set { _RolString = value; }
        }

        public string fechaString
        {
            get
            {
                return fecha.ToShortDateString();
            }
        }

        public const string
            Ingresar = "Ingresar",
            Registrar = "Registrar",
            Editar = "Editar",
            Eliminar = "Eliminar",
            Anular = "Anular",
            Generar = "Generar",
            Backup = "Respaldo",
            Restore = "Restaurar",
            IniciarSesion = "Iniciar Sesión",
            CerrarSesion = "Cerrar Sesión";
            



        public DAuditoria()
        {

        }

        public DAuditoria(int IdTrabajador, string Accion, string Descripcion)
        {
            this.idTrabajador = IdTrabajador;
            this.accion = Accion;
            this.descripcion = Descripcion;
        }
    }
}
