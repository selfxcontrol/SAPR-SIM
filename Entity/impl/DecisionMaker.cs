using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public enum ProbabilityChangeRule
    {
        UP,
        DOWN,
        EQ
    };

    public class DecisionMaker : Entity
    {
        static Random rand = new Random();
        public String inputProbabilityParams {get; set; }
        List<Double> probabilities = new List<Double>();

        double delta { get; set; }

        public ProbabilityChangeRule changeRule { get; set; }

        public override void execute() 
        {
            parseImpurtParams();
            int output = chooseOutput();

            Project prj = getProjectFromReadyQueue();
            Entity outputEntity = getOutputs()[output];
            outputEntity.addProjectToQueue(prj);

            getReadyProjectQueue().Remove(prj);
            changeProbs(output);
        }

        public override bool canUseAsInput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityStart || entity is Submodel;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return entity is Procedure || entity is Synchronization || entity is Parallel || entity is EntityDestination || entity is Submodel;
        }

        public override bool correctInputCount()
        {
            return input.Count == 1;
        }

        public override bool correctOutputCount()
        {
            // hz
            return true;
        }

        //ololo govnocod here. Should be refactor as soon as possible
        private void parseImpurtParams()
        {
            String[] imputStrs = inputProbabilityParams.Split('=');

            switch (imputStrs[0])
            {
                case "U" : changeRule = ProbabilityChangeRule.UP; break;
                case "D" : changeRule = ProbabilityChangeRule.DOWN; break;
                case "E": changeRule = ProbabilityChangeRule.EQ; break;
                default: throw new Exception();
            }

            String[] probs = imputStrs[1].Split(';');

            foreach (String prob in probs)
            {
                probabilities.Add(Double.Parse(prob));
            }

            probabilities.Sort();
        }

        private int chooseOutput()
        {
            double randProbability = rand.NextDouble();

            for (int i = 0; i < probabilities.Count; i++)
            {
                if (randProbability <= probabilities[i])
                {
                    return i;
                } 
            }

            return 0;
        }

        private void changeProbs(int output)
        {
            switch (changeRule)
            {
                case ProbabilityChangeRule.UP :
                    upProb(output);
                    break;
                case ProbabilityChangeRule.DOWN :
                    downProb(output);
                    break;
                default :
                    break;
            }
        }

        private void downProb(int output)
        {
            double delta = probabilities[output];

            for (int i = 0; i < probabilities.Count; i++)
            {
                if (i == output)
                    probabilities[i] -= delta * (probabilities.Count - 1);
                else
                    probabilities[i] += delta;
            }
        }

        private void upProb(int output)
        {
            double delta = probabilities[output];

            for (int i = 0; i < probabilities.Count; i++)
            {
                if (i == output)
                    probabilities[i] += delta * (probabilities.Count - 1);
                else
                    probabilities[i] -= delta;
            }
        }

    }
}
