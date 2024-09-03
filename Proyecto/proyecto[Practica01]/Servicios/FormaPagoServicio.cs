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
    public class FormaPagoServicio
    {
        private readonly IFormaPagoRepository _repositorio;
        public FormaPagoServicio()
        {
            _repositorio = new FormaPagoRepo_ADO();
        }
        public FormaPago GetById(int id) 
        {
            return _repositorio.GetById(id);
        }
    }
}
