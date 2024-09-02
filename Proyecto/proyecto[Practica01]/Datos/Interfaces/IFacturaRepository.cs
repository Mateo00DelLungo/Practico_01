﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proyecto_Practica01_.Dominio;

namespace proyecto_Practica01_.Datos.Interfaces
{
    public interface IFacturaRepository
    {
        //CRUD
        List<Factura> GetAll();
        Factura GetById(int id);
        bool Save(Factura oFactura, bool esInsert);
       
        bool Delete(int id);
    }
}
