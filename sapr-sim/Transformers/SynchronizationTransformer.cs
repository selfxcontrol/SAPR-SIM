using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator.Transformers
{
    public class SynchronizationTransformer : Transformer
    {

        public Entities.Entity transform(sapr_sim.Figures.UIEntity entity)
        {
            return new Entities.impl.Synchronization() { name = entity.EntityName, id = entity.Id };
        }
    }
}
