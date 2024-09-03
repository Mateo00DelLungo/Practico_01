using proyecto_Practica01_.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos.Interfaces
{
    public interface IFormaPagoRepository
    {
        FormaPago GetById(int id);
    }
}
