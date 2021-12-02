using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using FantomLib;
using UnityEngine.Events;
using UnityEngine.Android;

public class DictationManager : MonoBehaviour
{
    [Serializable] public class ResultHandler : UnityEvent<string> { }
    public ResultHandler OnRequest;

    // Start is called before the first frame update
    void Start()
    {
        RequestUserMicrophonePermission();
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
    }

    public void OnResult(string sentence)
    {
        Debug.Log("------------------------------Result");
        Debug.Log(sentence);
    }

    public void OnComplete(string sentence)
    {
        Debug.Log("-----------------------Complete");
        Debug.Log(sentence);
        string[] bufferString = new string[1];
        bufferString[0] = sentence;
        SwitchWebSearch(bufferString);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
