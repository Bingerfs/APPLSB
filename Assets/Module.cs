using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [Serializable]
    public class Module
    {
        public string Name { get; set; }

        public IEnumerable<ModuleCategory> Categories { get; set; } 
    }
}
