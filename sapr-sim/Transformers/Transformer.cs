using Entities;
using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTransformator
{

    public interface Transformer
    {
        Entity transform(UIEntity entity);
    }

}
