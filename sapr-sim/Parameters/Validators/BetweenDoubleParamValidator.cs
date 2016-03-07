using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Parameters.Validators
{
    [Serializable]
    public class BetweenDoubleParamValidator : ParamValidator
    {

        private readonly double minValue, maxValue;

        public BetweenDoubleParamValidator(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public bool validate(string value)
        {
            double result;
            Double.TryParse(value, out result);
            return result > minValue && result <= maxValue;
        }

    }
}
