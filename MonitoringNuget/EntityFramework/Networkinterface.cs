//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitoringNuget.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Networkinterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Networkinterface()
        {
            this.Device = new HashSet<Device>();
            this.Ports = new HashSet<Ports>();
        }
    
        public int Id { get; set; }
        public string DuplexType { get; set; }
        public string NetworkSpeed { get; set; }
        public int PodId { get; set; }
        public int LocationID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Device> Device { get; set; }
        public virtual Locations Locations { get; set; }
        public virtual Point_Of_Delivery Point_Of_Delivery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ports> Ports { get; set; }
    }
}
