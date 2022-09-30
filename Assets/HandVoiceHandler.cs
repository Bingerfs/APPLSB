using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using MRTKGestureVoice;
using UnityEngine;
using System;
using UnityEngine.Events;

public class HandVoiceHandler : MonoBehaviour
{
    [Serializable] public class ResultHandler : UnityEvent<string> { }
    public ResultHandler OnRecord;

    [Serializable] public class LoadHandler : UnityEvent { }
    public LoadHandler OnProcessing;

    [Serializable] public class CancelHandler : UnityEvent { }
    public CancelHandler OnCancel;

    [SerializeField]
    private TrackedHandJoint trackedHandJoint = TrackedHandJoint.Palm;

    [SerializeField]
    private Handedness trackedHand = Handedness.Right;

    private IMixedRealityHandJointService _handJointService;

    private IMixedRealityHandJointService HandJointService =>
        _handJointService ??
        (_handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

    private MixedRealityPose? previousHandPose;

    private Vector3? previousDirectionVector = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var handPose = GetHandPose(trackedHand);
        ProcessPoseChange(previousHandPose, handPose);
    }

    private Vector3? GetHandPose(Handedness hand)
    {
        if ((trackedHand & hand) == hand)
        {
            if (HandJointService.IsHandTracked(hand))
            {
                if (GestureUtils.IsDoingVoiceRecordingGesture(hand, HandJointService, Vector3.up))
                {
                    return Vector3.up;
                }
                else
                {
                    if (GestureUtils.IsDoingVoiceRecordingGesture(hand, HandJointService, Vector3.down))
                    {
                        return Vector3.down;
                    }
                }
            }
        }

        return null;
    }

    private void ProcessPoseChange(MixedRealityPose? previousPose, Vector3? currentDirection)
    {
        if (previousDirectionVector == currentDirection)
        {
            return;
        }
        else
        {
            previousDirectionVector = currentDirection;
            if (Vector3.up == currentDirection)
            {
                Debug.Log("OnRecord through gesture");
                OnRecord.Invoke("");
                OnProcessing.Invoke();
            }

            if (Vector3.down == currentDirection)
            {
                Debug.Log("OnStop through gesture");
                OnCancel.Invoke();
            }
            
        }
    }
}
