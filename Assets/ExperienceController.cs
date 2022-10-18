using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour, IDataPersistence
{
    private float gainedExperience;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
