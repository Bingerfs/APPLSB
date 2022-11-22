using Assets.DataPersistence.Models;
using Assets.Util;
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
    public class FileDataHandler<T> : IDataHandler<T>
    {
        private string _dataDirPath = "";

        private string _dataFileName = "";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
        }

        public T Load()
        {
            var fullPath = _dataFileName != null ? Path.Combine(_dataDirPath, _dataFileName) : null;
            T userData = default(T);
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

                    userData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (ArgumentException ex)
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    userData = JsonConvert.DeserializeObject<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e.Message}");
                }
            }

            return userData;
        }

        public async Task<T> LoadAsync()
        {
            var fullPath = _dataFileName != null ? Path.Combine(_dataDirPath, _dataFileName) : null;
            T userData = default(T);
            if (fullPath != null && File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = await reader.ReadToEndAsync();
                        }
                    }

                    userData = JsonUtility.FromJson<T>(dataToLoad);
                }
                catch (ArgumentException ex)
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    userData = JsonConvert.DeserializeObject<T>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e.Message}");
                }
            }

            return userData;
        }

        public async Task<List<T>> LoadAllInDirectory()
        {
            var files = Directory.GetFiles(_dataDirPath, "*.json");
            List<T> retrievedData = new List<T>();
            if (files.Any())
            {
                var originalFileName = _dataFileName;
                foreach (var file in files)
                {
                    _dataFileName = file;
                    retrievedData.Add(await LoadAsync());
                }

                _dataFileName = originalFileName;
            }

            return retrievedData;
        }

        public void Save(T data)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                //Debug.LogError("About to create directory");
                if (!new DirectoryInfo(Application.persistentDataPath).Exists)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                }
                
                //Debug.LogError("Directory created. About to create file");
                string dataToStore = JsonUtility.ToJson(data, true);
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
                Debug.LogError($"{e.Message}");
            }
        }

        public bool DoesFileExist(string fileName)
        {
            return File.Exists(Path.Combine(_dataDirPath, fileName));
        }
    }
}
