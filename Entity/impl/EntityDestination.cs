using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class EntityDestination : Entity
    {

        public override void execute() {
            ICollection<Project> projects = getReadyProjectQueue();
            ICollection<Project> projects1 = getNotReadyProjectQueue();
            
            foreach (Project prj in projects)
            {
                prj.overallTime = Timer.Instance.getTime() - prj.startTime;
                prj.state = State.DONE;
            }

            foreach (Project prj in projects1)
            {
                prj.overallTime = Timer.Instance.getTime() - prj.startTime;
                prj.state = State.DONE;
            }
        }

        public override bool canUseAsInput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityStart || entity is DecisionMaker || entity is Submodel;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return false;
        }

        public override bool correctInputCount()
        {
            return input.Count == 1;
        }

        public override bool correctOutputCount()
        {
            return output.Count == 0;
        }
    }
}
