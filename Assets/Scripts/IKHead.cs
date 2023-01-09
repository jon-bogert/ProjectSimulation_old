using System.Collections;
using System.Collections.Generic;
using Unity.XR.GoogleVr;
using UnityEngine;

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

    void LateUpdate()
    {
        transform.position = headset.position;
        transform.forward = Vector3.ProjectOnPlane(headset.up, Vector3.up).normalized;


        // rootObject.position = followObject.position + headBodyOffset;
        // rootObject.forward = Vector3.ProjectOnPlane(followObject.up, Vector3.up).normalized;

        // transform.position = followObject.TransformPoint(positionOffset);
        // transform.rotation = followObject.rotation * Quaternion.Euler(rotationOffset);
    }
}
