using Assets.Util;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//[ExecuteInEditMode]
public class CategoryProgressContainer : MonoBehaviour
{
    [SerializeField]
    public string _categoryName;

    public string CategoryName { get => _categoryName; set => _categoryName = value; }

    [SerializeField]
    public TextMeshPro _categoryNameText;

    [SerializeField]
    private List<UserExpresssionProgress> _expressionsProgress;

    public List<UserExpresssionProgress> ExpressionsProgress { get => _expressionsProgress; set => _expressionsProgress = value; }

    [SerializeField]
    private Transform _objectCollectionTransform;

    [SerializeField]
    private GameObject _wordProgressPrefab;

    [SerializeField]
    private GameObject _scrollButtons;

    [SerializeField]
    private bool _areScrollButtonsActive;
    void Start()
    {
        _categoryNameText.SetText(_categoryName);
        if (_expressionsProgress != null && _expressionsProgress.Any() && _objectCollectionTransform != null && _wordProgressPrefab != null)
        {
            foreach (var expressionProgress in _expressionsProgress)
            {
                var prefabInstance = Instantiate(_wordProgressPrefab, _objectCollectionTransform);
                var progresScript = prefabInstance.GetComponent<WordProgressContainer>();
                progresScript.WordProgressData = expressionProgress;
            }

            var gridObjectCollection = _objectCollectionTransform.gameObject.GetComponent<GridObjectCollection>();
            gridObjectCollection.UpdateCollection();
        }

        _scrollButtons.SetActive(_areScrollButtonsActive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
