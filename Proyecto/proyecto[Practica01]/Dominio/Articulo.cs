using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Dominio
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double PrecioUnitario { get; set; }
        public Articulo()
        {
            Id = 0;
            Nombre = "";
            PrecioUnitario = 0;
        }

        public Articulo(int id, string nombre, double precio)
        {
            Id = id;
            Nombre = nombre;
            PrecioUnitario = precio;
        }

        public override string ToString()
        {
            return "[" + Id + "] " + Nombre + " - $" + PrecioUnitario;
        }
    }
}
