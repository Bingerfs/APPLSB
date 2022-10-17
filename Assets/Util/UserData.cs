using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    [Serializable]
    public class UserData
    {
        public string userName;

        public int userExperience;

        public bool isFirstTime;

        public SerializableDictionary<string, int> progress;

        public UserData(string userName)
        {
            progress = new SerializableDictionary<string, int>();
            this.userName = userName;
            userExperience = 0;
            isFirstTime = true;
        }

        public UserData()
        {
            progress = new SerializableDictionary<string, int>();
            userName = "";
            userExperience = 0;
            isFirstTime = true;
        }
    }
}
