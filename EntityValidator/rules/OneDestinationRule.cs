using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.impl;
using EntityValidator.exeptions;

namespace EntityValidator.rules
{
    class OneDestinationRule : Rule
    {
        private List<Entity> destinations = new List<Entity>();

        public OneDestinationRule(ICollection<Entity> entities)
            : base(entities)
        {
        }

        public override bool validate()
        {
            if (entities == null || entities.Count == 0)
                return false;

            foreach (Entity entity in entities)
            {
                if (entity is EntityDestination)
                    destinations.Add(entity);
            }

            return destinations.Count == 1;                
        }

        public override ICollection<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            if (destinations.Count == 0)
            {
                errors.Add(new ValidationError("Блок 'Конец' не найден"));
            }
            else if (destinations.Count > 1)
            {
                foreach(Entity dest in destinations)
                {
                    errors.Add(new ValidationError("Лишний блок конец '" + dest.name + "'", dest));
                }
            }
            return errors;
        }
    }
}
