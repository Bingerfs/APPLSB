using Assets.Util;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour, IDataPersistence
{
    private float gainedExperience;

    [SerializeField]
    private GameObject slatePrefab;

    [SerializeField]
    private UserPreferences _userPreferences;

    public void OnExperienceIncrement(float increment)
    {
        gainedExperience = gainedExperience + increment;
    }

    public void LoadUserData(UserData userData)
    {
        gainedExperience = userData.userExperience;
    }

    public void SaveUserData(ref UserData userData)
    {
        userData.userExperience = gainedExperience;
    }

    public void OnSlateSpawned()
    {
        var cameraTransform = CameraCache.Main.transform;
        var cameraPosition = cameraTransform.position;
        switch (_userPreferences.PreferredHandedness)
        {
            case Handedness.Left:
                cameraPosition.x = cameraPosition.x - 1.5f;
                break;
            case Handedness.Right:
                cameraPosition.x = cameraPosition.x + 1.5f;
                break;
            default:
                break;
        }

        cameraPosition.z = cameraPosition.z + 1.5f;
        slatePrefab.transform.position = cameraPosition;
        var directionToTarget = cameraTransform.position - slatePrefab.transform.position;
        slatePrefab.transform.SetPositionAndRotation(cameraPosition, Quaternion.LookRotation(-directionToTarget));
        var progressSlateComponent = slatePrefab.GetComponent<ProgressSlate>();
        progressSlateComponent.Progress = ProgressLevelUtil.GetPercentageToNextLevel(gainedExperience);
        progressSlateComponent.Username = _userPreferences.UserName;
        progressSlateComponent.UserId = _userPreferences.UserId;
        progressSlateComponent.UserLevel = ProgressLevelUtil.GetLevel(gainedExperience);
        if (!slatePrefab.activeSelf)
        {
            slatePrefab.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
