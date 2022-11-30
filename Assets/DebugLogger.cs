using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public static DebugLogger Instance;

    [SerializeField]
    private TextMeshPro _debugText;

    [SerializeField]
    private GameObject _smalDialogPrefab;

    private Dialog _errorDialog;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one instance of the Module Data Manager.");
        }

        Instance = this;
        Application.logMessageReceived += HandleException;
    }

    void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            //_debugText.text = ($"{logString}-{ stackTrace}-{ type}");
            OpenErrorDialog();
        }
    }

    private void OpenErrorDialog()
    {
        if (_errorDialog != null)
        {
            Destroy(_errorDialog);
            _errorDialog = null;
        }

        if (_smalDialogPrefab != null)
        {
            _errorDialog = Dialog.Open(_smalDialogPrefab, DialogButtonType.OK, "<align=\"center\">Error", "Ocurrio un error en la aplicacion. Comuniquese con el desarrollador.", false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    
}
