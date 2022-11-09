using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressSlate : MonoBehaviour
{
    [SerializeField]
    private GameObject _progressBar;

    [SerializeField]
    private Renderer _targetBarRenderer;

    [SerializeField]
    private GameObject _userInfoText;

    [SerializeField]
    private TextMeshPro _levelText;

    [SerializeField]
    private Renderer _targetLevelRenderer;

    private float _progress;

    public float Progress { get => _progress; set => _progress = value; }

    private float _previousProgress;

    private string _username;

    public string Username { get => _username; set => _username = value; }

    private string _userId;

    public string UserId { get => _userId; set => _userId = value; }

    private int _userLevel;

    public int UserLevel { get => _userLevel; set => _userLevel = value; }

    private bool _hasBarBeenTouched;

    public bool HasBarBeenTouched { get => _hasBarBeenTouched; set => _hasBarBeenTouched = value;}

    private Color _originalBarColor;

    private Color _originalLevelTextColor;

    private float _timeOfTouch;

    private float t;

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

        if (_levelText != null)
        {
            _levelText.SetText($"<color=#E3B134>{_userLevel}");
        }

        if (_targetBarRenderer != null)
        {
            _originalBarColor = _targetBarRenderer.material.color;
        }

        if (_targetLevelRenderer != null)
        {
            //_originalLevelTextColor = _targetLevelRenderer.material.color;
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

        if (_hasBarBeenTouched)
        {
            _timeOfTouch = _timeOfTouch == 0f ? Time.unscaledTime : _timeOfTouch;
            var newColor = GetColor((int)Mathf.PingPong(Time.time, 3));
            _targetBarRenderer.material.color = newColor;
            //_targetLevelRenderer.material.color = newColor;
        }

        if (_hasBarBeenTouched && Time.unscaledTime - _timeOfTouch >= 3f)
        {
            _hasBarBeenTouched = false;
            _timeOfTouch = 0;
            _targetBarRenderer.material.color = _originalBarColor;
            //_targetLevelRenderer.material.color = _originalLevelTextColor;
        }
    }

    private Color GetColor(int index)
    {
        var firstColor = new Color(227f / 255f, 177f / 255f, 52f / 255f);
        var secondColor = new Color(52f / 255f, 236f / 255f, 141f / 255f);
        var thirdColor = new Color(236f / 255f, 52f / 255f, 148f / 255f);
        var fourthColor = new Color(52f / 255f, 102f / 255f, 227f / 255f);
        var list = new List<Color> { firstColor, secondColor, thirdColor, fourthColor };
        return list[index];
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
