using Entities.impl;
using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator.Transformers
{
    public class DecisionTransformer : Transformer
    {

        public Entities.Entity transform(UIEntity entity)
        {
            Decision d = entity as Decision;
            return new DecisionMaker() { inputProbabilityParams = d.InputProbabilityParams, name = d.EntityName, id = d.Id };
        }
    }
}
