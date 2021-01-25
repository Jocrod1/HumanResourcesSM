using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DTipoTramite:Conexion
    {

        private int _IdTipoTramite;
        public int idTipoTramite
        {
            get { return _IdTipoTramite; }
            set { _IdTipoTramite = value; }
        }

        private string _Nombre;
        public string nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private string _StatusCambio;
        public string statusCambio
        {
            get { return _StatusCambio; }
            set { _StatusCambio = value; }
        }


        public DTipoTramite()
        {

        }

        public DTipoTramite(int IdTipoTramite, string Nombre, string StatusCambio)
        {
            this.idTipoTramite = IdTipoTramite;
            this.nombre = Nombre;
            this.statusCambio = StatusCambio;
        }
    }
}
