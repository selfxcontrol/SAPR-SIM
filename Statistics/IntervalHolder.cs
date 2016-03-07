using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    public class IntervalHolder
    {
        private double start;
        private double end;

        public IntervalHolder(double start, double end)
        {
            this.start = start;
            this.end = end;
        }

        public double getFrom()
        {
            return start;
        }

        public void setFrom(double from)
        {
            this.start = from;
        }

        public double getTo()
        {
            return end;
        }

        public void setTo(double to)
        {
            this.end = to;
        }

    }
}
