using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine.Events;
using System.Threading.Tasks;

public class handHandler : MonoBehaviour, IMixedRealityPointerHandler
{
    [Serializable] public class ResultHandler : UnityEvent<string> { }
    public ResultHandler onRecord;
    Color colorBlue = Color.blue;
    Renderer rend;
    GameObject cube;

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }
    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("OnPointerClicked");
        onRecord.Invoke("");
    }

    public void OnPointerEnter(Vector3 cubePosition)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = Vector3.one * 0.1f;
        cube.transform.position = cubePosition;
    }


    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        rend.material.color = Color.red;

    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        rend.material.color = Color.yellow;
        Vector3 v = new Vector3(
                           eventData.Pointer.Position.x,
                           eventData.Pointer.Position.y,
                           eventData.Pointer.Position.z);
        rend.transform.position = v;

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        rend.material.color = Color.green;


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rend = GetComponent<Renderer>();
    }

}
