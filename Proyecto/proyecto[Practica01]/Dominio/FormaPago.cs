using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Dominio
{
    public class FormaPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public FormaPago(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
