using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float range = 100f;
    [SerializeField] InputActionReference shootAction;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] Animator muzzleFlashAnimator;

    [Header("Crosshair")]
    [SerializeField] GameObject crosshair;
    [SerializeField] float crosshairRange = 10f;
    

    VRDebug _vrDebug;

    void Start()
    {
        _vrDebug = FindObjectOfType<VRDebug>();
        //Subscribe to Input
        shootAction.action.performed += Shoot;
    }

    void OnDestroy()
    {
        //Unsubscribe to Input
        shootAction.action.performed -= Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        Vector3 chPos;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hitInfo, crosshairRange))
            chPos = hitInfo.point;
        else
        {
            chPos = new Vector3(muzzle.position.x, muzzle.position.y, muzzle.position.z);
            chPos += muzzle.forward * crosshairRange;
        }

        crosshair.transform.position = new Vector3(chPos.x, chPos.y, chPos.z);
        crosshair.transform.LookAt(muzzle);
    }

    void Shoot(InputAction.CallbackContext ctx)
    {
        muzzleFlashAnimator.SetTrigger("isFiring");
        RaycastHit hitInfo;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hitInfo, range))
        {
            Destructable destructable = hitInfo.transform.GetComponent<Destructable>();
            if (destructable != null)
            {
                destructable.Damage(damage);
                _vrDebug.Log("Hit: " + hitInfo.transform.name);
            }
            Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }

    }
}
