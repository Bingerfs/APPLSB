using Assets.Util;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//[ExecuteInEditMode]
public class WordProgressContainer : MonoBehaviour
{
    [SerializeField]
    private UserExpresssionProgress _wordProgressData;

    public UserExpresssionProgress WordProgressData { get => _wordProgressData; set => _wordProgressData = value; }

    [SerializeField]
    private TextMeshPro _signNameText;

    [SerializeField]
    private TextMeshPro _progressRatioText;

    [SerializeField]
    private WordProgressIndicatorBar _progressBar;

    private float? _percentageRatio;

    public float? PercentageRatio { get => _percentageRatio; set => _percentageRatio = value; }

    // Start is called before the first frame update
    void Start()
    {
        _signNameText.SetText(WordProgressData.word);
        if (_percentageRatio != null)
        {
            _progressRatioText.SetText($"{Mathf.RoundToInt(_percentageRatio.Value * 100)}%");
            if (_progressBar != null)
            {
                _progressBar.PercentageProgress = _percentageRatio.Value;
            }
        }
        else
        {
            _progressRatioText.SetText($"{WordProgressData.totalCorrectResponses}/{WordProgressData.totalResponses}");
            if (_progressBar != null)
            {
                float percentageSuccessTries = WordProgressData.totalCorrectResponses == 0 ? 0 : (float)WordProgressData.totalCorrectResponses / (float)WordProgressData.totalResponses;
                _progressBar.PercentageProgress = percentageSuccessTries;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
