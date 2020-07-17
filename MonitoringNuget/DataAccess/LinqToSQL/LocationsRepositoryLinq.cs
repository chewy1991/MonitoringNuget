using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MonitoringNuget.DataAccess.AbstractClassLinq;
using MonitoringNuget.LinqDTO;

namespace MonitoringNuget.DataAccess.LinqToSQL
{
    public class LocationsRepositoryLinq : RepositoryBaseLinq<LocationDTO>
    {
        /// <summary>
        /// Zählt in der Datenbank die Anzahl Model-Objekte vom Typ M, die der
        /// Where-Bedingung entsprechen
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public override long Count(Func<LocationDTO, bool> whereCondition, Dictionary<string, object> parameterValues)
        {
            long count = 0;

            using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
            {
                count = DbContext.locations.Where(whereCondition).Count();
            }

            return count;
        }

        /// <summary>Zählt alle Model-Objekte vom Typ M</summary>
        /// <returns></returns>
        public override long Count()
        {
            long count = 0;

            using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
            {
                count = DbContext.locations.Count();
            }

            return count;
        }

        /// <summary>Löscht das Model-Objekt aus der Datenbank (Delete)</summary>
        /// <param name="entity">zu löschendes Model-Object</param>
        public override void Delete(LocationDTO entity)
        {
            try
            {
                using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
                {
                    DbContext.locations.DeleteOnSubmit(entity);
                    DbContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>Aktualisiert das Model-Objekt in der Datenbank hinzu (Update)</summary>
        /// <param name="entity">zu aktualisierendes Model-Object</param>
        public override void Update(LocationDTO entity)
        {
            try
            {
                using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
                {
                    var loc = DbContext.locations.Where((x) => x.Id == entity.Id).Single();
                    loc.LocationName = entity.LocationName;
                    loc.Locationparent = entity.Locationparent;

                    DbContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
        /// z.B. "NetPrice &gt; @netPrice and Active = @active and Description like @desc</param>
        /// <param name="parameterValues">Parameter-Werte für die Wherebedingung
        /// bspw: {{"netPrice", 10.5}, {"active", true}, {"desc", "Wolle%"}}</param>
        /// <returns></returns>
        public override IQueryable<LocationDTO> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gibt eine Liste von Model-Objekten vom Typ M zurück,
        /// die gemäss der WhereBedingung geladen wurden. Die Werte der
        /// Where-Bedingung können als separat übergeben werden,
        /// damit diese für PreparedStatements verwendet werden können.
        /// (Verhinderung von SQL-Injection)        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public override IQueryable<LocationDTO> GetAll(Func<LocationDTO, bool> whereCondition, Dictionary<string, object> parameterValues)
        {
            var DbContext = new LinqDBContext.LinqDBContext(ConnectionString);
            var Query = DbContext.locations.Where(whereCondition).AsQueryable();
            return Query;
        }

        /// <summary>Gibt eine Liste aller in der DB vorhandenen Model-Objekte vom Typ M zurück</summary>
        /// <returns></returns>
        public override IQueryable<LocationDTO> GetAll()
        {
            var DbContext = new LinqDBContext.LinqDBContext(ConnectionString);
            var Query = from l in DbContext.locations select l;
            return Query;
        }

        /// <summary>
        /// Liefert ein einzelnes Model-Objekt vom Typ M zurück,
        /// welches anhand dem übergebenen PrimaryKey geladen wird.
        /// </summary>
        /// <typeparam name="P">Type des PrimaryKey</typeparam>
        /// <param name="pkValue">Wert des PrimaryKey</param>
        /// <returns>gefundenes Model-Objekt, ansonsten null</returns>
        public override LocationDTO GetSingle<P>(P pkValue)
        {
            LocationDTO location = null;
            var bOK = int.TryParse(Convert.ToString(pkValue), out int Id);
            if (bOK)
            {
                using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
                {
                    location = DbContext.locations.Where((x) => x.Id == Id).Single();
                }
            }

            return location;
        }

        /// <summary>Gibt den Tabellennamen zurück, auf die sich das Repository bezieht</summary>
        public override string TableName { get; }

        /// <summary>Fügt das Model-Objekt zur Datenbank hinzu (Insert)</summary>
        /// <param name="entity">zu speicherndes Model-Object</param>
        public override void Add(LocationDTO entity)
        {
            try
            {
                using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
                {
                    DbContext.locations.InsertOnSubmit(entity);
                    DbContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
