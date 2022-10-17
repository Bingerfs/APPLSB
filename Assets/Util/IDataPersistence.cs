using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public interface IDataPersistence
    {
        void LoadUserData(UserData userData);

        void SaveUserData(ref UserData userData);
    }
}
