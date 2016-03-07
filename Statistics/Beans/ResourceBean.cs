using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Beans
{
    public class ResourceBean : AbstractStatiscticsBean
    {
        public string type { get; set; }
        public double time { get; set; }
        public double percentage { get; set; }

        public ResourceBean(long id, string name, string type, double time, double percentage)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.time = time;
            this.percentage = percentage;
        }
    }
}
