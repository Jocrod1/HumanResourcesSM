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
