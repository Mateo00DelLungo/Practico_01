using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Dominio
{
    public class Factura
    {
        public int Id { get; set; }
        public int NroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public FormaPago _FormaPago { get; set; }
        public Cliente _Cliente { get; set; }
        private List<DetalleFactura> _detalles { get; set; }

        public Factura()
        {
            _detalles = new List<DetalleFactura>();
        }
        public Factura(int id, int nrofactura, DateTime fecha, FormaPago formaPago, Cliente cliente, List<DetalleFactura> detalles)
        {
            Id = id;
            NroFactura = nrofactura;
            Fecha = fecha;
            _FormaPago = formaPago;
            _Cliente = cliente;
            _detalles = detalles;
        }
        public List<DetalleFactura> ObtenerDetalles() 
        {
            return _detalles;
        }

        public void AgregarDetalle(DetalleFactura oDetalle) 
        {
            if (oDetalle != null) 
            {
                _detalles.Add(oDetalle);
            }
        }

        public void BorrarDetalle(int index) 
        {
            _detalles.RemoveAt(index);
        }

        public double CalcularMontoTotal() 
        {
            double total = 0;
            foreach (var detalle in _detalles)
            {
                total += detalle.CalcularSubTotal();
            }
            return total;
        }
    }
}
