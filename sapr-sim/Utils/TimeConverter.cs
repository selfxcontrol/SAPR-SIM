using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sapr_sim.Parameters;

namespace sapr_sim.Utils
{
    public class TimeWithMeasure 
    {
        public double doubleValue { get; set;}
        public TimeMeasure measure { get; set;}

        public TimeWithMeasure(double doubleValue, TimeMeasure measure)
        {
            this.doubleValue = doubleValue;
            this.measure = measure;
        }
    }

    public class TimeConverter
    {
        public static double fromHumanToModel(TimeParam time)
        {
            double result = 0;
            switch (time.Measure.Order)
            {
                case 0:
                    result = time.Time;
                    break;
                case 1:
                    result = time.Time * 60;
                    break;
                case 2:
                    result = time.Time * 3600;
                    break;
                case 3:
                    result = time.Time * 86400;
                    break;
                default:
                    throw new Exception("Wrong time measure");
            }
            return result;
        }

        public static double fromHumanToModel(TimeWithMeasure time)
        {
            double result = 0;
            switch (time.measure.Order)
            {
                case 0:
                    result = time.doubleValue;
                    break;
                case 1:
                    result = time.doubleValue * 60;
                    break;
                case 2:
                    result = time.doubleValue * 3600;
                    break;
                case 3:
                    result = time.doubleValue * 86400;
                    break;
                default:
                    throw new Exception("Wrong time measure");
            }
            return result;
        }

        public static TimeWithMeasure fromModelToHuman(double modelTime)
        {
            // "modelTime / 86400" in return statement is for beautifull output. Further the divider will decrease   
            if ((modelTime / 86400) >= 1)
                return new TimeWithMeasure(modelTime / 86400, TimeMeasure.DAY); 

            if ((modelTime / 3600) >= 1)
                return new TimeWithMeasure(modelTime / 3600, TimeMeasure.HOUR);

            if ((modelTime / 60) >= 1)
                return new TimeWithMeasure(modelTime / 60, TimeMeasure.MINUTE);

            return new TimeWithMeasure(modelTime , TimeMeasure.SECOND);
        }
    }
}
