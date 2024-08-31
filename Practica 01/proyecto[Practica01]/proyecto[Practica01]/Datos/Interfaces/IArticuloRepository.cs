using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proyecto_Practica01_.Dominio;

namespace proyecto_Practica01_.Datos.Interfaces
{
    public interface IArticuloRepository
    {
        List<Articulo> GetAll();
        Articulo GetById(int id);
        bool Save(Articulo oArticulo);
        bool Delete(int id);
        Articulo Mapeo(DataRow row);

    }
}
