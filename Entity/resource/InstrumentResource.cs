using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InstrumentResource : Resource
    {

        public double power { get; set; }
        public double price { get; set; }

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
                foreach (MaterialResource res in materials)
                    total += res.totalCount;
                return total;
            }
        }
    }
}
