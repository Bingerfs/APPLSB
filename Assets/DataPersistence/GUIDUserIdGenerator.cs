using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataPersistence
{
    public class GUIDUserIdGenerator : IUserIdGenerator
    {
        private FileDataHandler _fileDataHandler;

        public GUIDUserIdGenerator(FileDataHandler fileDataHandler)
        {
            _fileDataHandler = fileDataHandler;
        }

        public string GenerateUserId()
        {
            var newGUID = "";
            do
            {
                newGUID = Guid.NewGuid().ToString();
                newGUID = newGUID.Substring(0, 8);
            } while (_fileDataHandler.DoesFileExist(newGUID));

            return newGUID;
        }
    }
}
