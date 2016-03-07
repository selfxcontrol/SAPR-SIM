using Entities;
using EntityTransformator.Transformers;
using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator
{
    public class TransformerFactory
    {

        private TransformerFactory() { }

        // c# generics are shit... where is automatically cast to object???
        public static Transformer getTransformer(Type type)
        {
            Transformer transformer = null;

            if (type == typeof(Collector))
                transformer = new CollectorTransformer();
            else if (type == typeof(Decision))
                transformer = new DecisionTransformer();
            else if (type == typeof(Destination))
                transformer = new DestinationTransformer();
            else if (type == typeof(sapr_sim.Figures.Parallel))
                transformer = new ParallelTransformer();
            else if (type == typeof(Procedure))
                transformer = new ProcedureTransformer();
            else if (type == typeof(Source))
                transformer = new SourceTransformer();
            else if (type == typeof(Sync))
                transformer = new SynchronizationTransformer();
            else if (type == typeof(SubDiagram))
                transformer = new SubdiagramTransformer();

            return transformer;
        }

    }
}
