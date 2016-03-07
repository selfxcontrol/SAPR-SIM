using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Parameters.Validators
{
    [Serializable]
    public class PositiveDoubleParamValidator : ParamValidator
    {
        public bool validate(string value)
        {
            double result;
            Double.TryParse(value, out result);
            return result > 0.0 && result <= Double.MaxValue;
        }
    }
}
