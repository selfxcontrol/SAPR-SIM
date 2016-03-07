using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityValidator.exeptions
{
    public class ValidationError
    {

        private string message;
        private Entities.Identifable failedEntity;

        public ValidationError(string message) 
        {
            this.message = message;
        }

        public ValidationError(string message, Entities.Identifable failedEntity) : this(message)
        {
            this.failedEntity = failedEntity;
        }

        public string Message
        {
            get { return message; }
        }

        public Entities.Identifable FailedEntity
        { 
            get { return failedEntity; } 
        }

    }
}
