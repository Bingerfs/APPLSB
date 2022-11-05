using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public static DebugLogger Instance;

    [SerializeField]
    private TextMeshPro debugText;

    private IMixedRealityHandJointService _handJointService;

    private IMixedRealityHandJointService HandJointService =>
        _handJointService ??
        (_handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

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
            debugText.text = ($"{logString}-{ stackTrace}-{ type}");
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
