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

    private string _username;

    public string Username { get => _username; set => _username = value; }

    private string _userId;

    public string UserId { get => _userId; set => _userId = value; }

    async void Start()
    {
        var indicator = _progressBar.GetComponent<ProgressIndicatorLoadingBar>();
        if (indicator != null)
        {
            indicator.Progress = _progress;
            await indicator.OpenAsync();
        }

        var userTextMesh = _userInfoText.GetComponent<TextMeshPro>();
        if (userTextMesh != null)
        {
            userTextMesh.SetText($"{(_username ?? "").ToUpper()}\n#{_userId}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
