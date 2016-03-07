using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityValidator.rules;
using Entities;
using Entities.impl;
using System.Collections.ObjectModel;

namespace EntityValidator.validator
{
    public class SystemValidator : IValidator
    {
        private ICollection<Rule> rules = new Collection<Rule>();

        public SystemValidator(ICollection<Entity> entities, ICollection<Resource> resources) //add here new rules for include them into validation
        {
            rules.Add(new OneStartRule(entities));
            rules.Add(new OneDestinationRule(entities));
            rules.Add(new InputAndOutputNotEmptyRule(entities));
            rules.Add(new EntityHasCorrectInputsAndOutputsRule(entities));
            rules.Add(new ResourceConnectedCorrectRule(entities, resources));
            rules.Add(new SubmodelRule(entities));
        }     

        public ValidationResult startValidation()
        {
            ValidationResult result = new ValidationResult();

            foreach (Rule rule in rules)
            {
                if (!rule.validate())
                {
                    result.addErrors(rule.explain());
                }
            }

            return result;
        }

    }
}
