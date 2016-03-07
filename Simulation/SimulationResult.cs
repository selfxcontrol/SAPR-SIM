using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class SimulationResult
    {
        public Entities.ProcessingState state { get; set; }
        public double simulationTime { get; set; }
    }
}
