using Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities.impl
{
    public class Procedure : Entity
    {
        public double manHour { get; set; }

        private double needTime;
        private double operationTime;
        IList<Resource> _resources;


        public virtual IList<Resource> resources
        {
            get { return _resources ?? (_resources = new List<Resource>()); }
            protected set { _resources = value; }
        }


        IEnumerable<Resource> busyResBefore = null;

        public override void execute()
        {
            Timer timer = Timer.Instance;

            double overallEfficiency = 0.0000001; //  some inaccuracy 

            Project prj = getProjectFromReadyQueue();

            if (prj != null)
            {
                if (busyResBefore == null)
                {
                    busyResBefore = new List<Resource>(resources.Where(i => i.isBusy == true));
                }

                needTime = getNeedTime(overallEfficiency, prj.complexity);

                operationTime += timer.getStep();

                if (operationTime >= needTime)
                {
                    operationTime = 0;
                    Entity outputEntity = getOutputs().First();
                    outputEntity.addProjectToQueue(prj);
                    getReadyProjectQueue().Remove(prj);

                    ICollection<ResourceDataHolder> resList = getUsedResources();

                    TimeTrackerEngine.track(this.id, this.name, prj.id, resList, timer.getTime() - needTime, timer.getTime()); 

                    foreach (Resource res in getResources())
                    {
                        if (res is WorkerResource)
                        {
                            foreach (InstrumentResource instrument in (res as WorkerResource).instruments)
                            {
                                foreach (MaterialResource material in instrument.materials)
                                {
                                    material.users.Remove(this.id);
                                    material.isBusy = false;
                                }

                                instrument.users.Remove(this.id);
                                instrument.isBusy = false;
                            }
                        }
                        res.users.Remove(this.id);
                        res.isBusy = false;
                    }

                    busyResBefore = null;
                }

            }
        }

        private ICollection<ResourceDataHolder> getUsedResources()
        {
            IEnumerable<Resource> busyResAfter = new List<Resource>( resources.Where(i => i.isBusy == true));
            IEnumerable<Resource> procedureUsedResources = busyResAfter.Except(busyResBefore);

            // Duplication of resources =(( Need to resolve cyclic dependencies 
            IDictionary<ResourceType, ResourceDataHolder.ResourceType> mapping = new Dictionary<ResourceType, ResourceDataHolder.ResourceType>();
            mapping.Add(ResourceType.INSTRUMENT, ResourceDataHolder.ResourceType.INSTRUMENT);
            mapping.Add(ResourceType.MATERIAL, ResourceDataHolder.ResourceType.MATERIAL);
            mapping.Add(ResourceType.WORKER, ResourceDataHolder.ResourceType.WORKER);

            ICollection<ResourceDataHolder> resList = new List<ResourceDataHolder>();
            foreach (Resource res in procedureUsedResources)
            {
                resList.Add(new ResourceDataHolder(res.id, res.name, mapping[res.type]));
            }
            return resList;
        }

        public override bool canUseAsInput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityStart || entity is Submodel;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityDestination || entity is Submodel;
        }

        public void addResource(Resource res)
        {
            resources.Add(res);
        }

        public IList<Resource> getResources()
        {
            return resources;
        }

        public override bool correctInputCount()
        {
            return input.Count == 1;
        }

        public override bool correctOutputCount()
        {
            return output.Count == 1;
        }

        private double getNeedTime(double overallEfficiency, int complexity)
        {
            foreach (Resource res in resources)
            {  
                if (res is WorkerResource)
                {
                    WorkerResource worker = res as WorkerResource;
                    bool isUsedByThis = worker.users.Contains(this.id);

                    if (worker.users.Count == 0)
                        worker.isBusy = false;

                    if (worker.isBusy && !worker.isShared && !isUsedByThis)
                        return Double.MaxValue;

                    if (worker.isBusy && worker.isShared || worker.isBusy && isUsedByThis)
                    {
                        if (!worker.users.Contains(this.id))
                        {
                            worker.users.Add(this.id);
                        }

                        overallEfficiency += processInstrumentalReesource(worker) * worker.efficiency * worker.count / (worker.users.Count > 0 ? worker.users.Count : 1);
                    }

                    if (!worker.isBusy)
                    {
                        overallEfficiency += worker.efficiency * worker.count;
                        if (!worker.users.Contains(this.id))
                        {
                            worker.users.Add(this.id);
                            worker.isBusy = true;
                        }
                    }

                    foreach (MaterialResource material in worker.materials)
                    {
                        processSingleMaterial(material);
                    }
                }

                if (res is MaterialResource)
                {
                    processSingleMaterial(res as MaterialResource);
                }

                if (res is InstrumentResource)
                {
                    overallEfficiency += processSingleInstrument(res as InstrumentResource);
                }

            }               

            return needTime = (1 / overallEfficiency) * manHour * complexity;
        }

        private double processInstrumentalReesource(WorkerResource worker)
        {
            double result = 1.0;
            foreach (InstrumentResource instrument in worker.instruments)
            {
                result += processSingleInstrument(instrument);
            }
            return result == 1.0 ? result : result - 1.0;
        }

        private double processSingleInstrument(InstrumentResource instrument)
        {
            double result = 1.0;
            bool isUsedByThis = instrument.users.Contains(this.id);

            if (instrument.users.Count == 0)
                instrument.isBusy = false;

            if (instrument.isBusy && !instrument.isShared && !isUsedByThis)
                return Double.Epsilon;

            if (instrument.isBusy && instrument.isShared || instrument.isBusy && isUsedByThis)
            {
                if (!instrument.users.Contains(this.id))
                {
                    instrument.users.Add(this.id);
                }

                result += processMaterials(instrument) * instrument.power * instrument.count / (instrument.users.Count > 0 ? instrument.users.Count : 1);
            }

            if (!instrument.isBusy)
            {
                result += processMaterials(instrument) * instrument.power * instrument.count;
                if (!instrument.users.Contains(this.id))
                {
                    instrument.users.Add(this.id);
                    instrument.isBusy = true;
                }
            }
            return result == 1.0 ? result : result - 1.0;
        }

        private double processMaterials(InstrumentResource resource) 
        {
            double result = 1.0;
 
            foreach (MaterialResource material in resource.materials)
            {      

                result += processSingleMaterial(material);                    
            }
            return result == 1.0 ? result : result - 1.0;
        }

        private double processSingleMaterial(MaterialResource material)
        {
            Model model = Model.Instance;

            bool isUsedByThis = material.users.Contains(this.id);

            if (material.users.Count == 0)
                material.isBusy = false;

            if (material.isBusy && !material.isShared && !isUsedByThis)
                return Double.Epsilon;

            double usersCount = material.users.Count > 0 ? material.users.Count : 1.0 ;
            if ((material.count - material.perTick / usersCount) > 0)
            {
                material.count -= material.perTick / usersCount;
                
                if (!isUsedByThis) {
                    material.users.Add(this.id);
                    material.isBusy = true;
                }
                return 1.0;
            }
            else
            {
                model.state = ProcessingState.RESOURCES_EMPTY;
                return Double.Epsilon;
            } 
        }
        //private void calculatePerformanse(Project project, double needTime)
        //{
        //    double sumPrice = 0.0;
        //    foreach (Resource res in resources)
        //    {
        //        sumPrice += res.price;
        //    }

        //    project.performance += needTime / sumPrice;
        //}

    }
}
