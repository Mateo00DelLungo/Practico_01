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
    public class FormaPagoRepo_ADO : IFormaPagoRepository
    {
        public FormaPago GetById(int id)
        {
            List<Parametro> parametros = new List<Parametro>() { new Parametro("@id", id) };
            DataTable dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_BYID_FORMAS_PAGOS", parametros);
            if(dt!=null && dt.Rows.Count > 0) 
            {
                int idformapago = Convert.ToInt32(dt.Rows[0][0]);
                string nombre = Convert.ToString(dt.Rows[0]["nombre"]);
                FormaPago oFormapago = new FormaPago(idformapago,nombre);
                return oFormapago;
            }
            else { return null; }
        }
    }
}
