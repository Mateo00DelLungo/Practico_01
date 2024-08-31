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
    public class ClienteServicio
    {
        private readonly IClienteRepository _repositorio;

        public ClienteServicio()
        {
            _repositorio = new ClienteRepo_ADO();
        }

        public List<Cliente> GetAll()
        {
            return _repositorio.GetAll();
        }
        public Cliente GetById(int id)
        {
            return _repositorio.GetById(id);
        }
        public bool Save(Cliente oCliente)
        {
            return _repositorio.Save(oCliente);
        }
        public bool Delete(int id)
        {
            return _repositorio.Delete(id);
        }
    }

}
