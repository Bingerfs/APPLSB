using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Assets.Util;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.WSA;

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

    private bool registered = true;

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("focused");
        if (focus)
        {
            UnRegister();
            Register();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("paused");
        if (!pause)
        {
            UnRegister();
            Register();
        }
    }

    private void UnRegister()
    {
        if (registered)
        {
            if (CoreServices.InputSystem != null)
            {
                CoreServices.InputSystem.UnregisterHandler<IMixedRealityDictationHandler>(_dictationHandler);
            }

            registered = false;
        }
        else
        {
            Debug.LogError("Keyword listener already unregistered");
        }
    }

    private void Register()
    {
        if (!registered)
        {
            registered = true;
            CoreServices.InputSystem.RegisterHandler<IMixedRealityDictationHandler>(_dictationHandler);
            UnityEngine.Windows.Speech.PhraseRecognitionSystem.Shutdown();
            UnityEngine.Windows.Speech.PhraseRecognitionSystem.Restart();
            CoreServices.ResetCacheReferences();
            var dict = CoreServices.GetInputSystemDataProvider<IMixedRealityDictationSystem>();
            dict.Reset();
        }
        else
        {
            Debug.LogError("Keyword listener already registered");
        }
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
        Debug.Log("------------------------------Gesture activated. Current status");
        if (!_dictationHandler.IsListening)
        {
            if (_noSoundContainer.activeSelf)
            {
                _noSoundContainer.SetActive(false);
            }

            _dictationHandler.StartRecording();
            if (_dictationHandler.IsListening)
            {
                _microphoneIcon.SetActive(true);
                Debug.Log("------------------------------Recording started");
            }
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
        Dialog myDialog = Dialog.Open(_smallDialogPrefab, DialogButtonType.OK, "Error", $"Sucedio un error en el sistema de dictado: {message}", true);
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
}
