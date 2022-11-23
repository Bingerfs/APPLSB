using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public static class LookingDirectionHelpers
{
    private static int _meshPhysicsLayer = 0;

    public static Vector3? GetPositionOnSpatialMap(float maxDistance = 2)
    {
        RaycastHit hitInfo;
        var transform = CameraCache.Main.transform;
        var headRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(headRay, out hitInfo, maxDistance, GetSpatialMeshMask()))
        {
            return hitInfo.point;
        }
        return null;
    }

    private static int GetSpatialMeshMask()
    {
        if (_meshPhysicsLayer == 0)
        {
            var spatialMappingConfig = CoreServices.SpatialAwarenessSystem.ConfigurationProfile as
                MixedRealitySpatialAwarenessSystemProfile;
            if (spatialMappingConfig != null)
            {
                foreach (var config in spatialMappingConfig.ObserverConfigurations)
                {
                    var observerProfile = config.ObserverProfile
                        as MixedRealitySpatialAwarenessMeshObserverProfile;
                    if (observerProfile != null)
                    {
                        _meshPhysicsLayer |= (1 << observerProfile.MeshPhysicsLayer);
                    }
                }
            }
        }

        return _meshPhysicsLayer;
    }
}
