using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public class ValidationException : Exception
    {
        private List<ErrorTuple> errors = new List<ErrorTuple>();

        public void addError(string message, List<Identifable> entities)
        {
            errors.Add(new ErrorTuple(message, entities));
        }

        public void addError(string message, Identifable entity)
        {
            addError(message, new List<Identifable>() { entity });
        }

        public List<ErrorTuple> Errors
        {
            get { return errors; }
        }

    }

    public class ErrorTuple
    {
        private string message;
        private List<Identifable> entities;

        public ErrorTuple(string message, List<Identifable> entities)
        {
            this.message = message;
            this.entities = entities;
        }

        public string Message
        {
            get { return message; }
        }

        public List<Identifable> Entities
        {
            get { return entities; }
        }
    }
}
