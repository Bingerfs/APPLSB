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

    private IUserIdGenerator _userIdGenerator;

    private IEnumerable<IDataPersistence> _dataPersistenceObjects;

    [SerializeField]
    private string _fileName;

    [SerializeField]
    private UserPreferences _userPreferences;

    private FileDataHandler<UserData> dataHandler;

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
        _fileName = string.IsNullOrEmpty(_userPreferences._userId) ? null : $"{_userPreferences.UserId}.json";
        dataHandler = new FileDataHandler<UserData>(Application.persistentDataPath, _fileName);
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        _userIdGenerator = new GUIDUserIdGenerator(dataHandler);
        LoadUserData();
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(_userPreferences.UserId) && !_fileName.Contains(_userPreferences.UserId))
        {
            _fileName = $"{_userPreferences.UserId}.json";
            dataHandler = new FileDataHandler<UserData>(Application.persistentDataPath, _fileName);
        }
    }

    public void NewUserData(string userName)
    {
        _userData = new UserData(userName);
        _userPreferences.UserId = _userIdGenerator.GenerateUserId();
        _fileName = $"{_userPreferences.UserId}.json";
        dataHandler = new FileDataHandler<UserData>(Application.persistentDataPath, _fileName);
        dataHandler.Save(_userData);
    }

    public void LoadUserData()
    {
        if (!_userPreferences.IsGuestUser)
        {
            _userData = dataHandler.Load();
            if (_userData == null)
            {
                Debug.Log("No data found. Starting a guest session.");
                NewUserData(_userPreferences.UserName);
            }

            foreach (var persistenceObject in _dataPersistenceObjects)
            {
                persistenceObject.LoadUserData(_userData);
            }
        }
    }

    public void UpdateUserData()
    {
        if (!_userPreferences.isGuestUser)
        {
            foreach (var persistenceObject in _dataPersistenceObjects)
            {
                persistenceObject.SaveUserData(ref _userData);
            }

            dataHandler.Save(_userData);
        }
    }

    public IEnumerable<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return dataPersistenceObjects;
    }

    public void OnApplicationQuit()
    {
        UpdateUserData();
        _userPreferences.UserName = null;
        _userPreferences.PreferredHandedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;
        _userPreferences.UserId = null;
        _userPreferences.isGuestUser = false;
    }
}
