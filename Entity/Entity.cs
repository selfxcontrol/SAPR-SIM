using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public abstract class Entity : Identifable
    {

        private IList<Entity> _input;

        public virtual IList<Entity> input
        {
            get { return _input ?? (_input = new List<Entity>()); }
            protected set { _input = value; }
        }

        private IList<Entity> _output;

        public virtual IList<Entity> output
        {
            get { return _output ?? (_output = new List<Entity>()); }
            protected set { _output = value; }
        }

        private IList<Project> notReadyProjectQueue = new List<Project>();
        private IList<Project> readyProjectQueue = new List<Project>();

        public IList<Entity> getInputs()
        {
            return input;
        }

        public virtual void setInputs(IList<Entity> inputs)
        {
            input = inputs;
        }

        public virtual void addInput(Entity input)
        {
            this.input.Add(input);
        }

        public bool hasInputs()
        {
            return input.Count > 0;
        }

        public IList<Entity> getOutputs()
        {
            return output;
        }

        public virtual void setOutputs(IList<Entity> output)
        {
            this.output = output;
        }

        public virtual void addOutput(Entity output)
        {
            this.output.Add(output);
        }

        public bool hasOutputs()
        {
            return output.Count > 0;
        }

        public IList<Project> getReadyProjectQueue()
        {
            return readyProjectQueue;
        }

        public void setReadyProjectQueue(IList<Project> inputQueue)
        {
            this.readyProjectQueue = new List<Project>(inputQueue);
        }

        public IList<Project> getNotReadyProjectQueue()
        {
            return notReadyProjectQueue;
        }

        public void setNotReadyProjectQueue(IList<Project> inputQueue)
        {
            this.notReadyProjectQueue = new List<Project>(inputQueue);
        }

        public Project getProjectFromReadyQueue()
        {
            IList<Project> prjQueue = getReadyProjectQueue();
            if (prjQueue != null && prjQueue.Count != 0)
            {
                return prjQueue.First();
            }

            return null;
        }

        public void removeProjectFromReadyQueue()
        {
            IList<Project> prjQueue = getReadyProjectQueue();
            if (prjQueue != null && prjQueue.Count != 0)
            {
                Project prj = prjQueue.First();
                prjQueue.Remove(prj);
            }
        }

        public void addProjectToQueue(Project project)
        {
            IList<Project> prjQueue = getNotReadyProjectQueue();
            prjQueue.Add(project);
            setNotReadyProjectQueue(prjQueue);
        }

        public void proceed()
        {
           execute();
           ICollection<Project> projects = getNotReadyProjectQueue();

           foreach (Project project in projects)
           {
               getReadyProjectQueue().Add(project);
           }

           getNotReadyProjectQueue().Clear(); //did we miss that?
        }

        public abstract void execute();

        public abstract bool canUseAsInput(Entity entity);

        public abstract bool canUseAsOutput(Entity entity);

        public abstract bool correctInputCount();

        public abstract bool correctOutputCount();

        public override string ToString()
        {
            return name;
        }

    }
}
