using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos.ADO
{
    //MATEO DEL LUNGO
    public class ClienteRepo_ADO : IClienteRepository
    {
        public Cliente Mapeo(DataRow row) 
        {
            int id = Convert.ToInt32(row[0]);
            string nombre = row["nombre"].ToString();
            string apellido = row["apellido"].ToString();
            Cliente oCliente = new Cliente(id,nombre,apellido);
            return oCliente;
        }
        public bool Save(Cliente oCliente)
        {
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("@id", oCliente.Id));
            parametros.Add(new Parametro("@nombre", oCliente.Nombre));
            parametros.Add(new Parametro("@apellido", oCliente.Apellido));
            (int result, int idout)= DataHelper.GetInstance().ExecuteSPNonQuery("SP_SAVE_CLIENTES", parametros);
            if (idout != null) {oCliente.Id = idout; }
            return 1 == result;
        }

        public bool Delete(int id)
        {
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("@id", id));
            (int result, int idout) = DataHelper.GetInstance().ExecuteSPNonQuery("SP_DELETE_CLIENTES", parametros);
            return 1 == result;
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> clientes = new List<Cliente>();
            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_ALL_CLIENTS", null);
            foreach (DataRow row in dt.Rows) 
            {
                Cliente oCliente = Mapeo(row);
                clientes.Add(oCliente);
            }
            return clientes;
        }

        public Cliente GetById(int id)
        {
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("@id", id));
            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_BYID_CLIENTS", parametros);
            if (dt.Rows.Count > 0 && dt != null)
            {
                Cliente oCliente = Mapeo(dt.Rows[0]);
                return oCliente;
            }
            else
            {
                return null;
            }
            
        }
    }
}
