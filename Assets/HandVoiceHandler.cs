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

    [SerializeField]
    private TrackedHandJoint trackedHandJoint = TrackedHandJoint.ThumbTip;

    [SerializeField]
    private Handedness trackedHand = Handedness.Right;

    private IMixedRealityHandJointService _handJointService;

    private IMixedRealityHandJointService HandJointService =>
        _handJointService ??
        (_handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

    private MixedRealityPose? previousHandPose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var handPose = GetHandPose(trackedHand, previousHandPose != null);
        if (handPose != null)
        {
            ProcessPoseChange(previousHandPose, handPose);
        }
    }

    private MixedRealityPose? GetHandPose(Handedness hand, bool hasBeenDoneAlready)
    {
        if ((trackedHand & hand) == hand)
        {
            if (HandJointService.IsHandTracked(hand) && GestureUtils.IsDoingStartVoiceRecordingGesture(hand) && !hasBeenDoneAlready)
            {
                var jointTransform = HandJointService.RequestJointTransform(trackedHandJoint, hand);
                return new MixedRealityPose(jointTransform.position);
            }
        }

        return null;
    }

    private void ProcessPoseChange(MixedRealityPose? previousPose, MixedRealityPose? currentPose)
    {
        if (previousPose == currentPose)
        {
            return;
        }
        else
        {
            Debug.Log("OnRecord through gesture");
            OnRecord.Invoke("");
        }
    }
}
