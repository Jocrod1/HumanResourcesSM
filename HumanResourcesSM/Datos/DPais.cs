using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DPais:Conexion
    {
        private string codigo;
        private string pais;

        public string Codigo { get => codigo; set => codigo = value; }
        public string Pais { get => pais; set => pais = value; }
        
        public DPais()
        {

        }

        public DPais(string codigo, string pais)
        {
            Codigo = codigo;
            Pais = pais;
        }
    }
}
