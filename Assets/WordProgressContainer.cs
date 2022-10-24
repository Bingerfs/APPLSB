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

    // Start is called before the first frame update
    void Start()
    {
        _signNameText.SetText(WordProgressData.wordCode);
        _progressRatioText.SetText($"{WordProgressData.totalCorrectResponses}/{WordProgressData.totalResponses}");
        if (_progressBar != null)
        {
            float percentageSuccessTries = (float)WordProgressData.totalCorrectResponses / (float)WordProgressData.totalResponses;
            _progressBar.PercentageProgress = percentageSuccessTries;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
