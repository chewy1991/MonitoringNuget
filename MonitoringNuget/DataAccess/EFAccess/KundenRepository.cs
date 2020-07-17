using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.DataAccess.EFAccess.AbstractClassEF;
using MonitoringNuget.EntityFramework;

namespace MonitoringNuget.DataAccess.EFAccess
{
    public class KundenRepository : RepositoryBaseEF<Kunde>
    {
        /// <summary>
        /// Liefert ein einzelnes Model-Objekt vom Typ M zurück,
        /// welches anhand dem übergebenen PrimaryKey geladen wird.
        /// </summary>
        /// <typeparam name="P">Type des PrimaryKey</typeparam>
        /// <param name="pkValue">Wert des PrimaryKey</param>
        /// <returns>gefundenes Model-Objekt, ansonsten null</returns>
        public override Kunde GetSingle<P>(P pkValue) 
        {
            Kunde client = null;
            bool CanparseInt = int.TryParse(Convert.ToString(pkValue), out int pk);
            if (CanparseInt)
            {
                using (var context = new TestatEntities())
                {
                    var kunde = from s in context.Kunde where s.Id == pk select s;

                    client = kunde.FirstOrDefault();
                }
            }

            return client;
        }

        /// <summary>
        /// Fügt das Model-Objekt zur Datenbank hinzu (Insert)
        /// </summary>
        /// <param name="entity">zu speicherndes Model-Object</param>
        public override void Add(Kunde entity)
        {
            using (var context = new TestatEntities())
            {
                context.Kunde.Add(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Löscht das Model-Objekt aus der Datenbank (Delete)
        /// </summary>
        /// <param name="entity">zu löschendes Model-Object</param>
        public override void Delete(Kunde entity)
        {
            using (var context = new TestatEntities())
            {
                context.Kunde.Attach(entity);
                context.Kunde.Remove(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Aktualisiert das Model-Objekt in der Datenbank hinzu (Update)
        /// </summary>
        /// <param name="entity">zu aktualisierendes Model-Object</param>
        public override void Update(Kunde entity)
        {
            using (var context = new TestatEntities())
            {
                context.Kunde.AddOrUpdate(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gibt eine Liste von Model-Objekten vom Typ M zurück,
        /// die gemäss der WhereBedingung geladen wurden. Die Werte der
        /// Where-Bedingung können als separat übergeben werden,
        /// damit diese für PreparedStatements verwendet werden können.
        /// (Verhinderung von SQL-Injection)
        /// </summary>
        /// <param name="whereCondition">WhereBedingung als string
        /// z.B. "NetPrice > @netPrice and Active = @active and Description like @desc</param>
        /// <param name="parameterValues">Parameter-Werte für die Wherebedingung
        /// bspw: {{"netPrice", 10.5}, {"active", true}, {"desc", "Wolle%"}}</param>
        /// <returns></returns>
        public override IQueryable<Kunde> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            List<Kunde> filteredKundenliste = null;
            if (parameterValues.ContainsKey("Bezeichnung"))
            {
                using (var context = new TestatEntities())
                {
                    parameterValues.TryGetValue("Bezeichnung", out object value);
                    var query = from k in context.Kunde 
                                where k.Bezeichnung.Equals((string)value) 
                                select k;
                    filteredKundenliste = query.ToList();
                }
            }
            return filteredKundenliste.AsQueryable();
        }

        /// <summary>
        /// Gibt eine Liste aller in der DB vorhandenen Model-Objekte vom Typ M zurück
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Kunde> GetAll()
        {
            List<Kunde> kundenliste = null;

            using (var context = new TestatEntities())
            {
                var query = from k in context.Kunde select k;
                kundenliste = query.ToList();
            }

            return kundenliste.AsQueryable();
        }

        /// <summary>
        /// Zählt alle Model-Objekte vom Typ M
        /// </summary>
        /// <returns></returns>
        public override long Count()
        {
            long count = 0;

            using (var context = new TestatEntities())
            {
                count = context.Kunde.Count();
            }

            return count;
        }
    }
}
