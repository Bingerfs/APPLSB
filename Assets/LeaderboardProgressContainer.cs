using Assets.Util;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardProgressContainer : MonoBehaviour
{
    [SerializeField]
    public string _categoryName;

    public string CategoryName { get => _categoryName; set => _categoryName = value; }

    [SerializeField]
    public TextMeshPro _categoryNameText;

    [SerializeField]
    private List<Tuple<string, float>> _leaderBoardUsers;

    public List<Tuple<string, float>> LeaderBoardUsers { get => _leaderBoardUsers; set => _leaderBoardUsers = value; }

    [SerializeField]
    private Transform _objectCollectionTransform;

    [SerializeField]
    private GameObject _wordProgressPrefab;

    [SerializeField]
    private ScrollingObjectCollection scrollObject;

    private bool requireItemsUpdate = false;

    private GridObjectCollection gridObjectCollection = null;

    void Start()
    {
        _leaderBoardUsers= _leaderBoardUsers != null ? _leaderBoardUsers : new List<Tuple<string, float>>();
        _leaderBoardUsers.Add(new Tuple<string, float>("Hector", 0.5f));
        _leaderBoardUsers.Add(new Tuple<string, float>("Hect", 0.3f));
        _leaderBoardUsers.Add(new Tuple<string, float>("Heor", 0.1f));
        _categoryNameText.SetText(_categoryName);
        if (_leaderBoardUsers != null && _leaderBoardUsers.Any() && _objectCollectionTransform != null && _wordProgressPrefab != null)
        {
            int counter = 0;
            foreach (var user in _leaderBoardUsers)
            {
                counter++;
                var prefabInstance = Instantiate(_wordProgressPrefab, _objectCollectionTransform);
                var progresScript = prefabInstance.GetComponent<WordProgressContainer>();
                var tempProgress = new UserExpresssionProgress();
                tempProgress.word = $"{counter}. {user.Item1}";
                progresScript.PercentageRatio = user.Item2;
                progresScript.WordProgressData = tempProgress;
            }

            gridObjectCollection = _objectCollectionTransform.gameObject.GetComponent<GridObjectCollection>();
            gridObjectCollection.UpdateCollection();
            requireItemsUpdate = true;
        }
    }

    void Update()
    {
        if (requireItemsUpdate)
        {
            requireItemsUpdate = false;
            scrollObject.UpdateContent();
        }
    }
}
