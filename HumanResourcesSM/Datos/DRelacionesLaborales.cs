using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DRelacionesLaborales:Conexion
    {

        private int _IdRelacionesLaborales;
        public int idRelacionesLaborales
        {
            get { return _IdRelacionesLaborales; }
            set { _IdRelacionesLaborales = value; }
        }

        private int _IdEmpleado;
        public int idEmpleado
        {
            get { return _IdEmpleado; }
            set { _IdEmpleado = value; }
        }

        private int _IdTipoTramite;
        public int idTipoTramite
        {
            get { return _IdTipoTramite; }
            set { _IdTipoTramite = value; }
        }

        private DateTime _FechaTramite;
        public DateTime fechaTramite
        {
            get { return _FechaTramite; }
            set { _FechaTramite = value; }
        }

        private string _DocumentoUrl;
        public string documentoUrl
        {
            get { return _DocumentoUrl; }
            set { _DocumentoUrl = value; }
        }

        private string _NombreEmpleado;
        public string nombreEmpleado
        {
            get { return _NombreEmpleado; }
            set { _NombreEmpleado = value; }
        }

        private string _NombreTramite;
        public string nombreTramite
        {
            get { return _NombreTramite; }
            set { _NombreTramite = value; }
        }

        private string _CedulaEmpleado;
        public string cedulaEmpleado
        {
            get { return _CedulaEmpleado; }
            set { _CedulaEmpleado = value; }
        }

        private string _UsuarioEmisor;
        public string usuarioEmisor
        {
            get { return _UsuarioEmisor; }
            set { _UsuarioEmisor = value; }
        }

        public DRelacionesLaborales()
        {

        }


        public DRelacionesLaborales(int IdRelacionesLaborales, int IdEmpleado, int IdTipoTramite, DateTime FechaTramite, string DocumentoUrl)
        {
            this.idRelacionesLaborales = IdRelacionesLaborales;
            this.idEmpleado = IdEmpleado;
            this.idTipoTramite = IdTipoTramite;
            this.fechaTramite = FechaTramite;
            this.documentoUrl = DocumentoUrl;
        }
    }
}
