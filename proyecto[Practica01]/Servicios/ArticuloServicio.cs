using proyecto_Practica01_.Datos.ADO;
using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Servicios
{
    public class ArticuloServicio
    {
        private IArticuloRepository _repositorio;


        public ArticuloServicio()
        {
            _repositorio = new ArticuloRepo_ADO();
        }
        public List<Articulo> GetAll()
        {
            return _repositorio.GetAll();
        }
        public Articulo GetById(int id)
        {
            return _repositorio.GetById(id);
        }
        public bool Save(Articulo oArticulo)
        {
            return _repositorio.Save(oArticulo);
        }
        public bool Delete(int id)
        {
            return _repositorio.Delete(id);
        }
    }
}
