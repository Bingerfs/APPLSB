using Assets.DataPersistence;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; set; }

    private UserData _userData;

    private IEnumerable<IDataPersistence> _dataPersistenceObjects;

    [SerializeField]
    private string _fileName;

    [SerializeField]
    private UserPreferences _userPreferences;

    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one instance of Data Persistance Manager.");
        }

        Instance = this;
    }

    void Start()
    {
        _fileName = $"{_userPreferences.UserId}.json";
        dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewUserData(string userName)
    {
        _userData = new UserData(userName);
    }

    public void LoadUserData(string userCode)
    {
        _userData = dataHandler.Load();
        if (_userData == null)
        {
            Debug.Log("No data found. Starting a guest session.");
            _userData = new UserData();
        }

        foreach (var persistenceObject in _dataPersistenceObjects)
        {
            persistenceObject.LoadUserData(_userData);
        }
    }

    public void UpdateUserData(string userCode)
    {
        foreach (var persistenceObject in _dataPersistenceObjects)
        {
            persistenceObject.SaveUserData(ref _userData);
        }

        dataHandler.Sava(_userData);
    }

    public IEnumerable<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return dataPersistenceObjects;
    }
}
