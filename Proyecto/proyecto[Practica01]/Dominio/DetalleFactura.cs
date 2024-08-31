using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Dominio
{
    public class DetalleFactura
    {
        public int Id { get; set; }
        public Articulo _Articulo { get; set; }
        public int Cantidad { get; set; }

        public double CalcularSubTotal() 
        {
            return Cantidad * _Articulo.PrecioUnitario;
        }


    }
}
