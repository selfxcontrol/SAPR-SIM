using Entities;
using sapr_sim.Figures;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EntityTransformator
{
    public class TransformerService
    {

        private Dictionary<UIEntity, Entity> map = new Dictionary<UIEntity, Entity>();
        private Dictionary<UIEntity, Entities.Resource> resMap = new Dictionary<UIEntity, Entities.Resource>();
        private List<Entities.Resource> resources = new List<Entities.Resource>();
        
        public List<Entity> transform(UIElementCollection elements)
        {
            List<Entity> realEntities = new List<Entity>();
            
            foreach(UIElement e in elements)
            {
                // skip no logic ui entities
                if (e is sapr_sim.Figures.Label || e is Port || e is ConnectionLine || e is sapr_sim.Figures.Resource) continue;
                
                Transformer transformer = TransformerFactory.getTransformer(e.GetType());
                Entity re = transformer.transform(e as UIEntity);
                if (re != null)
                {
                    realEntities.Add(re);
                    map.Add(e as UIEntity, re);
                }
            }

            foreach (UIElement e in elements)
            {
                if (e is ConnectionLine)
                    processConnectionLine(e as ConnectionLine, true);
                else if (e is sapr_sim.Figures.Resource)
                    processResource(elements, e as sapr_sim.Figures.Resource);
            }

            foreach (UIElement e in elements)
            {
                if (e is sapr_sim.Figures.MaterialResource || e is sapr_sim.Figures.InstrumentResource)
                    connectResourcesBetweenThemselves(elements, e as sapr_sim.Figures.Resource);
            }

            processSubprocess(elements);

            return realEntities;
        }

        public List<UIEntity> transform(List<Identifable> elements)
        {
            List<UIEntity> uiEntities = new List<UIEntity>(elements.Count);

            foreach(Identifable e in elements)
            {
                if (e is Entity)
                {
                    Entity ent = e as Entity;
                    if (map.ContainsValue(ent))
                    {
                        UIEntity uie = map.FirstOrDefault(x => x.Value.Equals(ent)).Key;
                        if (uie != null)
                            uiEntities.Add(uie);
                    }
                }
                else if (e is Entities.Resource)
                {
                    Entities.Resource res = e as Entities.Resource;
                    if (resources.Contains(res))
                    {
                        UIEntity uie = resMap.FirstOrDefault(x => x.Value.Equals(res)).Key;
                        if (uie != null)
                            uiEntities.Add(uie);
                    }
                }
            }
            return uiEntities;
        }

        public List<Entities.Resource> getResources()
        {
            return resources;
        }

        private void processConnectionLine(ConnectionLine c, bool skipSubprocess)
        {
            UIEntity src = c.SourcePort.Owner;
            UIEntity dest = c.DestinationPort.Owner;

            if (skipSubprocess && (src is SubDiagram || dest is SubDiagram)) return;

            if (map.ContainsKey(src) && map.ContainsKey(dest))
                addLinkForRealEntities(c, map[src] as Entity, map[dest] as Entity);
        }

        private void processResource(UIElementCollection elements, sapr_sim.Figures.Resource resource)
        {
            List<ConnectionLine> connectors = ConnectorFinder.find(elements, resource);

            Entities.Resource res = null;
            if (resource is sapr_sim.Figures.WorkerResource)
            {
                sapr_sim.Figures.WorkerResource wr = resource as sapr_sim.Figures.WorkerResource;
                res = new Entities.WorkerResource()
                {
                    name = wr.EntityName,
                    id = wr.Id,
                    count = wr.Count,
                    type = Entities.ResourceType.WORKER,
                    efficiency = wr.Efficiency,
                    isShared = wr.IsShared,
                    price = wr.Price
                };
            }
            else if (resource is sapr_sim.Figures.InstrumentResource)
            {
                sapr_sim.Figures.InstrumentResource ir = resource as sapr_sim.Figures.InstrumentResource;
                res = new Entities.InstrumentResource()
                {
                    name = ir.EntityName,
                    id = ir.Id,
                    count = ir.Count,
                    type = Entities.ResourceType.INSTRUMENT,
                    isShared = ir.IsShared,
                    price = ir.Price,
                    power = ir.Power
                };
            }
            else if (resource is sapr_sim.Figures.MaterialResource)
            {
                sapr_sim.Figures.MaterialResource mr = resource as sapr_sim.Figures.MaterialResource;
                res = new Entities.MaterialResource()
                {
                    name = mr.EntityName,
                    id = mr.Id,
                    count = mr.Count,
                    type = Entities.ResourceType.MATERIAL,
                    isShared = mr.IsShared,
                    perTick = mr.Consumption
                };
            }

            resources.Add(res);
            resMap.Add(resource as UIEntity, res);

            foreach (ConnectionLine con in connectors)
            {
                UIEntity procedure = null;
                if (con.SourcePort != null)
                {
                    UIEntity src = con.SourcePort.Owner;
                    UIEntity dst = con.DestinationPort.Owner;
                    procedure = src is Procedure ? src : dst is Procedure ? dst : null;
                    
                    if (procedure != null)
                    {
                        Entities.impl.Procedure realprocedure = map[procedure] as Entities.impl.Procedure;
                        if (!realprocedure.getResources().Contains(res))
                            addAdditionalRelations(realprocedure, res);
                    }                    
                }
            }
        }

        private void connectResourcesBetweenThemselves(UIElementCollection elements, sapr_sim.Figures.Resource resource)
        {
            List<ConnectionLine> connectors = ConnectorFinder.find(elements, resource);

            foreach (ConnectionLine con in connectors)
            {
                UIEntity worker = null;
                UIEntity instrument = null;
                UIEntity material = null;

                if (con.SourcePort != null)
                {
                    UIEntity src = con.SourcePort.Owner;
                    UIEntity dst = con.DestinationPort.Owner;
                    worker = src is sapr_sim.Figures.WorkerResource ? src : dst is sapr_sim.Figures.WorkerResource ? dst : null;
                    instrument = src is sapr_sim.Figures.InstrumentResource ? src : dst is sapr_sim.Figures.InstrumentResource ? dst : null;
                    material = src is sapr_sim.Figures.MaterialResource ? src : dst is sapr_sim.Figures.MaterialResource ? dst : null;

                    if (src is sapr_sim.Figures.WorkerResource && dst is sapr_sim.Figures.MaterialResource
                        || dst is sapr_sim.Figures.WorkerResource && src is sapr_sim.Figures.MaterialResource)
                    {
                        if (worker != null && material != null)
                        {
                            Entities.WorkerResource realWorker = resMap[worker] as Entities.WorkerResource;
                            Entities.MaterialResource realMaterial = resMap[material] as Entities.MaterialResource;
                            if (!realWorker.materials.Contains(realMaterial))
                                realWorker.materials.Add(realMaterial);
                        }
                    }

                    if (src is sapr_sim.Figures.WorkerResource && dst is sapr_sim.Figures.InstrumentResource
                        || dst is sapr_sim.Figures.WorkerResource && src is sapr_sim.Figures.InstrumentResource)
                    {
                        if (worker != null && instrument != null)
                        {
                            Entities.WorkerResource realWorker = resMap[worker] as Entities.WorkerResource;
                            Entities.InstrumentResource realInstrument = resMap[instrument] as Entities.InstrumentResource;
                            if (!realWorker.instruments.Contains(realInstrument))
                                realWorker.instruments.Add(realInstrument);
                        }
                    }

                    if (src is sapr_sim.Figures.InstrumentResource && dst is sapr_sim.Figures.MaterialResource
                        || src is sapr_sim.Figures.MaterialResource && dst is sapr_sim.Figures.InstrumentResource)
                    {
                        if (instrument != null && material != null)
                        {
                            Entities.InstrumentResource realInstrument = resMap[instrument] as Entities.InstrumentResource;
                            Entities.MaterialResource realMaterial = resMap[material] as Entities.MaterialResource;
                            if (!realInstrument.materials.Contains(realMaterial))
                                realInstrument.materials.Add(realMaterial);
                        }
                    }
                }
            }
        }

        private void processSubprocess(UIElementCollection elements)
        {
            foreach (UIElement e in elements)
            {
                if (e is SubDiagram)
                {
                    SubDiagram sd = e as SubDiagram;
                    List<ConnectionLine> connectors = ConnectorFinder.find(elements, sd);
                    foreach (ConnectionLine cl in connectors)
                    {
                        processConnectionLine(cl, false);
                    }
                }
            }
        }

        private void addLinkForRealEntities(ConnectionLine c, Entity realSrc, Entity realDest)
        {
            if (c.SourcePort.PortType == PortType.OUTPUT && c.DestinationPort.PortType == PortType.INPUT)
            {
                realSrc.addOutput(realDest);
                realDest.addInput(realSrc);
            }
            else if (c.SourcePort.PortType == PortType.INPUT && c.DestinationPort.PortType == PortType.OUTPUT)
            {
                realSrc.addInput(realDest);
                realDest.addOutput(realSrc);
            }
        }

        private void addAdditionalRelations(Entity src, Entities.Resource dst)
        {
            if (src is Entities.impl.Procedure)
            {
                (src as Entities.impl.Procedure).addResource(dst);
            }
        }

    }
}
