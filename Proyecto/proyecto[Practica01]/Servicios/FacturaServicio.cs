using proyecto_Practica01_.Datos;
using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Servicios
{
    public class FacturaServicio
    {
        private readonly UnitOfWork _unitOfWork;
        public FacturaServicio(UnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }

        public List<Factura> GetAll() 
        {
            return _unitOfWork.RepositorioFacturas.GetAll();
        }
        public Factura GetById(int id) 
        {
            return _unitOfWork.RepositorioFacturas.GetById(id);
        }
        public bool DeleteFactura(Factura oFactura) 
        {
            return _unitOfWork.RepositorioFacturas.DeleteFactura(oFactura);
        }
        public int DeleteDetalle(int idfactura, int iddetalle) 
        {
            return _unitOfWork.RepositorioFacturas.DeleteSoloDetalle(idfactura,iddetalle);
        }
        public bool Save(Factura oFactura, bool esInsert) 
        {
            return _unitOfWork.RepositorioFacturas.Save(oFactura, esInsert);
        }
        public void GuardarCambios() 
        {
            UnitOfWork.SaveChanges();
            //se llama a t.commit al final
        }
    }
}
