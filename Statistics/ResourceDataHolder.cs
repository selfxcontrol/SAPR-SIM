using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    public class ResourceDataHolder
    {

        // duplicate, cause cyclic dependencies :((
        public enum ResourceType 
        {
            WORKER,
            INSTRUMENT,
            MATERIAL
        }

        public long id { get; set;}
        public string name { get; set; }
        public ResourceType type;

        public ResourceDataHolder(long id, string name, ResourceType type)
        {
            this.type = type;
            this.id = id;
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is ResourceDataHolder)
            {
                return this.id == (obj as ResourceDataHolder).id;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

    }
}
