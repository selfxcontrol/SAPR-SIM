using Entities;
using Entities.impl;
using EntityValidator.exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.rules
{
    public class ResourceConnectedCorrectRule : Rule
    {

        private List<Identifable> failed = new List<Identifable>();
        private ICollection<Resource> resources = Model.Instance.getResources();

        private List<WorkerResource> workers = new List<WorkerResource>();
        private List<InstrumentResource> instruments = new List<InstrumentResource>();
        private List<MaterialResource> materials = new List<MaterialResource>();

        public ResourceConnectedCorrectRule(ICollection<Entity> entities, ICollection<Resource> resources)
            : base(entities)
        {
            this.resources = resources;
        }

        public override bool validate()
        {
            foreach (Resource res in resources)
            {
                if (ResourceType.WORKER.Equals(res.type))
                    workers.Add(res as WorkerResource);
                else if (ResourceType.INSTRUMENT.Equals(res.type))
                    instruments.Add(res as InstrumentResource);
                else if (ResourceType.MATERIAL.Equals(res.type))
                    materials.Add(res as MaterialResource);
            }

            foreach (Entity e in entities)
            {
                if (e is Procedure)
                {
                    Procedure p = (e as Procedure);
                    foreach (Resource res in p.getResources())
                    {
                        if (ResourceType.WORKER.Equals(res.type))
                            processWorkerResources(res as WorkerResource);
                        else if (ResourceType.INSTRUMENT.Equals(res.type))
                            processInstrumentResources(new List<InstrumentResource>() { res as InstrumentResource });
                        else if (ResourceType.MATERIAL.Equals(res.type))
                            processMaterialResources(new List<MaterialResource>() { res as MaterialResource });
                    }
                }
            }

            failed.AddRange(workers);
            failed.AddRange(instruments);
            failed.AddRange(materials);

            return failed.Count == 0;
        }

        public override ICollection<ValidationError> explain()
        {
            List<ValidationError> errors = new List<ValidationError>();
            foreach(Identifable fail in failed)
                errors.Add(new ValidationError("Ресурс '" + fail.name + "' не верно подключен", fail));            
            return errors;
        }

        private void processWorkerResources(WorkerResource wr)
        {
            processInstrumentResources(wr.instruments);
            processMaterialResources(wr.materials);

            if (workers.Contains(wr))
                workers.Remove(wr);
        }

        private void processInstrumentResources(IList<InstrumentResource> resources)
        {
            foreach (InstrumentResource ir in resources)
            {
                processMaterialResources(ir.materials);

                if (instruments.Contains(ir))
                    instruments.Remove(ir);
            }
        }

        private void processMaterialResources(IList<MaterialResource> resources)
        {
            foreach (MaterialResource mr in resources)
            {
                if (materials.Contains(mr))
                    materials.Remove(mr);
            }
        }
    }
}
