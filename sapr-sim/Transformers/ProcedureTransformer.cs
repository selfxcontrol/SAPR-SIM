using sapr_sim.Figures;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator.Transformers
{
    public class ProcedureTransformer : Transformer
    {

        public Entities.Entity transform(sapr_sim.Figures.UIEntity entity)
        {
            Procedure p = entity as Procedure;
           
            return new Entities.impl.Procedure() { 
                manHour = TimeConverter.fromHumanToModel(new TimeWithMeasure(p.Time, p.TimeMeasure)), 
                name = p.EntityName,
                id = p.Id 
            };
        }
    }
}
