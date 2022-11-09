using Assets.DataPersistence;
using Assets.Util;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private GameObject _smallDialog;

    public GameObject MediumDialog { get => _smallDialog; set => _smallDialog = value; }

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

    private void NewUserData(string userName)
    {
        _userData = new UserData(userName);
        _userPreferences.UserId = _userIdGenerator.GenerateUserId();
        _userPreferences.isGuestUser = false;
        _fileName = $"{_userPreferences.UserId}.json";
        dataHandler = new FileDataHandler<UserData>(Application.persistentDataPath, _fileName);
        dataHandler.Save(_userData);
    }

    private void NewGuestSession()
    {
        _userData = new UserData("INVITADO");
        _userPreferences.isGuestUser = true;
        _userPreferences.UserId = null;
        _userPreferences.UserName = "INVITADO";
    }

    private void PushDataToAllObjects()
    {
        foreach (var persistenceObject in _dataPersistenceObjects)
        {
            persistenceObject.LoadUserData(_userData);
        }
    }

    public void LoadUserData()
    {
        _userData = dataHandler.Load();
        if (_userData == null && !string.IsNullOrEmpty(_userPreferences.UserId))
        {
            NewGuestSession();
            Dialog myDialog = Dialog.Open(MediumDialog, DialogButtonType.Yes | DialogButtonType.No, "Usuario no encontrado", "No se encontre el id del usuario ingresado. Desea continuar como un invitado?", true);
            if (myDialog != null)
            {
                myDialog.OnClosed += OnClosedDialogEvent;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(_userPreferences.UserName) && string.IsNullOrEmpty(_userPreferences.UserId))
            {
                NewUserData(_userPreferences.UserName);
                Dialog myDialog = Dialog.Open(MediumDialog, DialogButtonType.OK, "Usuario Nuevo", $"Su codigo de usuario es: {_userPreferences.UserId}. No olvide anotarlo antes de cerrar el dialogo, este codigo le sirve para poder recuperar su progreso en otra sesion.", true);
            }

            if (_userPreferences.isGuestUser)
            {
                NewGuestSession();
            }
        }

        _userPreferences.UserName = _userData.userName;
        PushDataToAllObjects();
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

    private async void OnClosedDialogEvent(DialogResult obj)
    {
        switch (obj.Result)
        {
            case DialogButtonType.No:
                _userPreferences.isGuestUser = false;
                _userPreferences.UserId = null;
                _userPreferences.UserName = null;
                IMixedRealitySceneSystem sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
                ISceneTransitionService transition = MixedRealityToolkit.Instance.GetService<ISceneTransitionService>();
                if (!transition.TransitionInProgress)
                {
                    await transition.DoSceneTransition(
                    () => sceneSystem.LoadContent("MainMenuScene", LoadSceneMode.Single)
                );
                }
                break;
            case DialogButtonType.Yes:
                Debug.Log("No data found. Starting a guest session.");
                break;
            default:
                break;
        }
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
