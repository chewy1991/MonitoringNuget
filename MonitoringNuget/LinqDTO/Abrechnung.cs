using System;
using System.Data.Linq.Mapping;

namespace MonitoringNuget.LinqDTO
{
    [Table(Name = "Testat.Abrechnung")]
    public class Abrechnung
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "Id")]
        public int Id { get; set; }
        [Column(Name = "PodAbrechnungId")]
        public int PodAbrechnungId { get; set; }
        [Column(Name = "Bruttopreis")]
        public decimal Bruttopreis { get; set; }
        [Column(Name = "UsedGuthaben", CanBeNull = false, DbType = "decimal(10,2)")]
        public decimal UsedGuthaben { get; set; }
        [Column(Name = "Nettopreis", CanBeNull = false, DbType = "decimal(10,2)")]
        public decimal Nettopreis { get; set; }
        [Column(Name = "CreationTime", IsDbGenerated = true)]
        public DateTime CreationTime { get; set; }
    }
}
