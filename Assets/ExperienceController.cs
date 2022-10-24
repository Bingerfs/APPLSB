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

    private GameObject _previosInstantiated;

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
        if (_previosInstantiated != null)
        {
            Destroy(_previosInstantiated);
            _previosInstantiated = null;
        }


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
        var directionToTarget = cameraTransform.position - transform.position;
        _previosInstantiated = Instantiate(slatePrefab, cameraPosition, Quaternion.LookRotation(-directionToTarget));
        var progressSlateComponent = _previosInstantiated.GetComponent<ProgressSlate>();
        progressSlateComponent.Progress = ProgressLevelUtil.GetPercentageToNextLevel(gainedExperience);
        progressSlateComponent.UserId = _userPreferences.UserId;
        progressSlateComponent.Username = _userPreferences.UserName;
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
