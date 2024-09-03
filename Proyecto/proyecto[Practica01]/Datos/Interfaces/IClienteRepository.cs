using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proyecto_Practica01_.Dominio;

namespace proyecto_Practica01_.Datos.Interfaces
{
    public interface IClienteRepository
    {
        List<Cliente> GetAll();
        Cliente GetById(int id);
        bool Save(Cliente oCliente);
        bool Delete(int id);
        Cliente Mapeo(DataRow row);

    }
}
