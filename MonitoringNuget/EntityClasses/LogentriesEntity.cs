using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DuplicateCheckerLib;

namespace MonitoringNuget.EntityClasses
{
    public class LogentriesEntity: IEntity
    {
        private int _id;
        private int _severity;
        private string _logmessage;

        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public int Severity
        {
            get { return this._severity; }
            set { this._severity = value; }
        }

        public string Logmessage
        {
            get { return this._logmessage; }
            set { this._logmessage = value; }
        }

        public override bool Equals(object value)
        {
            return Equals(value as LogentriesEntity);
        }

        public bool Equals(LogentriesEntity logentry)
        {
            if (object.ReferenceEquals(null, logentry)) return false;
            if (object.ReferenceEquals(this, logentry)) return true;

            return string.Equals(_logmessage, logentry.Logmessage)
                && _severity == logentry.Severity;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int) 2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = ( hash * HashingMultiplier )
                     ^ ( !Object.ReferenceEquals(null, this._severity) ? this._severity.GetHashCode() : 0 );
                hash = ( hash * HashingMultiplier )
                     ^ ( !Object.ReferenceEquals(null, this._logmessage) ? this._logmessage.GetHashCode() : 0 );

                return hash;
            }
        }

        public static bool operator ==(LogentriesEntity loga, LogentriesEntity logb)
        {
            if (Object.ReferenceEquals(loga, logb)) { return true; }
            if (Object.ReferenceEquals(null, loga)) { return false; }

            return ( loga.Equals(logb) );
        }

        public static bool operator !=(LogentriesEntity loga, LogentriesEntity logb)
        {
            return !( loga == logb );
        }
    }
}
