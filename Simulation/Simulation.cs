using Entities;
using Entities.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class Simulation
    {
        public static SimulationResult simulate()
        {
            SimulationResult result = new SimulationResult();

            Model model = Model.Instance;
            Timer timer = Timer.Instance;

            Timer.Instance.resetTime();
            model.getProjects().Clear();

            IList<Entity> entities = model.getEntities();

            EntityStart start = getSchemaStart(entities) as EntityStart;
            for (int i = 0; i < start.projectsCount; i++)
                model.addProject(new Project() { complexity = start.Complexity, id = i });
            IList<Project> projects = model.getProjects();

            start.setReadyProjectQueue(projects);

            model.state = ProcessingState.FINE;

            while (projects.Count != 0 && checkForNotReadyProjects(projects))
            {
                foreach (Entity entity in entities)
                {
                    if ((entity.getReadyProjectQueue() != null && entity.getReadyProjectQueue().Count != 0) ||
                        (entity.getNotReadyProjectQueue() != null && entity.getNotReadyProjectQueue().Count != 0))
                    {
                        entity.proceed();
                    }
                }
                timer.increment();
                if (timer.getTime() > model.timeRestriction)
                {
                    model.state = ProcessingState.TIME_OUT;
                    break;
                }
                if (model.state.Equals(ProcessingState.RESOURCES_EMPTY))
                    break;
            }
            
            //set simulations results
            result.simulationTime = timer.getTime();
            result.state = model.state;

            return result;
        }

        private static bool checkForNotReadyProjects(ICollection<Project> projects)
        {
            foreach (Project prj in projects)
            {
                if (prj.state.Equals(Entities.State.IN_PROGRESS))
                    return true;
            }

            return false;
        }

        private static Entity getSchemaStart(ICollection<Entity> entities)
        {
            foreach (Entity ent in entities)
            {
                if (ent is EntityStart)
                {
                    return ent;
                }
            }
            return null;
        }

        private static Entity getSchemaDestenation(ICollection<Entity> entities)
        {
            foreach (Entity ent in entities)
            {
                if (ent is EntityDestination)
                {
                    return ent;
                }
            }
            return null;
        }
    }
}
