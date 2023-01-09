using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHand : MonoBehaviour
{
    [SerializeField] Transform controller;
    [SerializeField] Transform handTarget;
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!controller)
            Debug.LogWarning("Controller GameObjects not Assigned in to IKHand in Inspector");
    }

    // Update is called once per frame
    void Update()
    {
        handTarget.position = controller.TransformPoint(positionOffset);
        handTarget.rotation = controller.rotation * Quaternion.Euler(rotationOffset);
    }
}
