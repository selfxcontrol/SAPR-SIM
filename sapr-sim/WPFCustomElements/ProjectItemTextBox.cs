using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim.WPFCustomElements
{
    public class ProjectItemTextBox : TextBox
    {
        public override string ToString()
        {
            return Text;
        }
    }
}
