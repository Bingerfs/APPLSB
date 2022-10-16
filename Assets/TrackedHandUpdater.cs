using Assets.Util;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedHandUpdater : MonoBehaviour
{
    [SerializeField]
    private UserPreferences _userPreferences;

    private SolverHandler _solverHandler;

    private Handedness _previousHandedness;
    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<SolverHandler>(out _solverHandler))
        {
            _solverHandler.TrackedHandedness = DefineHandedness().GetValueOrDefault();
        }
        else
        {
            Debug.Log("No solver handler found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_solverHandler != null)
        {
            var definedHandedness = DefineHandedness().GetValueOrDefault();
            if (_solverHandler.TrackedHandedness != definedHandedness)
            {
                _solverHandler.TrackedHandedness = definedHandedness;
            }
        }
        else
        {
            try
            {
                _solverHandler = GetComponent<SolverHandler>();
            }
            catch (System.Exception)
            {
                Debug.Log("The has been an error while retrieving the solver handler");
            }

            Debug.Log("No solver instantiated");
        }
    }

    private Handedness? DefineHandedness()
    {
        Handedness? definedHandedness = null;
        switch (_userPreferences.PreferredHandedness)
        {
            case Microsoft.MixedReality.Toolkit.Utilities.Handedness.Left:
                definedHandedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;
                _previousHandedness = definedHandedness.GetValueOrDefault();
                break;
            case Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right:
                definedHandedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Left;
                _previousHandedness = definedHandedness.GetValueOrDefault();
                break;
            default:
                break;
        }

        return definedHandedness;
    }
}
