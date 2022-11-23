using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignImagePlacer : MonoBehaviour
{
    [SerializeField]
    private bool isPlaceEnabled = false;

    async void Update()
    {
        if (isPlaceEnabled)
        {
            isPlaceEnabled = false;
            if (!this.positionAttempted)
            {
                this.positionAttempted = true;



                var canCompute = await SceneUnderstandingHelper.CanComputeAsync();

                if (canCompute)
                {
                    var parent = await SceneUnderstandingHelper.ParentGameObjectOnLargestPlatformAsync(this.gameObject);

                    // Not yet sure whether I should be checking the orientation of the platform
                    // that we have found here and then rotating based upon it but, for the moment
                    // I'm going to say that this model should face the user and should not be rotated
                    // around x,z so that it (hopefully) sits flat on the platform in question.
                    var lookPos = CameraCache.Main.transform.position;
                    lookPos.y = this.gameObject.transform.position.y;

                    this.gameObject.transform.LookAt(lookPos);
                }

            }
        }
    }
    bool positionAttempted = false;
}
