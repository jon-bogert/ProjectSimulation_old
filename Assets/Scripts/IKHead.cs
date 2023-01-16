using System.Collections;
using System.Collections.Generic;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IKHead : MonoBehaviour
{
    // [SerializeField] Transform rootObject;
    // [SerializeField] Transform followObject;
    // [SerializeField] Vector3 positionOffset;
    // [SerializeField] Vector3 rotationOffset;
    //
    // [SerializeField] Vector3 headBodyOffset;

    [SerializeField] Transform headTarget;
    [SerializeField] Transform headset;
    [SerializeField] Vector3 positionOffset;

    void LateUpdate()
    {
        //transform.position = headset.TransformPoint(positionOffset);
        transform.position = headset.position;
        Vector3 newForward = Vector3.ProjectOnPlane(headset.up, Vector3.up).normalized;
        if (newForward != Vector3.zero)
        {
            transform.forward = newForward;
        }
        
        // rootObject.position = followObject.position + headBodyOffset;
        // rootObject.forward = Vector3.ProjectOnPlane(followObject.up, Vector3.up).normalized;

        // transform.position = followObject.TransformPoint(positionOffset);
        // transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);
    }
}
