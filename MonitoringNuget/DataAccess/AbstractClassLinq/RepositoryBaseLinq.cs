using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace MonitoringNuget.DataAccess.AbstractClassLinq
{
    public abstract class RepositoryBaseLinq<T>: IRepositoryBase<T>
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

        public abstract string TableName { get; }

        public abstract void Add(T entity);

        public long Count(string whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        public abstract long Count(Func<T,bool> whereCondition, Dictionary<string, object> parameterValues);

        public abstract long Count();

        public abstract void Delete(T entity);

        public abstract IQueryable<T> GetAll(string whereCondition, Dictionary<string, object> parameterValues);
        
        public abstract IQueryable<T> GetAll(Func<T, bool> whereCondition, Dictionary<string, object> parameterValues);

        public abstract IQueryable<T> GetAll();

        public abstract T GetSingle<P>(P pkValue);

        public abstract void Update(T entity);
    }
}
