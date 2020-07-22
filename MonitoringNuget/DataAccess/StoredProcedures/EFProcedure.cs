using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;
using MonitoringNuget.EntityFramework;

namespace MonitoringNuget.DataAccess.StoredProcedures
{
    public class EFProcedure : IStoredProcedures
    {
        public bool LogClear(int logId)
        {
            throw new NotImplementedException();
        }

        public bool AddMessage(
            string message
          , string podname
          , int severity
          , string hostname)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LoadHierarchy_Result> LoadHierarchy()
        {
            var context = new TestatEntities();

            return context.LoadHierarchy().AsQueryable();
        }
    }
}
