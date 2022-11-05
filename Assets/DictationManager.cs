using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using FantomLib;
using UnityEngine.Events;
using UnityEngine.Android;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Assets.Util;

public class DictationManager : MonoBehaviour
{
    [Serializable] public class ResultHandler : UnityEvent<string> { }
    public ResultHandler OnRequest;

    public ResultHandler OnEvaluationResponse;

    public ResultHandler OnStopRecording;

    [SerializeField]
    public GameObject mainTextToolTip;

    private ToolTip toolTip;

    private LSBModule _currentModule = LSBModule.INTERPRETATION;

    // Start is called before the first frame update
    void Start()
    {
        RequestUserMicrophonePermission();
    }

    private void OnEnable()
    {
        toolTip = mainTextToolTip.GetComponent<ToolTip>();
    }

    private static void RequestUserMicrophonePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }

    public void SwitchWebSearch(string[] words)
    {
        String basicText = words[0].ToLower();
        if (OnRequest != null)
            OnRequest.Invoke(basicText);
    }

    public void OnStartRecording()
    {
        Debug.Log("------------------------------Recording started");
        mainTextToolTip.SetActive(true);
        toolTip.ToolTipText = "Grabando...";
    }

    public void OnResult(string sentence)
    {
        Debug.Log("------------------------------Result");
        Debug.Log(sentence);
    }

    public void OnComplete(string sentence)
    {
        Debug.Log("-----------------------Complete");
        mainTextToolTip.SetActive(false);
        Debug.Log(sentence);
        string[] bufferString = new string[1];
        bufferString[0] = sentence;
        switch (_currentModule)
        {
            case LSBModule.INTERPRETATION:
                SwitchWebSearch(bufferString);
                break;
            case LSBModule.EVALUATION:
                OnEvaluationResponse.Invoke(sentence);
                break;
            default:
                break;
        }
        OnStopRecording.Invoke("");
    }

    public void OnHypo(string sentence)
    {
        Debug.Log("-------------------------hypothesis");
        Debug.Log(sentence);
    }

    public void OnError(string message)
    {
        Debug.Log("----------------------------------Error");
        Debug.Log(message);
        toolTip.ToolTipText = "Error: " + message;
        mainTextToolTip.SetActive(false);
        OnStopRecording.Invoke("");
    }

    public void OnCancel()
    {
        Debug.Log("________________Cancelled");
        mainTextToolTip.SetActive(false);
        OnStopRecording.Invoke("");
    }

    public void OnSwapToEvaluationModule()
    {
        _currentModule = LSBModule.EVALUATION;
    }

    public void OnSwapToInterpretationMode()
    {
        _currentModule = LSBModule.INTERPRETATION;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
