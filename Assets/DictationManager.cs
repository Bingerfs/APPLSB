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
using Microsoft.MixedReality.Toolkit.Input;

public class DictationManager : MonoBehaviour
{
    [Serializable] public class ResultHandler : UnityEvent<string> { }
    public ResultHandler OnRequest;

    public ResultHandler OnEvaluationResponse;

    public ResultHandler OnStopRecording;

    [SerializeField]
    public GameObject mainTextToolTip;

    [SerializeField]
    private GameObject _microphoneIcon;

    [SerializeField]
    private DictationHandler _dictationHandler;

    private ToolTip toolTip;

    private LSBModule _currentModule = LSBModule.INTERPRETATION;

    [SerializeField]
    private GameObject _noSoundContainer;

    [SerializeField]
    private GameObject _smallDialogPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        toolTip = mainTextToolTip.GetComponent<ToolTip>();
    }

    public void SwitchWebSearch(string[] words)
    {
        String basicText = words[0].ToLower();
        if (OnRequest != null)
            OnRequest.Invoke(basicText);
    }

    public void OnStartRecording()
    {
        if (!_dictationHandler.IsListening)
        {
            Debug.Log("------------------------------Recording started");
            if (_noSoundContainer.activeSelf)
            {
                _noSoundContainer.SetActive(false);
            }

            _microphoneIcon.SetActive(true);
            _dictationHandler.StartRecording();
        }
    }

    public void OnResult(string sentence)
    {
        Debug.Log("------------------------------Result");
        Debug.Log(sentence);
    }

    public void OnComplete(string sentence)
    {
        Debug.Log("-----------------------Complete");
        _microphoneIcon.SetActive(false);
        Debug.Log(sentence);
        if (string.IsNullOrEmpty(sentence))
        {
            StartCoroutine(OnNoSound());
            return;
        }

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
    }

    private IEnumerator OnNoSound()
    {
        _noSoundContainer.SetActive(true);
        yield return new WaitForSeconds(3);
        _noSoundContainer.SetActive(false);
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
        Dialog myDialog = Dialog.Open(_smallDialogPrefab, DialogButtonType.OK, "Error", "Sucedio un error en el sistema de dictado.", true);
        _microphoneIcon.SetActive(false);
        mainTextToolTip.SetActive(false);
        OnStopRecording.Invoke("");
    }

    public void OnCancel()
    {
        Debug.Log("________________Cancelled");
        _microphoneIcon.SetActive(false);
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
