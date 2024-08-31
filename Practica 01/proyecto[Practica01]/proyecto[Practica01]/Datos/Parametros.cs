using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos
{
    public class Parametros
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Parametros(string name, object value)
        {
            Name = name;
            Value = value;
        }

    }
}
