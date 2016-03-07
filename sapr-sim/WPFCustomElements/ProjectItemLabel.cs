using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim.WPFCustomElements
{
    public class ProjectItemLabel : Label
    {
        public override string ToString()
        {
            return Content.ToString();
        }
    }
}
