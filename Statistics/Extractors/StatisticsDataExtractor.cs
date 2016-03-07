using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    interface StatisticsDataExtractor <T> where T: AbstractStatiscticsBean
    {
        ICollection<T> extract();
    }
}
