using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator
{
    public class ValidationResult
    {
        private List<ValidationError> errors = new List<ValidationError>();

        public bool Success
        {
            get { return errors.Count == 0; } 
        }

        public List<ValidationError> Errors
        {
            get { return errors; }
        }

        public void addError(ValidationError err)
        {
            errors.Add(err);
        }

        public void addErrors(ICollection<ValidationError> err)
        {
            errors.AddRange(err);
        }
    }
}
