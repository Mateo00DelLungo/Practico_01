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
        public DetalleFactura()
        {
            Id = 0;
            _Articulo = new Articulo();
            Cantidad = 0;
        }
        public DetalleFactura(int id, Articulo articulo, int cantidad)
        {
            Id = id;
            _Articulo = articulo;
            Cantidad = cantidad;
        }
        public double CalcularSubTotal() 
        {
            return Cantidad * _Articulo.PrecioUnitario;
        }


    }
}
