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
        public bool Delete(int id) 
        {
            return _unitOfWork.RepositorioFacturas.Delete(id);
        }
        public bool Save(Factura oFactura) 
        {
            return _unitOfWork.RepositorioFacturas.Save(oFactura);
        }
    }
}
