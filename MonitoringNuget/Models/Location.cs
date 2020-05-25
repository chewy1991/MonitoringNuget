using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.Models.ModelInterface;

namespace MonitoringNuget.Models
{
    class Location:IModel
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? Locationparent { get; set; }
    }
}
