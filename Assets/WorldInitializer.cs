using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInitializer : MonoBehaviour
{
    private bool _isSceneAlreadyChanged = false;
    // Start is called before the first frame update

    async void Start()
    {

    }

    // Update is called once per frame
    async void Update()
    {
        if (!_isSceneAlreadyChanged)
        {
            _isSceneAlreadyChanged = true;
            IMixedRealitySceneSystem sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
            await sceneSystem.LoadContent("MainMenuScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
