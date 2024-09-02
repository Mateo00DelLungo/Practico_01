using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos.ADO
{
    //MATEO DEL LUNGO
    public class FacturaRepo_ADO : IFacturaRepository
    {
        private SqlTransaction _transaction;
        public FacturaRepo_ADO(SqlTransaction t)
        {
            _transaction = t;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Factura GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Factura> GetAll()
        {
            throw new NotImplementedException();
        }
        
        public bool Save(Factura oFactura, bool esInsert)
        {
            bool result = false;
            string queryMaster = "SP_INSERT_FACTURAS";
            string queryDetalles = "SP_INSERT_DETALLES";
            var detalles = oFactura.ObtenerDetalles();
            List<Parametro> parametrosMaster = new List<Parametro>();
            if (oFactura != null)
            {
                
                List<string> nombresMaster = new List<string>() { "@fecha", "@formapagoid", "@clienteid", "@nrofactura" };
                List<Object> valoresMaster = new List<Object>() { oFactura.Fecha, oFactura._FormaPago.Id, oFactura._Cliente.Id, oFactura.NroFactura };
                if (!esInsert) 
                {
                    nombresMaster.Insert(0,"@facturaid");
                    valoresMaster.Insert(0, oFactura.Id);
                    queryMaster = "SP_UPDATE_FACTURAS";
                } 
                parametrosMaster = Parametro.LoadParamList(nombresMaster, valoresMaster);
            }
            else 
            {
                throw new Exception("La Factura no puede ser nula");
            }
            try
            {
                int registros = 0;
                var helper = DataHelper.GetInstance();
                //factura nueva creada
                (int filasFacturas,int idfactura) = helper.ExecuteSPNonQueryMaster(queryMaster,0,parametrosMaster,_transaction);
                foreach (var detalle in detalles) 
                {
                    List<String> nombresDetalle = new List<String>() { "@articuloid", "@facturaid", "@cantidad" };
                    List<Object> valoresDetalle = new List<Object>() { detalle._Articulo.Id, idfactura, detalle.Cantidad};
                    if (!esInsert) 
                    {
                        idfactura = oFactura.Id;
                        nombresDetalle.Insert(0, "@detalleid");
                        valoresDetalle.Insert(0,detalle.Id);
                        queryDetalles = "SP_UPDATE_DETALLES";
                    }
                    List<Parametro> parametrosDetalles = Parametro.LoadParamList(nombresDetalle,valoresDetalle); 
                    registros = helper.ExecuteSPNonQueryDetalles(queryDetalles,parametrosDetalles,_transaction);
                }
                result = (filasFacturas==1 && detalles.Count == registros);
            }
            catch (SqlException)
            {
                result = false;
                throw;
            }
            return result;
        }
    }
}
