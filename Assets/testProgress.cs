using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testProgress : MonoBehaviour
{
    [SerializeField]
    private GameObject progress;

    private IProgressIndicator progresscomp;

    private bool first = false;
    void Start()
    {
        progresscomp = progress.GetComponent<IProgressIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!first)
        {
            first = true;
            progresscomp.OpenAsync();
        }
    }
}
