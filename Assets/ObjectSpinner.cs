using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpinner : MonoBehaviour
{
    [SerializeField]
    private Transform _targetObjectTransform;

    [SerializeField]
    private float _angularSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetObjectTransform != null)
        {
            _targetObjectTransform.Rotate(Vector3.up * (_angularSpeed * Time.deltaTime));
        }
    }
}
