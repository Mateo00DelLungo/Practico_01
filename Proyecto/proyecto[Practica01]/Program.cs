using proyecto_Practica01_.Datos.ADO;
using proyecto_Practica01_.Dominio;
using proyecto_Practica01_.Datos.Interfaces;
using proyecto_Practica01_.Servicios;

Console.WriteLine("Hello, World!");
        //nuevo articulo => id = 0
Articulo a1 = new Articulo(0, "Pepsi", 2.5);


Console.WriteLine(a1);



ArticuloServicio service = new ArticuloServicio();


if (service.Save(a1))
{
    Console.WriteLine("Guardado con exito");

}
//else
//{
//    Console.WriteLine("No se pudo guardar");
//}


