using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        void Dispose();
        void SaveChanges();
    }
}
