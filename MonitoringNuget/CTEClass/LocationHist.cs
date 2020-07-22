using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringNuget.EntityFramework;

namespace MonitoringNuget.CTEClass
{
    public class LocationHist
    {
        public LocationHist()
        {
            LocationChilds = new ObservableCollection<LocationHist>();
        }

        public ObservableCollection<LocationHist> LocationChilds { get; set; }
        public string Name { get; set; }
        public string PodName { get; set; }
        public int? Id { get; set; }

        public void Add(List<LoadHierarchy_Result> query)
        {
            var lst = query;
            var nodes = lst.Where((x) => x.Locationparent == Id);
            foreach (var child in nodes)
            {
                var childloc = new LocationHist()
                               {
                                   Name = child.Locationname
                                 , Id = child.Id
                               };
                
                childloc.Add(query);
                LocationChilds.Add(childloc);
            }
        }
    }
}
