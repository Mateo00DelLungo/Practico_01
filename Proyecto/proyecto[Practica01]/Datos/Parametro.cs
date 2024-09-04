using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos
{
    public class Parametro
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Parametro(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public void LoadParameterToCmd(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue(Name, Value);
        }
        public static List<Parametro> LoadParamList(List<string> listaNombres, List<object> listaValores)
        {
            List<Parametro> parametros = new List<Parametro>();
            for (int i = 0; i < listaNombres.Count; i++)
            {
                parametros.Add(new Parametro(listaNombres[i], listaValores[i]));
            }
            return parametros;
        }
    }
}
