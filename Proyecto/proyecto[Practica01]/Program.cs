using proyecto_Practica01_.Datos.ADO;
using proyecto_Practica01_.Dominio;
using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Servicios;
using proyecto_Practica01_.Datos;

////////////////////////////////////////////////////////////
/// Mateo Del Lungo    legajo: 412337
/// 
ArticuloServicio ManagerArticulo = new ArticuloServicio();
ClienteServicio ManagerCliente = new ClienteServicio();


Console.WriteLine("-------------------------------------------");
Console.WriteLine("Programa Factura");
Console.WriteLine("-------------------------------------------");

////Carga de artículos
///
Articulo articulo1 = new Articulo(0,"Helado 1kg",300.0);
Articulo articulo2 = new Articulo(0,"Jamón en fetas",200.0);
Articulo articulo3 = new Articulo(0,"Pepsi 2L",230.0);
Articulo articulo4 = new Articulo(0,"Coca de vidrio",89.0);
List<Articulo> articulos = new List<Articulo> { articulo1,articulo2,articulo3,articulo4 };

/////Guardamos en la base de datos
///
foreach (Articulo oArticulo in articulos)
{
    if (ManagerArticulo.Save(oArticulo))
    {
        Console.WriteLine("Articulo: " + oArticulo.ToString() + " guardado con éxito");
    }
    else { Console.WriteLine("Error al guardar"); }
}



///////// Carga de Clientes
///
Console.WriteLine("                        ");
Console.WriteLine("////////////////////////");
Console.WriteLine("Carga Clientes");
Console.WriteLine("////////////////////////");
Cliente cliente1 = new Cliente(0,"Juan","Perez");
Cliente cliente2 = new Cliente(0, "Mateo", "Del Lungo");
Cliente cliente3 = new Cliente(0, "Franco", "Diaz");
Cliente cliente4 = new Cliente(0, "Bianca", "Sosa");
Cliente cliente5 = new Cliente(0, "Esteban", "Quito");
List<Cliente> clientes = new List<Cliente> { cliente1, cliente2, cliente3, cliente4, cliente5 };

foreach (Cliente oCliente in clientes)
{
    if (ManagerCliente.Save(oCliente))
    {
        Console.WriteLine("Cliente: [ "+oCliente.ToString() + " ] guardado con éxito");
    }
    else { Console.WriteLine("Error al guardar"); }
}
FormaPago formapagoEfectivo = new FormaPago(1,"efectivo");


////////Carga de Facturas con transacción
///
UnitOfWork UnidadTrabajo = new UnitOfWork();
FacturaServicio ManagerFactura = new FacturaServicio(UnidadTrabajo);


List<DetalleFactura> detalles = new List<DetalleFactura>();
Factura Factura1 = new Factura(0,1,DateTime.Now,formapagoEfectivo,cliente2,detalles);
DetalleFactura detalle1 = new DetalleFactura(0,articulo4,2);
Factura1.AgregarDetalle(detalle1);

Console.WriteLine("                        ");
Console.WriteLine("////////////////////////");
Console.WriteLine("Carga Facturas");
Console.WriteLine("////////////////////////");
if (ManagerFactura.Save(Factura1, true)) 
{
    Console.WriteLine("Factura guardada con éxito");
}





