using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTKGestureVoice
{
    public static class GestureUtils
    {
        private static float flatHandThreshold = 45.0f;

        public static bool IsDoingVoiceRecordingGesture(Handedness trackedHand, IMixedRealityHandJointService HandJointService, Vector3 directionVector)
        {
            var indexTipTransform = HandJointService.RequestJointTransform(TrackedHandJoint.IndexTip, trackedHand);
            var ringTipTransform = HandJointService.RequestJointTransform(TrackedHandJoint.RingTip, trackedHand);
            var palmTransform = HandJointService.RequestJointTransform(TrackedHandJoint.Palm, trackedHand);
            MixedRealityPose indexTipPose = new MixedRealityPose(indexTipTransform.position, indexTipTransform.rotation);
            MixedRealityPose ringTipPose = new MixedRealityPose(ringTipTransform.position, ringTipTransform.rotation);
            MixedRealityPose palmPose = new MixedRealityPose(palmTransform.position, palmTransform.rotation);
            return directionVector == Vector3.up ? IsPalmUpwardsMeetingThresholdRequirements(indexTipPose, ringTipPose, palmPose, trackedHand) : IsPalmForwardsMeetingThresholdRequirements(indexTipPose, ringTipPose, palmPose, trackedHand);
        }

        private static bool IsPalmUpwardsMeetingThresholdRequirements(MixedRealityPose indexTipPose, MixedRealityPose ringTipPose, MixedRealityPose palmPose, Handedness trackedHand)
        {
           
            // Check if the triangle's normal formed from the palm, to index, to ring finger tip roughly matches the palm normal.

            
                var handNormal = Vector3.Cross(indexTipPose.Position - palmPose.Position,
                                                ringTipPose.Position - indexTipPose.Position).normalized;
                handNormal *= (trackedHand == Handedness.Right) ? 1.0f : -1.0f;

                if (Vector3.Angle(palmPose.Up, handNormal) > flatHandThreshold)
                {
                    return false;
                }

            
            float palmCameraAngle = Vector3.Angle(palmPose.Up, CameraCache.Main.transform.forward);
            float facingCameraTrackingThreshold = 90.0f;
            var respuesta = palmCameraAngle <= facingCameraTrackingThreshold && palmCameraAngle >= facingCameraTrackingThreshold - 20.0f && palmPose.Up.y < 0 && palmPose.Up.y < palmPose.Up.x;
            // Check if the palm angle meets the prescribed threshold
            if (respuesta)
            {
                Debug.Log($"UP: {palmPose.Up} SIGN: {Vector3.SignedAngle(palmPose.Up, CameraCache.Main.transform.forward, Vector3.up)} ACTION: RECORD");
            }
            return respuesta;
        }

        private static bool IsPalmForwardsMeetingThresholdRequirements(MixedRealityPose indexTipPose, MixedRealityPose ringTipPose, MixedRealityPose palmPose, Handedness trackedHand)
        {

            // Check if the triangle's normal formed from the palm, to index, to ring finger tip roughly matches the palm normal.


            var handNormal = Vector3.Cross(indexTipPose.Position - palmPose.Position,
                                            ringTipPose.Position - indexTipPose.Position).normalized;
            handNormal *= (trackedHand == Handedness.Right) ? 1.0f : -1.0f;

            if (Vector3.Angle(palmPose.Up, handNormal) > flatHandThreshold)
            {
                return false;
            }


            float palmCameraAngle = Vector3.Angle(palmPose.Up, CameraCache.Main.transform.forward);
            float facingCameraTrackingThreshold = 140.0f;
            var respuesta = palmCameraAngle >= facingCameraTrackingThreshold && palmPose.Up.z < 0 && palmPose.Up.z < palmPose.Up.x && palmPose.Up.z < palmPose.Up.y;
            // Check if the palm angle meets the prescribed threshold
            if (respuesta)
            {
                Debug.Log($"UP: {palmPose.Up} SIGN: {Vector3.SignedAngle(palmPose.Up, CameraCache.Main.transform.forward, Vector3.up)} ACTION: STOP");
            }
            return respuesta;
        }
    }

}