using Entities.impl;
using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Model
    {
        private static Model instance = new Model();


        // EF not set params if constructor is private.
        // TODO find workaround 
        //private Model(){ }

        public static Model Instance
        {
            get
            {
                return instance;
            }
            // todo remove 
            set { instance = value; }
        }

        public long id { get; set; }

        public double simulatePeriod { get; set; }
        public double projectAppearenceProbability { get; set; }

        public double timeRestriction { get; set; }
        public ProcessingState state { get; set; }

        private IList<Entity> _entities;

        public virtual IList<Entity> entities
        {
            get { return _entities ?? (_entities = new List<Entity>()); }
            protected set { _entities = value; }
        }


        private IList<Project> _projects;

        public virtual IList<Project> projects
        {
            get { return _projects ?? (_projects = new List<Project>()); }
            protected set { _projects = value; }
        }


        private IList<Resource> _resources;

        public virtual IList<Resource> resources
        {
            get { return _resources ?? (_resources = new List<Resource>()); }
            protected set { _resources = value; }
        }

        public void addEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public IList<Entity> getEntities()
        {
            return entities;
        }

        public void addProject(Project project)
        {
            projects.Add(project);
        }

        public IList<Project> getProjects()
        {
            return projects;
        }

        public void addResource(Resource resource)
        {
            resources.Add(resource);
        }

        public IList<Resource> getResources()
        {
            return resources;
        }

        public void setResources(IList<Resource> resources)
        {
            this.resources = resources;
        }
    }
}
