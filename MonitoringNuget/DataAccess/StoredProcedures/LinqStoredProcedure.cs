﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;
using MonitoringNuget.EntityFramework;

namespace MonitoringNuget.DataAccess.StoredProcedures
{
    public class LinqStoredProcedure : IStoredProcedures
    {
        public bool LogClear(int logId)
        {
            using (var DbContext = new LinqDBContext.LinqDBContext())
            {
                var i = DbContext.LogClear(logId);
            }

            return true;
        }

        public bool AddMessage(
            string message
          , string podname
          , int severity
          , string hostname)
        {
            using (var DbContext = new LinqDBContext.LinqDBContext())
            {
                var i = DbContext.LogMessageAdd(message, podname, severity, hostname);
            }

            return true;
        }

        public IQueryable<LoadHierarchy_Result> LoadHierarchy()
        {
            throw new NotImplementedException();
        }
    }
}
