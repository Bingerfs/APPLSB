using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressSlate : MonoBehaviour
{
    [SerializeField]
    private GameObject _progressBar;

    [SerializeField]
    private GameObject _userInfoText;

    private float _progress;

    public float Progress { get => _progress; set => _progress = value; }

    private float _previousProgress;

    private string _username;

    public string Username { get => _username; set => _username = value; }

    private string _userId;

    public string UserId { get => _userId; set => _userId = value; }

    void Start()
    {
        var indicator = _progressBar.GetComponent<ProgressIndicatorLoadingBar>();
        if (indicator != null)
        {
            _previousProgress = Progress;
            indicator.Progress = _progress;
        }

        var userTextMesh = _userInfoText.GetComponent<TextMeshPro>();
        if (userTextMesh != null)
        {
            userTextMesh.SetText($"{(_username ?? "").ToUpper()}\n#{_userId}");
        }
    }

    void Update()
    {
        if (_previousProgress != Progress)
        {
            _previousProgress = Progress;
            var indicator = _progressBar.GetComponent<ProgressIndicatorLoadingBar>();
            indicator.Progress = Progress;
        }
    }

    private async void OnEnable()
    {
        if (gameObject.activeSelf)
        {
            var indicator = _progressBar.GetComponent<ProgressIndicatorLoadingBar>();
            await indicator.OpenAsync();
        }
    }

    private async void OnDisable()
    {
        if (!gameObject.activeSelf)
        {
            var indicator = _progressBar.GetComponent<ProgressIndicatorLoadingBar>();
            await indicator.CloseAsync();
        }
    }
}
