using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.DataAccess.AbstractClassLinq;
using MonitoringNuget.LinqDTO;

namespace MonitoringNuget.DataAccess.LinqToSQL
{
    public class LogentriesRepositoryLinq : RepositoryBaseLinq<VLogentriesDTO>
    {
        public override long Count(Func<VLogentriesDTO, bool> whereCondition, Dictionary<string, object> parameterValues)
        {
            long count = 0;
            using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
            {
                count = DbContext.v_logentries.Where(whereCondition).Count();
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
                count = ( from v in DbContext.v_logentries select v ).Count();
            }

            return count;
        }

        /// <summary>Löscht das Model-Objekt aus der Datenbank (Delete)</summary>
        /// <param name="entity">zu löschendes Model-Object</param>
        public override void Delete(VLogentriesDTO entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>Aktualisiert das Model-Objekt in der Datenbank hinzu (Update)</summary>
        /// <param name="entity">zu aktualisierendes Model-Object</param>
        public override void Update(VLogentriesDTO entity)
        {
            throw new NotImplementedException();
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
        public override IQueryable<VLogentriesDTO> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
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
        public override IQueryable<VLogentriesDTO> GetAll(Func<VLogentriesDTO, bool> whereCondition, Dictionary<string, object> parameterValues)
        {
            var DbContext = new LinqDBContext.LinqDBContext(ConnectionString);
            var query = DbContext.v_logentries.Where(whereCondition).AsQueryable();
            return query;
        }

        /// <summary>Gibt eine Liste aller in der DB vorhandenen Model-Objekte vom Typ M zurück</summary>
        /// <returns></returns>
        public override IQueryable<VLogentriesDTO> GetAll()
        {
            var DbContext = new LinqDBContext.LinqDBContext(ConnectionString);
            IQueryable<VLogentriesDTO> query = from v in DbContext.v_logentries select v;

            return query;
        }

        /// <summary>
        /// Liefert ein einzelnes Model-Objekt vom Typ M zurück,
        /// welches anhand dem übergebenen PrimaryKey geladen wird.
        /// </summary>
        /// <typeparam name="P">Type des PrimaryKey</typeparam>
        /// <param name="pkValue">Wert des PrimaryKey</param>
        /// <returns>gefundenes Model-Objekt, ansonsten null</returns>
        public override VLogentriesDTO GetSingle<P>(P pkValue)
        {
            VLogentriesDTO logentry = null;
            var bOK = int.TryParse(Convert.ToString(pkValue), out int Id);

            if (bOK)
            {
                using (var DbContext = new LinqDBContext.LinqDBContext(ConnectionString))
                {
                    logentry = DbContext.v_logentries.Where((x) => x.Id == Id).Single();
                }
            }

            return logentry;
        }

        /// <summary>Gibt den Tabellennamen zurück, auf die sich das Repository bezieht</summary>
        public override string TableName { get; }

        /// <summary>Fügt das Model-Objekt zur Datenbank hinzu (Insert)</summary>
        /// <param name="entity">zu speicherndes Model-Object</param>
        public override void Add(VLogentriesDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
