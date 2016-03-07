using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.extractors
{
    public class ProcedureStatisticsDE : StatisticsDataExtractor<ProcedureBean>
    {
        public ICollection<ProcedureBean> extract()
        {
            ICollection<StatDataHolder> tracker = TimeTrackerEngine.getStatistics();


            var groupedByProcedure = tracker.Select(i => new { Value = i, index = i.procedureId}).
                                                    GroupBy(item => item.index).
                                                    ToDictionary( 
                                                            p => p.Key, 
                                                            p => p.Select(a => a.Value).ToList()
                                                   );

            ICollection<ProcedureBean> result = new List<ProcedureBean>();

            foreach (var item in groupedByProcedure)
            {
                long procedureId = item.Key;
                List<StatDataHolder> dh = item.Value;

                int count = dh.Count();
                double min = dh.Min(i => i.time.getTo() - i.time.getFrom());
                double max = dh.Max(i => i.time.getTo() - i.time.getFrom());
                double avg = dh.Average(i => i.time.getTo() - i.time.getFrom());

                result.Add(new ProcedureBean(procedureId, dh[0].procedureName, count, min, avg, max));

            }

            return result;
        }
    }
}
