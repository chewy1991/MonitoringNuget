using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MonitoringNuget.Models.ModelInterface;

namespace MonitoringNuget.LinqDTO
{
    [Table(Name = "dbo.Locations")]
    public class LocationDTO : IModel
    {
        private int? _Locationparent;
        private EntitySet<LocationDTO> _Locations;
        private EntityRef<LocationDTO> _Location1;

        [Column(Name = "Id", IsPrimaryKey = true, CanBeNull = false, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "LocationName", CanBeNull = false, DbType = "NVarChar(100) NOT NULL")]
        public string LocationName { get; set; }
        [Column(Name = "Locationparent", CanBeNull = true, DbType = "Int", Storage = "_Locationparent")]
        public int? Locationparent 
        {
            get
            {
                return this._Locationparent;
            }
            set
            {
                this._Locationparent = value;
            }
        }

        [Association(Name = "Location_Location", ThisKey = "Locationparent", OtherKey = "Id", IsForeignKey = true, Storage = "_Locations")]
        public EntitySet<LocationDTO> Locations 
        {
            get
            {
                return this._Locations;
            }
            set
            {
                this._Locations.Assign(value);
            }
        }
        [Association(Name = "Location_Location", Storage = "_Location1", ThisKey = "Locationparent", OtherKey = "Id", IsForeignKey = true)]
        public LocationDTO Location1
        {
            get
            {
                return this._Location1.Entity;
            }
            set
            {
                LocationDTO previousValue = this._Location1.Entity;
                if (( ( previousValue != value ) || ( this._Location1.HasLoadedOrAssignedValue == false ) ))
                {
                    if (( previousValue != null ))
                    {
                        this._Location1.Entity = null;
                        previousValue.Locations.Remove(this);
                    }

                    this._Location1.Entity = value;
                    if (( value != null ))
                    {
                        value.Locations.Add(this);
                        this._Locationparent = value.Id;
                    }
                    else { this._Locationparent = default(Nullable<int>); }
                }
            }
        }
    }
}
