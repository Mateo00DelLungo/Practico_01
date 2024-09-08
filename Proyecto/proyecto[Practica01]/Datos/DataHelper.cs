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
        public DataTable ExecuteSPQuery(string sp, List<Parametro>? parametros) 
        {
            DataTable dt = new DataTable();

            if (_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
            SqlCommand cmd = new SqlCommand(sp, _cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlTransaction t = UnitOfWork.GetTransaction();
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
                UnitOfWork.SaveChanges();
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
        public (int affectedRows, int idout) ExecuteSPNonQuery(string sp, List<Parametro> parametros)
        {
            int rows = 0;
            int idout = 0;
            try
            {
                if (_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
                SqlCommand cmd = new SqlCommand(sp, _cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramout = null;

                if (parametros!=null) 
                {
                    if(parametros[0].Name == "@id" && (int)parametros[0].Value == 0) 
                    {
                        paramout = new SqlParameter("@param_out", SqlDbType.Int);
                        paramout.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramout);
                    }
                    foreach (var param in parametros)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }

                rows = cmd.ExecuteNonQuery();
                if (paramout != null) { idout = (int)paramout.Value; }
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
            return (rows,idout);
        }
        public (int affectedRows,int idOut) ExecuteSPNonQueryMaster(string spMaster, int facturaid,List<Parametro> masterParams)
        {
            int filas = 0;
            int idOutput = -1;
            try
            {
                if (_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
                SqlCommand cmdMaster = new SqlCommand(spMaster, _cnn);
                cmdMaster.Transaction = UnitOfWork.GetTransaction();
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
            return (filas,idOutput);
        }
        public int ExecuteSPNonQueryDetalles(string spDetail, List<Parametro> detailParams) //SqlTransaction t) 
        {
            int registros = 0;
            try
            {
                if(_cnn.State == ConnectionState.Closed) { _cnn.Open(); }
                SqlTransaction t = UnitOfWork.GetTransaction();
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
            return registros;
        }
    }
}
