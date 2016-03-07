using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    public class StatDataHolder
    {
        public string procedureName { get; set; }
        public long procedureId {get; set;}
        public ICollection<ResourceDataHolder> resources { get; set; }
        public IntervalHolder time { get; set; }
        public long projectId { get; set; }

    }
}
