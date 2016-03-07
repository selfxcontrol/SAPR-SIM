using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class EntityStart : Entity
    {
        public int projectsCount { get; set; }
        [Column("Complexity_EntityStart")]
        public int Complexity { get; set; }

        public override void execute() {
            ICollection<Entity> outputs = getOutputs();
            if (outputs != null && outputs.Count != 0)
            {
                outputs.First().setReadyProjectQueue(getReadyProjectQueue());
                getReadyProjectQueue().Clear();
            }
        }

        public override bool canUseAsInput(Entity entity)
        {
            return false;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityDestination || entity is DecisionMaker || entity is Submodel;
        }

        public override bool correctInputCount()
        {
            return input.Count == 0;
        }

        public override bool correctOutputCount()
        {
            return output.Count == 1;
        }
    }
}
