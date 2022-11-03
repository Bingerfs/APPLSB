using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoSpinner : MonoBehaviour
{
    [SerializeField]
    private Transform _lsbLogoTransform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_lsbLogoTransform != null)
        {
            _lsbLogoTransform.Rotate(Vector3.forward * (50.0f * Time.deltaTime));
        }
    }
}
