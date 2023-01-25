using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardSlate : MonoBehaviour
{
    [SerializeField]
    private List<Tuple<string, float>> _leaderBoardUsers;

    public List<Tuple<string, float>> LeaderBoardUsers { get => _leaderBoardUsers; set => _leaderBoardUsers = value; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
