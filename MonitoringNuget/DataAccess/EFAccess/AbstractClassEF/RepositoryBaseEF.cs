using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using MonitoringNuget.EntityFramework;

namespace MonitoringNuget.DataAccess.EFAccess.AbstractClassEF
{
    public abstract class RepositoryBaseEF<T> : IRepositoryBase<T>
    {
        /// <summary>
        /// Liefert ein einzelnes Model-Objekt vom Typ M zurück,
        /// welches anhand dem übergebenen PrimaryKey geladen wird.
        /// </summary>
        /// <typeparam name="P">Type des PrimaryKey</typeparam>
        /// <param name="pkValue">Wert des PrimaryKey</param>
        /// <returns>gefundenes Model-Objekt, ansonsten null</returns>
        public abstract T GetSingle<P>(P pkValue);


        /// <summary>
        /// Fügt das Model-Objekt zur Datenbank hinzu (Insert)
        /// </summary>
        /// <param name="entity">zu speicherndes Model-Object</param>
        public abstract void Add(T entity);

        /// <summary>
        /// Löscht das Model-Objekt aus der Datenbank (Delete)
        /// </summary>
        /// <param name="entity">zu löschendes Model-Object</param>
        public abstract void Delete(T entity);

        /// <summary>
        /// Aktualisiert das Model-Objekt in der Datenbank hinzu (Update)
        /// </summary>
        /// <param name="entity">zu aktualisierendes Model-Object</param>
        public abstract void Update(T entity);

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
        public abstract IQueryable<T> GetAll(string whereCondition, Dictionary<string, object> parameterValues);

        /// <summary>
        /// Gibt eine Liste von Model-Objekten vom Typ M zurück,
        /// die gemäss der WhereBedingung geladen wurden. Die Werte der
        /// Where-Bedingung können als separat übergeben werden,
        /// damit diese für PreparedStatements verwendet werden können.
        /// (Verhinderung von SQL-Injection)        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Func<T, bool> whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gibt eine Liste aller in der DB vorhandenen Model-Objekte vom Typ M zurück
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<T> GetAll();

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zählt in der Datenbank die Anzahl Model-Objekte vom Typ M, die der
        /// Where-Bedingung entsprechen
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public long Count(Func<T, bool> whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zählt alle Model-Objekte vom Typ M
        /// </summary>
        /// <returns></returns>
        public abstract long Count();

        /// <summary>
        /// Gibt den Tabellennamen zurück, auf die sich das Repository bezieht
        /// </summary>
        public string TableName { get; }
    }
}
