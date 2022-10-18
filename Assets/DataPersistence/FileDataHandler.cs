using Assets.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DataPersistence
{
    public class FileDataHandler
    {
        private string _dataDirPath = "";

        private string _dataFileName = "";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
        }

        public UserData Load()
        {
            var fullPath = _dataFileName != null ? Path.Combine(_dataDirPath, _dataFileName) : null;
            UserData userData = null;
            if (fullPath != null && File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    userData = JsonUtility.FromJson<UserData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            return userData;
        }

        public void Sava(UserData userData)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(userData, true);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public bool DoesFileExist(string fileName)
        {
            return File.Exists(Path.Combine(_dataDirPath, fileName));
        }
    }
}
