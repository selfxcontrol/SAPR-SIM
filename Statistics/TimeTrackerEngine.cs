using Statistics.extractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    public class TimeTrackerEngine
    {

        public static double fullTime { get; set; }

        static ICollection<StatDataHolder> tracker = new List<StatDataHolder>();

        public static void track(long procedureId, string procedureName, long projectId, ICollection<ResourceDataHolder> resources, double startTime, double endTime)
        {
            StatDataHolder th = new StatDataHolder();
            th.procedureId = procedureId;
            th.procedureName = procedureName;
            th.projectId = projectId;
            th.resources = resources;
            th.time = new IntervalHolder(startTime, endTime);

            tracker.Add(th);
        }

        public static ICollection<StatDataHolder> getStatistics()
        {
            return tracker;
        }

        public static void clear()
        {
            tracker.Clear();
            fullTime = 0;
        }

    }
}
