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

        public float userExperience;

        public bool isFirstTime;

        public SerializableDictionary<string, UserProgressData> progress;

        public UserData(string userName)
        {
            progress = new SerializableDictionary<string, UserProgressData>();
            this.userName = userName;
            userExperience = 0;
            isFirstTime = true;
        }

        public UserData()
        {
            progress = new SerializableDictionary<string, UserProgressData>();
            userName = "";
            userExperience = 0;
            isFirstTime = true;
        }
    }
}
