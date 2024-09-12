using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Dominio;
using proyecto_Practica01_.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos.ADO
{
    //MATEO DEL LUNGO
    public class FacturaRepo_ADO : IFacturaRepository
    {
        private FormaPagoServicio _formaPagoManager = new FormaPagoServicio();
        private ArticuloServicio _articuloManager = new ArticuloServicio();
        private ClienteServicio _clienteManager = new ClienteServicio();
        
        public FacturaRepo_ADO()
        {
        }
        public Factura MapeoFactura(DataRow row)
        {
            int id = Convert.ToInt32(row[0]);
            int nrofactura = Convert.ToInt32(row["nro_factura"]);
            DateTime fecha = Convert.ToDateTime(row["fecha"]);
            
            int formapagoid = Convert.ToInt32(row["forma_pago_id"]);
            FormaPago oFormapago = _formaPagoManager.GetById(formapagoid);

            int clienteid = Convert.ToInt32(row["cliente_id"]);
            Cliente oCliente = _clienteManager.GetById(clienteid);

            List<DetalleFactura> detalles = new List<DetalleFactura>();
            Factura oFactura = new Factura(id, nrofactura, fecha,oFormapago,oCliente,detalles);
            return oFactura;
        }
        private DetalleFactura MapeoDetalle(DataRow row) 
        {
            DetalleFactura oDetalle = new DetalleFactura();
            int id = Convert.ToInt32(row[0]);

            int articuloid = Convert.ToInt32(row["articulo_id"]);
            Articulo oArticulo = _articuloManager.GetById(articuloid);

            int cantidad = Convert.ToInt32(row["cantidad"]);
            return oDetalle;
        }
        public int DeleteSoloDetalle(int idfactura, int iddetalle)
        {
            int filas = 0;
            try
            {
                var helper = DataHelper.GetInstance();
                List<Parametro> parametros = new List<Parametro>()
                {
                    new Parametro("@facturaid", idfactura),
                    new Parametro("@detalleid",iddetalle)
                };
                filas = helper.ExecuteSPNonQueryDetalles("SP_DELETE_DETALLES", parametros);
                //t.commit y cnn.close
                UnitOfWork.SaveChanges();
            }
            catch (SqlException)
            {
                return filas = 0;
                throw;
            }
            finally 
            {
                var cnn = DataHelper.GetConnection();
                if(cnn.State == ConnectionState.Open && cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            return filas;
        }
        public int DeleteDetalle(int idfactura,int iddetalle) 
        {
            int filas = 0;
            try
            {
                var helper = DataHelper.GetInstance();
                List<Parametro> parametros = new List<Parametro>()
                {
                    new Parametro("@facturaid", idfactura),
                    new Parametro("@detalleid",iddetalle)
                };
                filas = helper.ExecuteSPNonQueryDetalles("SP_DELETE_DETALLES", parametros);
                //t.commit y cnn.close
            }
            catch (SqlException)
            {
                return filas = 0;
                throw;
            }
            return filas;
        }
        public bool DeleteFactura(Factura oFactura)
        {
            bool result = false;
            List<Parametro>parametros = new List<Parametro>()
            { new Parametro("@facturaid", oFactura.Id) };
            try
            {
                int filasdetalle = 0;
                var detalles = oFactura.ObtenerDetalles();
                foreach (var detalle in detalles)
                {
                    filasdetalle = DeleteDetalle(oFactura.Id, detalle.Id);
                }
                var helper = DataHelper.GetInstance();
                (int filas, int output) = helper.ExecuteSPNonQueryMaster("SP_DELETE_FACTURAS", oFactura.Id, parametros);
                if (filasdetalle == detalles.Count() && filas == 1)
                {
                    result = true;
                }
                UnitOfWork.SaveChanges();
            }
            catch (SqlException)
            {
                return false;
                throw;
            }
            finally 
            {
                var cnn = DataHelper.GetConnection();
                if (cnn.State == ConnectionState.Open && cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            return result;
        }
        public Factura GetById(int id)
        {
            Factura oFactura = new Factura();
            List<Parametro> parametros = new List<Parametro>()
            { new Parametro("@id", id) };
            try
            {
                var helper = DataHelper.GetInstance();
                //obtenemos la factura
                DataTable dtMaster = helper.ExecuteSPQuery("SP_GET_BYID_FACTURAS",parametros);
                oFactura = MapeoFactura(dtMaster.Rows[0]);

                //obtenemos los detalles de la factura
                parametros[0].Name = "@facturaid";
                DataTable dtDetalle = helper.ExecuteSPQuery("SP_GET_DETALLE",parametros);
                foreach (DataRow row in dtDetalle.Rows) 
                {
                    DetalleFactura detalle = MapeoDetalle(row);
                    oFactura.AgregarDetalle(detalle);
                }
                
            }
            catch (SqlException)
            {
                return null;
                throw;
            }
            return oFactura;
        }
        public List<Factura> GetAll()
        {
            List<Factura> facturas = new List<Factura>();
            var helper = DataHelper.GetInstance();
            DataTable dtMaster = helper.ExecuteSPQuery("SP_GET_MASTER",null);
            //por cada factura obtenemos sus detalles
            foreach (DataRow row in dtMaster.Rows) 
            {
                Factura oFactura = MapeoFactura(row);
                List<Parametro> parametros = new List<Parametro>() 
                { new Parametro("@facturaid",oFactura.Id)};
                
                DataTable dtDetalle = helper.ExecuteSPQuery("SP_GET_DETALLE", parametros);
                foreach(DataRow rowdetalle in dtDetalle.Rows) 
                {
                    //por cada detalle obtenemos sus datos
                    DetalleFactura oDetalle = MapeoDetalle(rowdetalle);
                    oFactura.AgregarDetalle(oDetalle);
                    //se agrega a la lista detalle de la factura
                }
                facturas.Add(oFactura);
            }
            return facturas;
        }
        public List<Parametro> CargarParametrosMaster(Factura oFactura)
        {
            List<string> nombresMaster = new List<string>() { "@fecha", "@formapagoid", "@clienteid", "@nrofactura" };
            List<object> valoresMaster = new List<object>() { oFactura.Fecha, oFactura._FormaPago.Id, oFactura._Cliente.Id, oFactura.NroFactura };
            List<Parametro> parametrosMaster = Parametro.LoadParamList(nombresMaster, valoresMaster);
            return parametrosMaster;
        }
        public List<Parametro> CargarParametrosDetalles(DetalleFactura oDetalle, int facturaid)
        {
            List<string> nombresDetalle = new List<string>() { "@articuloid", "@facturaid", "@cantidad" };
            List<object> valoresDetalle = new List<object>() { oDetalle._Articulo.Id, facturaid, oDetalle.Cantidad };
            List<Parametro> parametrosDetalles = Parametro.LoadParamList(nombresDetalle, valoresDetalle);
            return parametrosDetalles;
        }
        public bool Save(Factura oFactura, bool esInsert)
        {
            int facturaidIn = 0;
            bool result = false;
            List<Parametro> parametrosMaster = new List<Parametro>();
            string queryMaster = "SP_SAVE_FACTURAS";
            string queryDetalles = "SP_SAVE_DETALLES";
            var detalles = oFactura.ObtenerDetalles();

            if (oFactura != null)
            {
                parametrosMaster = CargarParametrosMaster(oFactura);
                if (!esInsert) 
                {
                    facturaidIn = oFactura.Id;
                    parametrosMaster.Insert(0,new Parametro("@facturaid", oFactura.Id));
                    queryMaster = "SP_UPDATE_FACTURAS";
                } 
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
                //se abre la conexion
                (int filasFacturas,int facturaidOut) = helper.ExecuteSPNonQueryMaster(queryMaster,facturaidIn,parametrosMaster);
                if (facturaidIn != 0) {facturaidOut = facturaidIn;}
                //si no es insert ya se conoce el id factura
                foreach (var detalle in detalles) 
                {
                    List<Parametro> parametrosDetalles = CargarParametrosDetalles(detalle, facturaidOut);
                    if (!esInsert) 
                    {
                        parametrosDetalles.Insert(0, new Parametro("@detalleid", detalle.Id));
                        queryDetalles = "SP_UPDATE_DETALLES";
                    }
                    registros = helper.ExecuteSPNonQueryDetalles(queryDetalles,parametrosDetalles);
                    //se cierra la conexion y se guardan los cambios
                    UnitOfWork.SaveChanges();
                }
                result = (filasFacturas==1 && detalles.Count == registros);
            }
            catch (SqlException)
            {
                result = false;
                throw;
            }
            finally
            {
                var cnn = DataHelper.GetConnection();
                if(cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            return result;
        }
    }
}
