using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Extractors.impl
{
    public class ResourceStatisticDE : StatisticsDataExtractor<ResourceBean>
    {

        IDictionary<ResourceDataHolder.ResourceType, string> mapping = new Dictionary<ResourceDataHolder.ResourceType, string> ();

        public ResourceStatisticDE()
        {
            mapping.Add(ResourceDataHolder.ResourceType.INSTRUMENT, "Оборудование");
            mapping.Add(ResourceDataHolder.ResourceType.MATERIAL, "Расходник");
            mapping.Add(ResourceDataHolder.ResourceType.WORKER, "Исполнитель");
        }

        public ICollection<ResourceBean> extract()
        {
            ICollection<StatDataHolder> tracker = TimeTrackerEngine.getStatistics();
            IDictionary<ResourceDataHolder, IList<IntervalHolder>> resources = new Dictionary<ResourceDataHolder, IList<IntervalHolder>>();

            foreach (StatDataHolder statDH in tracker)
            {
                foreach (ResourceDataHolder resDH in statDH.resources)
                {
                    if (resources.ContainsKey(resDH))
                    {
                        resources[resDH].Add(statDH.time);
                    } else {
                        List<IntervalHolder> time = new List<IntervalHolder>();
                        time.Add(statDH.time);
                        resources.Add(resDH, time);
                    } 
                }

            }

            ICollection<ResourceBean> resourcesTimeSum = resources.Select(
                        i =>  new ResourceBean(i.Key.id,  
                                               i.Key.name, 
                                               mapping[i.Key.type], 
                                               i.Value.Sum( j => j.getTo() - j.getFrom() ),
                                               i.Value.Sum(j => j.getTo() - j.getFrom()) / TimeTrackerEngine.fullTime * 100
                                              )  
            ).ToList();

            return resourcesTimeSum;
        }
    }
}
