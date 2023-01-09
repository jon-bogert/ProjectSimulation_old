using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    None
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

    [SerializeField] float climbTeleportThreshold = 0.66f;

    [Header("Input")]
    [SerializeField] InputActionReference _ejectInput;
    [SerializeField] InputActionReference _climbLeftInput;
    [SerializeField] InputActionReference _climbRightInput;
    [SerializeField] InputActionReference _gadgetRightInput;
    
    [Header("Weapon Objects")]
    [SerializeField] PlayerHand handRight;
    [SerializeField] GameObject semiAutoRifle;

    [Header("Gadget Objects")]
    [SerializeField] PlayerHand handLeft;
    
    VRDebug _vrDebug;
    XRCharacterController _charController;
    
    bool shouldTeleportLeft = false;
    bool shouldTeleportRight = false;

    WeaponMode lastWeapon = WeaponMode.None;
    GadgetMode lastGadget = GadgetMode.None;
    
    //Input Related
    bool isClimbingLeft = false;
    bool isClimbingRight = false;
    bool isRightGadgetVisible = false;


    
    
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
    }

    // Start is called before the first frame update
    void Start()
    {
        _vrDebug = FindObjectOfType<VRDebug>();

        _charController = GetComponent<XRCharacterController>();
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
        else _charController.SetGravityEnabled(true);
    }
    
    void OnDestroy()
    {
        _ejectInput.action.performed -= Eject;
        _climbLeftInput.action.performed -= ClimbLeft;
        _climbRightInput.action.performed -= ClimbRight;
        _gadgetRightInput.action.performed -= GadgetRight;
    }
    
    
    
    // =============================================================================================================
    //           MEMBER FUNCTIONS 
    // =============================================================================================================

    void Climb(bool left, bool right)
    {
        float teleThreshold = ((_camera.transform.position.y - cameraOffsetObj.position.y) * climbTeleportThreshold) + cameraOffsetObj.position.y;
        _vrDebug.Monitor(1, handLeft.transform.position.y.ToString());
        _vrDebug.Monitor(3, teleThreshold.ToString());
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
