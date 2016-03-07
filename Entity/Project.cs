using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum State
    {
        IN_PROGRESS,
        DONE
    };

    public class Project : Identifable
    {
        public Project()
        {
            state = State.IN_PROGRESS;
        }
        public State state { get; set; }
        public double startTime { get; set; }
        public double overallTime { get; set; }
        public int complexity { get; set; }
        public double performance { get; set;}

    }
}
