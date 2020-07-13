using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;

namespace MonitoringNuget.Strategy
{
    public class ContextStrategy<T>
    {
        private IRepositoryBase<T> strategy;
        private IStoredProcedures storedprocstrategy;

        public ContextStrategy(IRepositoryBase<T> strategy, IStoredProcedures storedprocstrategy = null)
        {
            this.strategy = strategy;
            this.storedprocstrategy = storedprocstrategy;
        }

        /// <summary>
        /// Liefert ein einzelnes Model-Objekt vom Typ M zurück,
        /// welches anhand dem übergebenen PrimaryKey geladen wird.
        /// </summary>
        /// <typeparam name="P">Type des PrimaryKey</typeparam>
        /// <param name="pkValue">Wert des PrimaryKey</param>
        /// <returns>gefundenes Model-Objekt, ansonsten null</returns>
        public T GetSingle<P>(P pkValue)
        {
            return this.strategy.GetSingle(pkValue);
        }

        /// <summary>
        /// Fügt das Model-Objekt zur Datenbank hinzu (Insert)
        /// </summary>
        /// <param name="entity">zu speicherndes Model-Object</param>
        public void Add(T entity)
        {
            this.strategy.Add(entity);
        }

        /// <summary>
        /// Löscht das Model-Objekt aus der Datenbank (Delete)
        /// </summary>
        /// <param name="entity">zu löschendes Model-Object</param>
        public void Delete(T entity)
        {
            this.strategy.Delete(entity);
        }

        /// <summary>
        /// Aktualisiert das Model-Objekt in der Datenbank hinzu (Update)
        /// </summary>
        /// <param name="entity">zu aktualisierendes Model-Object</param>
        public void Update(T entity)
        {
            this.strategy.Update(entity);
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
        public List<T> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            return this.strategy.GetAll(whereCondition, parameterValues);
        }

        /// <summary>
        /// Gibt eine Liste aller in der DB vorhandenen Model-Objekte vom Typ M zurück
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            return this.strategy.GetAll();
        }

        /// <summary>
        /// Zählt in der Datenbank die Anzahl Model-Objekte vom Typ M, die der
        /// Where-Bedingung entsprechen
        /// </summary>
        /// <param name="whereCondition">WhereBedingung als string
        /// z.B. "NetPrice > @netPrice and Active = @active and Description like @desc</param>
        /// <param name="parameterValues">Parameter-Werte für die Wherebedingung
        /// bspw: {{"netPrice", 10.5}, {"active", true}, {"desc", "Wolle%"}}</param>
        /// <returns></returns>
        public long Count(string whereCondition, Dictionary<string, object> parameterValues)
        {
            return this.Count(whereCondition, parameterValues);
        }

        /// <summary>
        /// Zählt alle Model-Objekte vom Typ M
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return this.strategy.Count();
        }

        public bool LogClear(int logId)
        {
            if (this.storedprocstrategy != null)
                return storedprocstrategy.LogClear(logId);
            else
                return false;
        }

        public bool AddMessage(string message, string podname, int severity, string hostname)
        {
            if (this.storedprocstrategy != null)
                return this.storedprocstrategy.AddMessage(message, podname, severity, hostname);
            else
                return false;
        }

        /// <summary>
        /// Gibt den Tabellennamen zurück, auf die sich das Repository bezieht
        /// </summary>
        public string TableName
        {
            get { return this.strategy.TableName; }
        }
    }
}
