using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DataPersistence
{
    public class AssetsDataHandler<T> : IDataHandler<T>
    {
        private string _dataDirPath = "";

        private string _dataFileName = "";

        public AssetsDataHandler(string dataDirPath, string dataFileName)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
        }

        public bool DoesFileExist(string fileName)
        {
            return File.Exists(Path.Combine(_dataDirPath, fileName));
        }

        public T Load()
        {
            var fullPath = _dataFileName != null ? Path.Combine(_dataDirPath, _dataFileName) : null;
            T userData = default(T);
            if (fullPath != null)
            {
                try
                {
                    string dataToLoad = "";
                    dataToLoad = Resources.Load<TextAsset>(fullPath).text;
                    userData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (ArgumentException ex)
                {
                    string dataToLoad = "";
                    dataToLoad = Resources.Load<TextAsset>(fullPath).text;
                    userData = JsonConvert.DeserializeObject<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            return userData;
        }

        public void Save(T data)
        {
            throw new NotImplementedException();
        }
    }
}
