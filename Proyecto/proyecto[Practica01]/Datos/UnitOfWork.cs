using proyecto_Practica01_.Datos.ADO;
using proyecto_Practica01_.Datos.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos
{
    public class UnitOfWork : IDisposable
    {
        private  static SqlTransaction _transaction = null;
        private  static SqlConnection _connection;
        private IFacturaRepository _repositorioFacturas;
        public IFacturaRepository RepositorioFacturas 
        { get 
            { if (_repositorioFacturas == null) 
                {
                    _repositorioFacturas= new FacturaRepo_ADO();
                }
                return _repositorioFacturas;
            } 
        }

        public UnitOfWork()
        {
            _connection = DataHelper.GetConnection();
        }
        public static SqlTransaction GetTransaction() 
        {
            if (_transaction != null)
            {
                return _transaction;
            }
            else
            {
                BeginTransaction();
                return _transaction;
            }
        }
        public static void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya hay una transacción activa");
            }
            else
            {
                _transaction = _connection.BeginTransaction();
            }
        }
        public static void SaveChanges() 
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw new Exception("Error al guardar en la base de datos", ex);
            }
        }
        public void Dispose()
        {
            if (_transaction != null) 
            {
                _transaction.Dispose();
            }
            if(_connection != null) 
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

    }
}
