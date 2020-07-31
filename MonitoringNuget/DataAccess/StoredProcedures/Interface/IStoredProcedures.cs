﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.EntityFramework;

namespace MonitoringNuget.DataAccess.StoredProcedures.Interface
{
    public interface IStoredProcedures
    {
        bool LogClear(int logId);

        bool AddMessage(string message, string podname, int severity, string hostname);

        IQueryable<LoadHierarchy_Result> LoadHierarchy();

    }
}
