using System;
using System.Data.Linq.Mapping;
using MonitoringNuget.Models.ModelInterface;

namespace MonitoringNuget.LinqDTO
{
    [Table(Name = "dbo.v_logentries")]
    public class VLogentriesDTO : IModel
    {
        [Column(Name = "Id", DbType = "Int NOT NULL")]
        public int Id { get; set; }
        [Column(Name = "pod",DbType = "NVarChar(45) NOT NULL", CanBeNull = false)]
        public string Pod { get; set; }
        [Column(Name = "location",DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Location { get; set; }
        [Column(Name = "hostname", DbType = "NVarChar(45) NOT NULL", CanBeNull = false)]
        public string Hostname { get; set; }
        [Column(Name = "severity", DbType = "Int NOT NULL", CanBeNull = false)]
        public int Severity { get; set; }
        [Column(Name = "timestamp", DbType = "DateTime NOT NULL", IsDbGenerated = true)]
        public DateTime Timestamp { get; set; }
        [Column(Name = "message", DbType = "NVarChar(45) NOT NULL", CanBeNull = false)]
        public string Message { get; set; }
    }
}
