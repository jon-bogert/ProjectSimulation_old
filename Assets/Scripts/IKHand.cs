using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHand : MonoBehaviour
{
    enum HandName
    {
        Left,
        Right
    };
    [SerializeField] Transform controller;
    [SerializeField] Transform handTarget;
    [SerializeField] Vector3 positionOffset;
    [SerializeField] HandName hand = HandName.Right;
    //[SerializeField] Quaternion rotationOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!controller)
            Debug.LogWarning("Controller GameObjects not Assigned in to IKHand in Inspector");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        handTarget.position = controller.TransformPoint(positionOffset);
        //handTarget.rotation = Quaternion.Euler(controller.rotation.eulerAngles + rotationOffset);
        
        if (hand == HandName.Right)
        {
            handTarget.rotation = controller.rotation * new Quaternion(0.51776f, -0.51840f, -0.50908f, 0.45169f);
        }
        else
        {
            handTarget.rotation = controller.rotation * new Quaternion(-0.516667f, -0.513858f, -0.558656f, -0.35390f);
        }
    }
}
