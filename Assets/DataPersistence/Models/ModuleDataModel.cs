
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataPersistence.Models
{
    [Serializable]
    public class ModuleDataModel
    {
        public string Name { get; set; }

        public List<CategoryFileModel> Categories { get; set; }
    }
}
