using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using MonitoringNuget.DataAccess.AbstractClass;
using MonitoringNuget.LinqDTO;
using MonitoringNuget.Models;

namespace MonitoringNuget.DataAccess.RepositoryClasses
{
    public class LoggingRepository : RepositoryBase<VLogentriesDTO>
    {
        public override string TableName => "v_logentries";

        public override void Add(VLogentriesDTO entity)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<VLogentriesDTO> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            var logginglist = new List<VLogentriesDTO>();
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
                        var log = new VLogentriesDTO
                                  {
                                      Id = (int) row["Id"]
                                    , Pod = (string) row["pod"]
                                    , Hostname = (string) row["hostname"]
                                    , Severity = (int) row["severity"]
                                    , Location = (string) row["location"]
                                    , Message = (string) row["message"]
                                    , Timestamp = (DateTime) row["timestamp"]
                                  };
                        logginglist.Add(log);
                    }
                }
            }

            return logginglist.AsQueryable();
        }

        public override IQueryable<VLogentriesDTO> GetAll()
        {
            var logginglist = new List<VLogentriesDTO>();

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
                        var log = new VLogentriesDTO
                                  {    Id = (int) row["Id"]
                                     , Pod = (string) row["pod"]
                                     , Hostname = (string) row["hostname"]
                                     , Severity = (int) row["severity"]
                                     , Location = (string) row["location"]
                                     , Message = (string) row["message"]
                                     , Timestamp = (DateTime) row["timestamp"]
                                   };
                        logginglist.Add(log);
                    }
                }
            }

            return logginglist.AsQueryable();
        }

        public override VLogentriesDTO GetSingle<P>(P pkValue)
        {
            var log = new VLogentriesDTO();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName} WHERE Id = {pkValue};";

                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);

                    log = new VLogentriesDTO
                          {
                              Id = (int) dt.Rows[0]["Id"]
                            , Pod = (string) dt.Rows[0]["pod"]
                            , Hostname = (string) dt.Rows[0]["hostname"]
                            , Severity = (int) dt.Rows[0]["severity"]
                            , Location = (string) dt.Rows[0]["location"]
                            , Message = (string) dt.Rows[0]["message"]
                            , Timestamp = (DateTime) dt.Rows[0]["timestamp"]
                          };
                }
            }

            return log;
        }

        public override void Update(VLogentriesDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}