using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.LinqDTO;

namespace MonitoringNuget.LinqDBContext
{
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "Testat")]
    public partial class LinqDBContext : DataContext
    {
        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion

        public LinqDBContext() : base(global::MonitoringNuget.Properties.Settings.Default.TestatConnectionString, mappingSource)
        {
            OnCreated();
        }

        public LinqDBContext(string connection) : base(connection, mappingSource)
        {
            OnCreated();
        }

        public LinqDBContext(System.Data.IDbConnection connection) : base(connection, mappingSource)
        {
            OnCreated();
        }

        public LinqDBContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : base(connection, mappingSource)
        {
            OnCreated();
        }

        public LinqDBContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<VLogentriesDTO> v_logentries
        {
            get { return this.GetTable<VLogentriesDTO>(); }
        }

        public System.Data.Linq.Table<LocationDTO> locations
        {
            get { return this.GetTable<LocationDTO>(); }
        }
    }
}
