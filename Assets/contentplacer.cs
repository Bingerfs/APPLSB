using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contentplacer : MonoBehaviour
{
    Vector3? foundPosition = null;

    [SerializeField]
    private bool isPlaced = false;

    public bool IsPlaced { get => isPlaced; set => isPlaced = value; }

    [SerializeField]
    public GameObject _indicatingSurfaceArrows = null;

    [SerializeField]
    public GameObject _mainContent = null;

    private void Update()
    {
        CheckLocationOnSpatialMap();
    }

    private void CheckLocationOnSpatialMap()
    {
        foundPosition = LookingDirectionHelpers.GetPositionOnSpatialMap(3.0f);
        if (isPlaced)
        {
            isPlaced = false;
            var newPosition = _mainContent.transform.position;
            newPosition.x = foundPosition.Value.x;
            newPosition.z = foundPosition.Value.z;
            var cameraTransform = CameraCache.Main.transform;
            var directionToTarget = cameraTransform.position - newPosition;
            directionToTarget.y = 0f;
            var newRotation = Quaternion.LookRotation(-directionToTarget);
            _mainContent.transform.SetPositionAndRotation(newPosition, newRotation);
            _mainContent.SetActive(true);
            gameObject.SetActive(false);
        }

        if (foundPosition != null)
        {
            if (CameraCache.Main.transform.position.y - foundPosition.Value.y > 1f)
            {
                _indicatingSurfaceArrows.transform.position = foundPosition.Value;
                _indicatingSurfaceArrows.SetActive(true);
            }
            else
            {
                foundPosition = null;
            }
        }

    }
}
