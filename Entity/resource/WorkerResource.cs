using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WorkerResource : Resource
    {

        public double efficiency { get; set; }

        public double price { get; set; }

        private IList<InstrumentResource> _instruments;

        public virtual IList<InstrumentResource> instruments
        {
            get { return _instruments ?? (_instruments = new List<InstrumentResource>()); }
            protected set { _instruments = value; }
        }


        private IList<MaterialResource> _materials;

        public virtual IList<MaterialResource> materials
        {
            get { return _materials ?? (_materials = new List<MaterialResource>()); }
            protected set { _materials = value; }
        }


        public override double totalCount
        {
            get 
            {
                double total = count;
                foreach (InstrumentResource res in instruments)
                    total += res.totalCount;
                foreach (MaterialResource res in materials)
                    total += res.totalCount;
                return total;
            }
        }
    }
}
