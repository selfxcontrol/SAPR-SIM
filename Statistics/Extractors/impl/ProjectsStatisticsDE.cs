using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.extractors
{
    public class ProjectsStatisticsDE : StatisticsDataExtractor<ProjectBean>
    {
        public ICollection<ProjectBean> extract()
        {
            ICollection<StatDataHolder> tracker = TimeTrackerEngine.getStatistics();

            Dictionary<long, IntervalHolder[]> groupedByProjectId = (Dictionary<long, IntervalHolder[]>) tracker.GroupBy(item => item.projectId)
                                                                      .ToDictionary(
                                                                             item => item.Key, 
                                                                             item => item.Select(i => i.time).ToArray()
                                                                             );
            ICollection<ProjectBean> result = new List<ProjectBean>();

            foreach (long key in groupedByProjectId.Keys)
            {
                IntervalHolder[] ih =  groupedByProjectId[key];

                double totalTime = 0;

                foreach (IntervalHolder h in ih)
                {
                    totalTime += h.getTo() - h.getFrom();
                }

                result.Add(new ProjectBean(key, key.ToString(), totalTime));
            }

            return result;
        }
    }
}
