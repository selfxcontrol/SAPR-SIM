using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Utils
{
    class IdGenerator
    {
        private static long id = 0;

        public static long GenerateId()
        {
            return ++id;
        }
    }
}
