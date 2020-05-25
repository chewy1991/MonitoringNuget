using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using MonitoringNuget.DataAccess.AbstractClass;
using MonitoringNuget.Models;

namespace MonitoringNuget.DataAccess.RepositoryClasses
{
    public class LoggingRepository : RepositoryBase<Logging>
    {
        public override string TableName => "v_logentries";

        public override void Add(Logging entity)
        {
            throw new NotImplementedException();
        }

        public override List<Logging> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            var logginglist = new List<Logging>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName} WHERE {whereCondition};";

                    foreach (var key in parameterValues)
                        cmd.Parameters.AddWithValue($@"@{key.Key}", key.Value);
                    cmd.Prepare();
                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        var log = new Logging
                                  {
                                      Id = (int) row["Id"]
                                    , pod = (string) row["pod"]
                                    , hostname = (string) row["hostname"]
                                    , severity = (int) row["severity"]
                                    , location = (string) row["location"]
                                    , message = (string) row["message"]
                                    , timestamp = (DateTime) row["timestamp"]
                                  };
                        logginglist.Add(log);
                    }
                }
            }

            return logginglist;
        }

        public override List<Logging> GetAll()
        {
            var logginglist = new List<Logging>();

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT id, pod, location, hostname, severity, timestamp, message FROM {TableName};";

                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        var log = new Logging
                                  {
                                      Id = (int) row["Id"]
                                    , pod = (string) row["pod"]
                                    , hostname = (string) row["hostname"]
                                    , severity = (int) row["severity"]
                                    , location = (string) row["location"]
                                    , message = (string) row["message"]
                                    , timestamp = (DateTime) row["timestamp"]
                                  };
                        logginglist.Add(log);
                    }
                }
            }

            return logginglist;
        }

        public override Logging GetSingle<P>(P pkValue)
        {
            var log = new Logging();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName} WHERE Id = {pkValue};";

                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);

                    log = new Logging
                          {
                              Id = (int) dt.Rows[0]["Id"]
                            , pod = (string) dt.Rows[0]["pod"]
                            , hostname = (string) dt.Rows[0]["hostname"]
                            , severity = (int) dt.Rows[0]["severity"]
                            , location = (string) dt.Rows[0]["location"]
                            , message = (string) dt.Rows[0]["message"]
                            , timestamp = (DateTime) dt.Rows[0]["timestamp"]
                          };
                }
            }

            return log;
        }

        public override void Update(Logging entity)
        {
            throw new NotImplementedException();
        }

        public bool LogClear(int logId)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
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

        public bool AddMessage(string message, string podname,int severity,string hostname)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    using (var cmd = new SqlCommand("LogMessageAdd", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@logmessage", SqlDbType.NVarChar).Value = message;
                        cmd.Parameters.Add("@PodName", SqlDbType.NVarChar).Value    = podname;
                        cmd.Parameters.Add("@Severity", SqlDbType.Int).Value        = severity;
                        cmd.Parameters.Add("@hostname", SqlDbType.NVarChar).Value   = hostname;
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