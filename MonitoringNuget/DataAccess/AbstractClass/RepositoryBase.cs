﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Windows;
using GenericRepository;
using MonitoringNuget.Models.ModelInterface;

namespace MonitoringNuget.DataAccess.AbstractClass
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T:IModel
    {
        protected string ConnectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

        public abstract string TableName { get; }

        public abstract void Add(T entity);

        public long Count(string whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName}";
                    return (long) cmd.ExecuteScalar();
                }
            }
        }

        public void Delete(T entity)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"DELETE FROM {TableName} WHERE Id = {entity.Id};";
                }
            }
        }

        public abstract List<T> GetAll(string whereCondition, Dictionary<string, object> parameterValues);
        
        public abstract List<T> GetAll();
        
        public abstract T GetSingle<P>(P pkValue);

        public abstract void Update(T entity);
    }
}

