using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class MaterialResource : Resource
    {
        public int perTick { get; set; }

        public override double totalCount
        {
            get { return count; }
        }
    }
}
