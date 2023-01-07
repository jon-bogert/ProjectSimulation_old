using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    bool canClimb = false;
    Vector3 prevPos;

    Climbable climbable;

    WeaponMode _weaponModeBuffer = WeaponMode.None;
    GadgetMode _gadgetModeBuffer = GadgetMode.None;
    
    //references
    SphereCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        prevPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _collider = GetComponent<SphereCollider>();
        if (!_collider)
        {
            Debug.LogError("No valid Collider on Hand Object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        prevPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Climbable")
        {
            canClimb = true;
            climbable = collision.GetComponent<Climbable>();
        }

        ToolMenuButton toolMenuButton = collision.GetComponent<ToolMenuButton>();
        if (toolMenuButton != null)
        {
            if (toolMenuButton.GetUseWeaponMode())
            {
                _weaponModeBuffer = toolMenuButton.GetWeaponMode();
            }
            else
            {
                _gadgetModeBuffer = toolMenuButton.GetGadgetMode();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Climbable")
        {
            canClimb = false;
            climbable = null;
        }
    }

    public bool GetCanClimb()
    {
        return canClimb;
    }

    public Vector3 GetPrevPos()
    {
        return prevPos;
    }

    public Vector3 GetPosDelta()
    {
        return transform.position - prevPos;
    }

    public Transform GetClimbableTransform()
    {
        return climbable.transform;
    }

    public bool GetIsTop()
    {
        return climbable.GetIsTop();
    }

    public Climbable GetClimbable()
    {
        return climbable;
    }

    public WeaponMode GetWeaponModeBuffer()
    {
        return _weaponModeBuffer;
    }

    public GadgetMode GetGadgetModeBuffer()
    {
        return _gadgetModeBuffer;
    }
}
