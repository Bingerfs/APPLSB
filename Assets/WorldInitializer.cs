using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Extensions.HandPhysics;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldInitializer : MonoBehaviour
{
    private bool _hasPlatonicBeenTouched = false;

    public bool HasPlatonicBeenTouched { get => _hasPlatonicBeenTouched; set => _hasPlatonicBeenTouched = value; }

    [SerializeField]
    private Transform _startAppPlatonicTransform;

    [SerializeField]
    private GameObject _uiElements;

    [SerializeField]
    private TextMeshPro _debugText;

    private Renderer _targetRenderer;

    private Color _originalColor;

    protected float _duration = 1.5f;

    protected float t = 0;

    private float _timeOfPlatonicTouch = 0;

    [SerializeField]
    private Transform _ucbBrochureTransform;

    [SerializeField]
    private Transform _lsbBrochureTransform;

    private bool disableTumbleUcb = false;

    private bool disableTumbleLsb = false;

    private bool _isWindowActive = false;

    public bool IsWindowActive { get => _isWindowActive; set => _isWindowActive = value; }

    private void Awake()
    {
    }

    async void Start()
    {
        _targetRenderer = _startAppPlatonicTransform.GetComponent<Renderer>();
        if (_targetRenderer != null)
        {
            _originalColor = _targetRenderer.material.color;
        }
    }

    async void Update()
    {
        if (_hasPlatonicBeenTouched)
        {
            _timeOfPlatonicTouch = _timeOfPlatonicTouch == 0f ? Time.unscaledTime : _timeOfPlatonicTouch;
            if ((_targetRenderer != null) && (_targetRenderer.material != null))
            {
                _targetRenderer.material.color = Color.Lerp(Color.blue, Color.magenta, t);
                t = Mathf.PingPong(Time.time, _duration) / _duration;
            }
        }

        if (_hasPlatonicBeenTouched && Time.unscaledTime - _timeOfPlatonicTouch >= 5f)
        {
            _hasPlatonicBeenTouched = false;
            _timeOfPlatonicTouch = 0;
            if ((_targetRenderer != null) && (_targetRenderer.material != null))
            {
                _targetRenderer.material.color = _originalColor;
            }

            _uiElements.SetActive(false);
            gameObject.SetActive(false);
            await SceneHandler.Instance.TransitionToAnotherScene("MainMenuScene");
        }
    }

    public void OnWebPageLaunched(string url)
    {
        if (!_isWindowActive)
        {
            _isWindowActive = true;
            UnityEngine.WSA.Launcher.LaunchUri(url, false);
        }
    }

    public void OnTumbleBrochure(int brochure)
    {
        if (!disableTumbleUcb && brochure == 0)
        {
            disableTumbleUcb = true;
            var position = _ucbBrochureTransform.position;
            position.z = 0.011f;
            _ucbBrochureTransform.position = position;
        }

        if (!disableTumbleLsb && brochure == 1)
        {
            disableTumbleLsb = true;
            var position = _lsbBrochureTransform.position;
            position.z = 0.01f;
            _lsbBrochureTransform.position = position;
        }
    }
}
