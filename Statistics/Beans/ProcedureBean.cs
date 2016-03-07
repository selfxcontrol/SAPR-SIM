using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Beans
{
    public class ProcedureBean : AbstractStatiscticsBean
    {
        public int count { get; set; }
        public double min { get; set; }
        public double avg { get; set; }
        public double max { get; set; }


        public ProcedureBean(long id, string name, int count, double min, double avg, double max)
        {
            this.id = id;
            this.name = name;
            this.count = count;
            this.min = min;
            this.avg = avg;
            this.max = max;
        }
    }
}
