using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.validator
{
    public interface IValidator
    {
        ValidationResult startValidation();
    }
}
