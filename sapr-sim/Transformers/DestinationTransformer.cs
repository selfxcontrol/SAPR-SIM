using Entities.impl;
using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator.Transformers
{
    public class DestinationTransformer : Transformer
    {

        public Entities.Entity transform(UIEntity entity)
        {
            return new EntityDestination() { name = entity.EntityName, id = entity.Id };
        }
    }
}
