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
    
    public partial class Device
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Device()
        {
            this.Logmessage = new HashSet<Logmessage>();
            this.Ports = new HashSet<Ports>();
            this.Referenzen = new HashSet<Referenzen>();
        }
    
        public int Id { get; set; }
        public int Devicecategory { get; set; }
        public string Hostname { get; set; }
        public string Ip_Adresse { get; set; }
        public int AnzahlPorts { get; set; }
        public int AdressId { get; set; }
        public int NetworkinterfaceId { get; set; }
    
        public virtual Adresse Adresse { get; set; }
        public virtual DeviceCategory DeviceCategory1 { get; set; }
        public virtual Networkinterface Networkinterface { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Logmessage> Logmessage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ports> Ports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Referenzen> Referenzen { get; set; }
    }
}