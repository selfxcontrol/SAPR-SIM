using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Synchronization : Entity
    {
        public override void execute()
        {
            // getting duplicate values 
            List<Project> projects = getReadyProjectQueue().GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            if (projects != null && projects.Count != 0)
            {
                Project proj = projects[0];
                Entity outputEntity = getOutputs().First();
                outputEntity.addProjectToQueue(proj);

                getReadyProjectQueue().Remove(proj);
            }
        }

        public override bool canUseAsInput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityStart || entity is Submodel;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityDestination || entity is Submodel;
        }

        public override bool correctInputCount()
        {
            return input.Count == 2;
        }

        public override bool correctOutputCount()
        {
            return output.Count == 1;
        }
    }
}
