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
    public class ArticuloRepo_ADO : IArticuloRepository
    {
        public bool Delete(int id)
        {

            List<Parametros> parametros = new List<Parametros>();
            parametros.Add(new Parametros("@id", id));
            int result = DataHelper.GetInstance().ExecuteSPNonQuery("SP_DELETE_ARTICULOS", parametros); 
            return 1 == result;
            
        }

        public Articulo Mapeo(DataRow row) 
        {
            int id = Convert.ToInt32(row[0]);
            string nombre = row["nombre"].ToString();
            double precio = Convert.ToDouble(row["precio_unitario"]);

            Articulo oArticulo = new Articulo(id, nombre, precio);
            return oArticulo;
        }

        public List<Articulo> GetAll()
        {
            List<Articulo> articulos = new List<Articulo>();
            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_ALL_ARTICULOS", null);

            foreach (DataRow row in dt.Rows)
            {
                Articulo oArticulo = Mapeo(row);
                articulos.Add(oArticulo);
            }

            return articulos;
        }

        public Articulo GetById(int id)
        {
            List<Parametros> parametros = new List<Parametros>
            {
                new Parametros("@id", id)
            };
            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_BYID_ARTICULOS", parametros);

            if (dt.Rows.Count > 0 && dt != null)
            {
                return Mapeo(dt.Rows[0]);
            }
            else 
            {
                return null;
            }
        }

        public bool Save(Articulo oArticulo)
        {
            List<Parametros> parametros = new List<Parametros>
            {
                new Parametros("@id", oArticulo.Id),
                new Parametros("@nombre", oArticulo.Nombre),
                new Parametros("@precio", oArticulo.PrecioUnitario)
            };
            int result = DataHelper.GetInstance().ExecuteSPNonQuery("SP_SAVE_ARTICULOS", parametros);
            return result == 1;
        }
    }
}
