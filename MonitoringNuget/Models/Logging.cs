using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.Models.ModelInterface;

namespace MonitoringNuget.Models
{
    public class Logging:IModel
    {
        public int Id { get; set; }
        public string pod { get; set; }
        public string location { get; set; }
        public string hostname { get; set; }
        public int severity { get; set; }
        public DateTime timestamp { get; set; }
        public string message { get; set; }
    }
}
