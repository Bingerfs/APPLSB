using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        IMixedRealitySceneSystem sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
        await sceneSystem.LoadContent("MainMenuScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
