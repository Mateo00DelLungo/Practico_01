using proyecto_Practica01_.Dominio;
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
            return DataHelper.GetInstance()._cnn;
        }
        public DataTable ExecuteSPQuery(string sp, List<Parametro>? parametros, SqlTransaction t) 
        {
            DataTable dt = new DataTable();

            if (_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
            SqlCommand cmd = new SqlCommand(sp, _cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (t != null) { cmd.Transaction = t; }

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
        public int ExecuteSPNonQuery(string sp, List<Parametro> parametros) 
        {
            int rows = 0;
            try
            {
                if (_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
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
        public (int affectedRows,int idOut) ExecuteSPNonQueryMaster(string spMaster, int facturaid,List<Parametro> masterParams, SqlTransaction t)
        {
            int filas = 0;
            int idOutput = -1;
            try
            {
                if (_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
                SqlCommand cmdMaster = new SqlCommand(spMaster, _cnn, t);
                cmdMaster.CommandType = CommandType.StoredProcedure;
                if (masterParams != null) 
                {
                    SqlParameter paramOut = new SqlParameter();
                    foreach (Parametro param in masterParams) 
                    {
                        //se cargan los parametros de entrada al comando
                        //cmd.parameter.addwithvalue
                        param.LoadParameterToCmd(cmdMaster);
                    }
                    if (facturaid == 0) // si la factura es nueva
                    {
                        //parametro de salida
                        paramOut = new SqlParameter("@facturaidout", SqlDbType.Int);
                        paramOut.Direction = ParameterDirection.Output;
                        cmdMaster.Parameters.Add(paramOut);
                        //devolvemos el id de la nueva factura
                    }
                    
                    filas = cmdMaster.ExecuteNonQuery();
                    idOutput = (int)paramOut.Value;
                    return (filas,idOutput);
                }

            }
            catch (SqlException)
            {
                return (-1,-1);
                throw;

            }
            finally 
            {
                if(_cnn.State == ConnectionState.Open && _cnn != null) 
                {
                    _cnn.Close();
                }
            }
            return (filas,idOutput);
        }
        public int ExecuteSPNonQueryDetalles(string spDetail, List<Parametro> detailParams, SqlTransaction t) 
        {
            int registros = 0;
            try
            {
                if(_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
                var cmdDetalle = new SqlCommand(spDetail, _cnn, t);
                cmdDetalle.CommandType = CommandType.StoredProcedure;
                //carga de parametros
                if (detailParams != null)
                {
                    foreach (Parametro param in detailParams)
                    { param.LoadParameterToCmd(cmdDetalle); }
                }
                //detalles agregados o afectados
                registros = cmdDetalle.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return -1;
                throw;
            }
            finally 
            {
                if(_cnn.State == ConnectionState.Open && _cnn != null) 
                {
                    _cnn.Close();
                }
            }
            
            return registros;
        }
    }
}
