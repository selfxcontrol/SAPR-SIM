using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Beans
{
    public class ProjectBean : AbstractStatiscticsBean
    {
        public double value { get; set; }

        public ProjectBean(long id, string name, double value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
        }
    }
}
