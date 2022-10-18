using Assets.Util;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialDataHandler : MonoBehaviour
{
    [SerializeField]
    private UserPreferences _userPreferences;

    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void OnReturningUserSelected(string userCode)
    {
        _userPreferences.UserId = userCode;
    }

    public void OnGuestUserSelected()
    {
        _userPreferences.UserName = null;
        _userPreferences.UserId = null;
        _userPreferences.IsGuestUser = true;
    }

    public void OnNewUserSelected()
    {
        _userPreferences.UserName = "bingerfs";
    }

    public void OnRightHandednessSelected()
    {
        _userPreferences.PreferredHandedness = Handedness.Right;
    }

    public void OnLeftHandednessSelected()
    {
        _userPreferences.PreferredHandedness = Handedness.Left;
    }

    public void OnNextSceneLoad()
    {
        SceneManager.LoadScene(1);
    }
}
