using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace sapr_sim.Utils
{
    public class ConnectorFinder
    {

        public static List<ConnectionLine> find(UIElementCollection entities, UIEntity entity)
        {
            List<ConnectionLine> result = new List<ConnectionLine>();
            foreach (UIEntity e in entities)
            {
                if (e is ConnectionLine)
                {
                    ConnectionLine conenctor = e as ConnectionLine;
                    BindingExpression srcExp = conenctor.GetBindingExpression(ConnectionLine.SourceProperty);
                    BindingExpression dstExp = conenctor.GetBindingExpression(ConnectionLine.DestinationProperty);

                    if (srcExp != null && dstExp != null)
                    {
                        Port src = srcExp.DataItem as Port;
                        Port dst = dstExp.DataItem as Port;

                        //if (entity.Equals(src.Owner) || entity.Equals(dst.Owner))
                        if (entity.Equals(entity, src.Owner) || entity.Equals(entity, dst.Owner))                        
                            result.Add(conenctor);
                    }
                }
            }

            return result;
        }

    }
}
