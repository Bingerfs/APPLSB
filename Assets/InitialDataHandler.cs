using Assets.Util;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public void OnReturningUserSelected(string enteredUserCode)
    {
        _userPreferences.UserId = enteredUserCode.Trim();
        _userPreferences.UserName = "";
        _userPreferences.isGuestUser = false;
    }

    public void OnGuestUserSelected()
    {
        _userPreferences.UserName = null;
        _userPreferences.UserId = null;
        _userPreferences.IsGuestUser = true;
    }

    public void OnNewUserSelected(string enteredUsername)
    {
        _userPreferences.UserName = enteredUsername.Trim().ToUpper();
        _userPreferences.UserId = "";
        _userPreferences.IsGuestUser = false;
    }

    public void OnRightHandednessSelected()
    {
        _userPreferences.PreferredHandedness = Handedness.Right;
    }

    public void OnLeftHandednessSelected()
    {
        _userPreferences.PreferredHandedness = Handedness.Left;
    }

    public async void OnNextSceneLoad()
    {
        await SceneHandler.Instance.TransitionToAnotherScene("MainScene");
    }

    //private async void ListenToSceneTransition(IMixedRealitySceneSystem sceneSystem, ISceneTransitionService transition)
    //{
    //    transition.SetProgressMessage("Starting transition...");

    //    while (transition.TransitionInProgress)
    //    {
    //        if (sceneSystem.SceneOperationInProgress)
    //        {
    //            transition.SetProgressMessage("Loading scene...");
    //            transition.SetProgressValue(sceneSystem.SceneOperationProgress);
    //        }
    //        else
    //        {
    //            transition.SetProgressMessage("Finished loading scene...");
    //            transition.SetProgressValue(1);
    //        }

    //        await Task.Yield();
    //    }
    //}
}
