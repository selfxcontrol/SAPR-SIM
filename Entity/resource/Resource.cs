using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public abstract class Resource : Identifable
    {

        public ResourceType type { get; set; }
        
        public double count { get; set; }
        public bool isShared { get; set; }
        public bool isBusy { get; set; }
        
        public HashSet<long> users = new HashSet<long>();

        public abstract double totalCount { get; }
    }
}
