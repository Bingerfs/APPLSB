using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTKGestureVoice
{
    public static class GestureUtils
    {

        public static bool IsDoingStartVoiceRecordingGesture(Handedness trackedHand)
        {
            return HandPoseUtils.IsThumbGrabbing(trackedHand);
        }
    }

}