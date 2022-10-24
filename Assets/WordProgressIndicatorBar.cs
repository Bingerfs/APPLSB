using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordProgressIndicatorBar : MonoBehaviour
{
    [Range(0f, 1f)]
    private float _progress = 0;

    private float _previousProgress;

    public float PercentageProgress { get => _progress; set => _progress = value; }

    [SerializeField]
    private Transform _progressBar;

    void Start()
    {
        if (_progressBar != null)
        {
            var updatedProgressTransform = Vector3.one;
            updatedProgressTransform.x = _progress;
            _progressBar.localScale = updatedProgressTransform;
            _previousProgress = _progress;
        }
    }

    void Update()
    {
        if (_previousProgress != _progress)
        {
            var updatedProgressTransform = Vector3.one;
            updatedProgressTransform.x = _progress;
            _progressBar.localScale = updatedProgressTransform;
            _previousProgress = _progress;
        }
    }
}
