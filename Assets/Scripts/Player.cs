using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public enum WeaponMode : int
{
    None, SemiAutoRifle
}

public enum GadgetMode : int
{
    None, Grapple
}

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    [SerializeField] Transform cameraOffsetObj;
    [SerializeField] GameObject rightController;
    [SerializeField] Cartridge cartridge;

    [SerializeField] WeaponMode weaponMode;
    [SerializeField] GadgetMode gadgetMode;
    [SerializeField] bool isWeaponLeft = false;

    [SerializeField] GameObject weaponMenu;
    [SerializeField] GameObject gadgetMenu;

    [SerializeField] float climbTeleportThreshold = 0.66f;

    [Header("Input")]
    [SerializeField] InputActionReference _ejectInput;
    [SerializeField] InputActionReference _climbLeftInput;
    [SerializeField] InputActionReference _climbRightInput;
    [SerializeField] InputActionReference _gadgetRightInput;
    [SerializeField] InputActionReference _gadgetLeftInput;
    
    [Header("Weapon Objects")]
    [SerializeField] PlayerHand handRight;
    [SerializeField] GameObject semiAutoRifle;

    [Header("Gadget Objects")]
    [SerializeField] PlayerHand handLeft;
    [SerializeField] GameObject grappleGun;
    
    VRDebug _vrDebug;
    XRCharacterController _charController;
    
    bool shouldTeleportLeft = false;
    bool shouldTeleportRight = false;

    WeaponMode lastWeapon = WeaponMode.None;
    GadgetMode lastGadget = GadgetMode.None;

    bool isGrappling = false;

    //Input Related
    bool isClimbingLeft = false;
    bool isClimbingRight = false;
    bool isRightGadgetVisible = false;
    bool isLeftGadgetVisible = false;


    
    
    // =============================================================================================================
    //           SYSTEM METHODS 
    // =============================================================================================================

    void Awake()
    {
        // Assign Input Variables
        _ejectInput.action.performed += Eject;
        _climbLeftInput.action.performed += ClimbLeft;
        _climbRightInput.action.performed += ClimbRight;
        _gadgetRightInput.action.performed += GadgetRight;
        _gadgetLeftInput.action.performed += GadgetLeft;
    }

    // Start is called before the first frame update
    void Start()
    {
        _vrDebug = FindObjectOfType<VRDebug>();

        _charController = GetComponent<XRCharacterController>();
        _charController.SetGravityEnabled(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Climbing
        bool left = isClimbingLeft && handLeft.GetCanClimb();
        bool right = isClimbingRight && handRight.GetCanClimb();
        if (left || right)
        {
            _charController.SetGravityEnabled(false);
            Climb(left, right);
        }
        else if (!isGrappling)
        {
            _charController.SetGravityEnabled(true);
        }
    }
    
    void OnDestroy()
    {
        _ejectInput.action.performed -= Eject;
        _climbLeftInput.action.performed -= ClimbLeft;
        _climbRightInput.action.performed -= ClimbRight;
        _gadgetRightInput.action.performed -= GadgetRight;
        _gadgetLeftInput.action.performed -= GadgetLeft;
    }
    
    
    
    // =============================================================================================================
    //           MEMBER FUNCTIONS 
    // =============================================================================================================

    void Climb(bool left, bool right)
    {
        float teleThreshold = ((_camera.transform.position.y - cameraOffsetObj.position.y) * climbTeleportThreshold) + cameraOffsetObj.position.y;
        if (left && right)
        {
            _charController.ClimbMove((-handLeft.GetPosDelta() - handRight.GetPosDelta()) / 2);
            if (handRight.transform.position.y <= teleThreshold && handRight.GetIsTop())
            {
                shouldTeleportRight = true;
            }

            if ((handLeft.transform.position.y <= teleThreshold) && handLeft.GetIsTop())
            {
                shouldTeleportLeft = true;
            }
        }
        else if (left)
        {
            _charController.ClimbMove(-handLeft.GetPosDelta());
            if (handLeft.transform.position.y <= teleThreshold && handLeft.GetIsTop())
            {
                shouldTeleportLeft = true;
            }
        }
        else if (right)
        {
            _charController.ClimbMove(-handRight.GetPosDelta());
            if (handRight.transform.position.y <= teleThreshold && handRight.GetIsTop())
            {
                shouldTeleportRight = true;
            }
        }
    }

    public bool GetIsClimbing()
    {
        return (isClimbingLeft && handLeft.GetCanClimb()) || (isClimbingRight && handRight.GetCanClimb());
    }
    
    void AssignWeapon()
    {
        //Deactivate All
        handRight.gameObject.SetActive(false);
        semiAutoRifle.SetActive(false);
        
        //ActivateSelected
        switch (weaponMode)
        {
            case WeaponMode.None:
                handRight.gameObject.SetActive(true);
                break;
            case WeaponMode.SemiAutoRifle :
                semiAutoRifle.SetActive(true);
                break;
        }
    }
    
    void AssignGadget()
    {
        //Deactivate All
        handLeft.gameObject.SetActive(false);
        grappleGun.SetActive(false);
        
        //ActivateSelected
        switch (gadgetMode)
        {
            case GadgetMode.None:
                handLeft.gameObject.SetActive(true);
                break;
            case GadgetMode.Grapple:
                grappleGun.SetActive(true);
                break;
        }
    }

    public void SetIsGrappling(bool set)
    {
        isGrappling = set;
        _charController.SetGravityEnabled(!set);
    }
    
    
    // =============================================================================================================
    //           INPUT PROCESSING 
    // =============================================================================================================
    
    void ClimbLeft(InputAction.CallbackContext ctx)
    {
        isClimbingLeft = !isClimbingLeft;
        if (!isClimbingLeft && shouldTeleportLeft && handLeft.GetCanClimb())
        {
            shouldTeleportLeft = false;
            handLeft.GetClimbable().TriggerTeleport(this);
        }
    }
    void ClimbRight(InputAction.CallbackContext ctx)
    {
        isClimbingRight = !isClimbingRight;
        if (!isClimbingRight && shouldTeleportRight && handRight.GetCanClimb())
        {
            shouldTeleportRight = false;
            handRight.GetClimbable().TriggerTeleport(this);
        }
    }

    WeaponMode tmpWeapon = WeaponMode.None;
    void GadgetRight(InputAction.CallbackContext ctx)
    {
        isRightGadgetVisible = !isRightGadgetVisible;
        if (isRightGadgetVisible) // On Press
        {
            tmpWeapon = weaponMode;
            weaponMode = WeaponMode.None;
            AssignWeapon();
            weaponMenu.SetActive(true);
            weaponMenu.transform.position = handRight.transform.position;
            weaponMenu.transform.LookAt(_camera.transform.position);
        }
        else // On Release
        {
            weaponMenu.SetActive(false);
            WeaponMode newMode = handRight.GetWeaponModeBuffer();
            // If None -> Check Toggle to last weapon used
            if (tmpWeapon == WeaponMode.None && newMode == WeaponMode.None)
            {
                weaponMode = lastWeapon;
            }
            else
            {
                if (newMode != WeaponMode.None || lastWeapon == WeaponMode.None)
                {
                    lastWeapon = tmpWeapon;
                }
                weaponMode = newMode;
                tmpWeapon = WeaponMode.None;
            }
            AssignWeapon();
        }
    }
    
    GadgetMode tmpGadget = GadgetMode.None;
    void GadgetLeft(InputAction.CallbackContext ctx)
    {
        isLeftGadgetVisible = !isLeftGadgetVisible;
        if (isLeftGadgetVisible) // On Press
        {
            tmpGadget = gadgetMode;
            gadgetMode = GadgetMode.None;
            AssignGadget();
            gadgetMenu.SetActive(true);
            gadgetMenu.transform.position = handLeft.transform.position;
            gadgetMenu.transform.LookAt(_camera.transform.position);
        }
        else // On Release
        {
            gadgetMenu.SetActive(false);
            GadgetMode newMode = handLeft.GetGadgetModeBuffer();
            // If None -> Check Toggle to last weapon used
            if (tmpGadget == GadgetMode.None && newMode == GadgetMode.None)
            {
                gadgetMode = lastGadget;
            }
            else
            {
                if (newMode != GadgetMode.None || lastGadget == GadgetMode.None)
                {
                    lastGadget = tmpGadget;
                }
                gadgetMode = newMode;
                tmpGadget = GadgetMode.None;
            }
            AssignGadget();
        }
    }
    
    void Eject(InputAction.CallbackContext ctx)
    {
        // if (weaponMode != WeaponMode.None)
        // {
        //     if (cartridge != null)
        //     {
        //         cartridge.Detach();
        //         cartridge = null;
        //     }
        //     else
        //     {
        //         _vrDebug.Log("Cartridge is Null");
        //     }
        // }
    }
}
