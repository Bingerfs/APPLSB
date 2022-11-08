using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    private IMixedRealitySceneSystem _sceneSystem;

    private ISceneTransitionService _transitionService;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one instance of Scene handler.");
        }

        _sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        _transitionService = MixedRealityToolkit.Instance.GetService<ISceneTransitionService>();
        if (_sceneSystem == null)
        {
            Debug.LogError("No Scene System was found.");
        }

        if (_transitionService == null)
        {
            Debug.LogError("No Scene Transition Service was found.");
        }

        Instance = this;
    }

    public async Task TransitionToAnotherScene(string sceneName, List<Action> actionsThroughtTransition = null)
    {
        actionsThroughtTransition = actionsThroughtTransition == null ? new List<Action> { } : actionsThroughtTransition;
        if (!_transitionService.TransitionInProgress)
        {
            await _transitionService.DoSceneTransition(
                () => {
                    foreach (var action in actionsThroughtTransition)
                    {
                        action();
                    }

                    return _sceneSystem.LoadContent(sceneName, LoadSceneMode.Single);
                }
            );
        }
    }
}
