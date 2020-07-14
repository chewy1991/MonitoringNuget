using System;
using System.Data.Linq.Mapping;

namespace MonitoringNuget.LinqDTO
{
    [Table(Name = "Testat.Abrechnungsposition")]
    public class Abrechnungsposition
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "Id")]
        public int Id { get; set; }
        [Column(Name = "Produktbeschreibung", DbType = "nvarchar(45)", CanBeNull = false)]
        public string Produktbeschreibung { get; set; }
        [Column(Name = "Stueckpreis", CanBeNull = false, DbType = "decimal(10,2)")]
        public decimal Stueckpreis { get; set; }
        [Column(Name = "Menge", CanBeNull = false)]
        public int Menge { get; set; }
        [Column(Name = "PodId", CanBeNull = false)]
        public int PodId { get; set; }
        [Column(Name = "isFakturiert", CanBeNull = true, DbType = "bit")]
        public bool? isFakturiert { get; set; }
        [Column(Name = "Buchungsdatum", CanBeNull = false, IsDbGenerated = true)]
        public DateTime Buchungsdatum { get; set; }
        [Column(Name = "AbrechnungId", CanBeNull = true)]
        public int? AbrechnungId { get; set; }
    }
}
