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
    public class ArticuloRepo_ADO : IArticuloRepository
    {
        public bool Delete(int id)
        {

            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro("@id", id));
            (int result, int idout) = DataHelper.GetInstance().ExecuteSPNonQuery("SP_DELETE_ARTICULOS", parametros); 
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
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id", id)
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
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id", oArticulo.Id),
                new Parametro("@nombre", oArticulo.Nombre),
                new Parametro("@precio", oArticulo.PrecioUnitario)
            };
            (int result, int idout) = DataHelper.GetInstance().ExecuteSPNonQuery("SP_SAVE_ARTICULOS", parametros);
            if (idout != null) { oArticulo.Id = idout; }
            return result == 1;
        }
    }
}
