using Entities;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public abstract class Rule
    {

        protected ICollection<Entity> entities;

        public Rule(ICollection<Entity> entities)
        {
            this.entities = entities;
        }

        public abstract bool validate();
        public abstract ICollection<ValidationError> explain();
    }
}
