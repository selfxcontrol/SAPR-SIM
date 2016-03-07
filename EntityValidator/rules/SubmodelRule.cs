using Entities;
using Entities.impl;
using EntityValidator.exeptions;
using EntityValidator.validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public class SubmodelRule : Rule
    {

        private Submodel submodel;

        public SubmodelRule(ICollection<Entity> entities)
            : base(entities)
        {
            foreach (Entity e in entities)
            {
                if (e is Submodel)
                {
                    submodel = e as Submodel;
                    this.entities = submodel.Entities;
                }
            }
        }

        public override bool validate()
        {
            if (submodel != null)
            {
                SystemValidator validator = new SystemValidator(entities, submodel.getResources());
                return validator.startValidation().Success;
            }
            return true;
        }

        public override ICollection<ValidationError> explain()
        {
            return new List<ValidationError>() 
            { 
                new ValidationError("Подключаемый подпроцесс '" + submodel.ToString() + "' имеет ошибки", submodel) 
            };
        }
    }
}
