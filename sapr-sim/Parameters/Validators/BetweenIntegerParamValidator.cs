using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Parameters.Validators
{
    [Serializable]
    public class BetweenIntegerParamValidator : ParamValidator
    {

        private readonly int minValue, maxValue;

        public BetweenIntegerParamValidator(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public bool validate(string value)
        {
            int result;
            Int32.TryParse(value, out result);
            return result >= minValue && result <= maxValue;
        }

    }
}
