using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.WPFCustomElements
{
    public class ListBoxItemError
    {
        private string message;
        private List<UIEntity> entities;

        public ListBoxItemError(string message, List<UIEntity> entities)
        {
            this.message = message;
            this.entities = entities;
        }

        public override string ToString()
        {
            return message;
        }

        public List<UIEntity> FailedEntities
        {
            get { return entities; }
        }
    }
}
