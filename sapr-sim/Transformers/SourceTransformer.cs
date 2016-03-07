using Entities.impl;
using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator.Transformers
{
    public class SourceTransformer : Transformer
    {

        public Entities.Entity transform(UIEntity entity)
        {
            Source s = entity as Source;
            return new EntityStart() { projectsCount = s.ProjectsCount, Complexity = s.Complexity, name = s.EntityName, id = s.Id };
        }
    }
}
