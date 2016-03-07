using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Parameters.Validators
{
    [Serializable]
    public class PositiveIntegerParamValidator : ParamValidator
    {

        public bool validate(string value)
        {
            int result;
            Int32.TryParse(value, out result);            
            return result > 0 && result <= Int32.MaxValue;
        }

    }
}
