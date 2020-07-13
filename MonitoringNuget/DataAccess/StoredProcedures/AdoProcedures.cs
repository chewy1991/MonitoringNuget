using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonitoringNuget.DataAccess.AbstractClass;
using MonitoringNuget.DataAccess.RepositoryClasses;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;
using MonitoringNuget.Models;

namespace MonitoringNuget.DataAccess.StoredProcedures
{
    public class AdoProcedures : IStoredProcedures
    {
        private RepositoryBase<Logging> dbconn = new LoggingRepository();
        public bool LogClear(int logId)
        {
            try
            {
                using (var conn = new SqlConnection(dbconn.ConnectionString))
                {
                    using (var cmd = new SqlCommand("LogClear", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = logId;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public bool AddMessage(string message, string podname, int severity, string hostname)
        {
            try
            {
                using (var conn = new SqlConnection(dbconn.ConnectionString))
                {
                    using (var cmd = new SqlCommand("LogMessageAdd", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@logmessage", SqlDbType.NVarChar).Value = message;
                        cmd.Parameters.Add("@PodName", SqlDbType.NVarChar).Value = podname;
                        cmd.Parameters.Add("@Severity", SqlDbType.Int).Value = severity;
                        cmd.Parameters.Add("@hostname", SqlDbType.NVarChar).Value = hostname;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
