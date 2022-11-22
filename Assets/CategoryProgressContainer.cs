using Assets.Util;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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

    private bool requireItemsUpdate = false;

    private GridObjectCollection gridObjectCollection = null;

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

            gridObjectCollection = _objectCollectionTransform.gameObject.GetComponent<GridObjectCollection>();
            requireItemsUpdate = true;
        }
    }

    void Update()
    {
        if (requireItemsUpdate)
        {
            requireItemsUpdate = false;
            gridObjectCollection.UpdateCollection();
        }
    }
}
