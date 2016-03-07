using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Timer
    {
        private const double STEP = 0.01;

        private double timer = 0.0;

        private static Timer instance = new Timer();

        static Timer() { }
        private Timer() { }

        public static Timer Instance
        {
            get
            {
                return instance;
            }
        }

        public void increment()
        {
            timer += STEP;
        }

        public void resetTime()
        {
            timer = 0;
        }

        public double getTime()
        {
            return timer;
        }

        public double getStep()
        {
            return STEP;
        }

    }
}
