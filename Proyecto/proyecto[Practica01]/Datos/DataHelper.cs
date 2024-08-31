using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos
{
    public class DataHelper
    {
        private SqlConnection _cnn;
        private static DataHelper _instance;

        private DataHelper()
        {
            _cnn = new SqlConnection(Properties.Resources.cnnString);
        }
        public static DataHelper GetInstance() 
        {
            if(_instance == null) 
            {
                _instance = new DataHelper();
            }
            return _instance;
        }
        public static SqlConnection GetConnection() 
        {
            return _instance._cnn;
        }

        public DataTable ExecuteSPQuery(string sp, List<Parametros>? parametros) 
        {
            DataTable dt = new DataTable();
            
            _cnn.Open();
            SqlCommand cmd = new SqlCommand(sp, _cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                if (parametros != null)
                {
                    foreach (var param in parametros)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }
                dt.Load(cmd.ExecuteReader());
                _cnn.Close();
            }
            catch (SqlException)
            {
                dt = null;
                throw;
            }
            finally 
            {
                if (_cnn != null && _cnn.State == ConnectionState.Open) 
                {
                    _cnn.Close();
                }
            }
            return dt;
        }
        public int ExecuteSPNonQuery(string sp, List<Parametros> parametros) 
        {
            int rows = 0;

            try
            {
                _cnn.Open();
                
                SqlCommand cmd = new SqlCommand(sp, _cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parametros!=null) 
                {
                    foreach (var param in parametros)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }

                rows = cmd.ExecuteNonQuery();
                _cnn.Close();
            }
            catch (SqlException)
            {
                rows = -1;
                throw;
            }
            finally 
            {
                if(_cnn != null && _cnn.State == ConnectionState.Open) 
                {
                    _cnn.Close();
                }
            }
            return rows;
        }
    }
}
