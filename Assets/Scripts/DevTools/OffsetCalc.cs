using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OffsetCalc : MonoBehaviour
{
   [SerializeField] Transform _from;
   [SerializeField] Transform _to;

   [SerializeField] InputActionReference triggerInput;

   void Awake()
   {
      triggerInput.action.performed += GetOffset;
   }

   void GetOffset(InputAction.CallbackContext ctx)
   {
      Vector3 positionOffset = _from.position - _to.position;
      Quaternion rotationOffset = Quaternion.Inverse(_from.rotation) * _to.rotation;
      
      Debug.Log("Position: " + positionOffset);
      Debug.Log("Rotation: " + rotationOffset);

   }
}
