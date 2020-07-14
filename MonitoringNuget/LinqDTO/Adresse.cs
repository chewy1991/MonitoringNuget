using System;
using System.Data.Linq.Mapping;

namespace MonitoringNuget.LinqDTO
{
    [Table(Name = "Testat.Adresse")]
    public class Adresse
    {
        [Column(Name = "Id", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column(Name = "Land", CanBeNull = false, DbType = "nvarchar(45)")]
        public string Land { get; set; }
        [Column(Name = "Ort", CanBeNull = false, DbType = "nvarchar(45)")]
        public string Ort { get; set; }
        [Column(Name = "Plz", CanBeNull = false, DbType = "nvarchar(10)")]
        public string PLZ { get; set; }
        [Column(Name = "Strasse", CanBeNull = false, DbType = "nvarchar(255)")]
        public string Strasse { get; set; }
        [Column(Name = "Hausnr", CanBeNull = false, DbType = "nvarchar(10)")]
        public string Hausnr { get; set; }
    }
}
