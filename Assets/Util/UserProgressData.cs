using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    [Serializable]
    public class UserProgressData
    {
        public string signCode;

        public string signWord;

        public long totalTries;

        public long correctResponses;
    }
}
