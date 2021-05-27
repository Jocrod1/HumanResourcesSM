using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DSeguridad:Conexion
    {

        private int _IdSeguridad;
        public int idSeguridad
        {
            get { return _IdSeguridad; }
            set { _IdSeguridad = value; }
        }

        private int _IdUsuario;
        public int idUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }

        private string _Pregunta;
        public string pregunta
        {
            get { return _Pregunta; }
            set { _Pregunta = value; }
        }

        private string _Respuesta;
        public string respuesta
        {
            get { return _Respuesta; }
            set { _Respuesta = value; }
        }

        private string _Usuario;
        public string usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }

        public DSeguridad()
        {

        }

        public DSeguridad(int IdUsuario, string Pregunta, string Respuesta)
        {
            this.idUsuario = IdUsuario;
            this.pregunta = Pregunta;
            this.respuesta = Respuesta;
        }
    }
}
