using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

enum WeaponMode : int
{
    None, SemiAutoRifle
}

enum GadgetMode : int
{
    None
}

public class Player : MonoBehaviour
{
    [SerializeField] GameObject rightController;
    [SerializeField] Cartridge cartridge;
    [SerializeField] PlayerHand handLeft;
    [SerializeField] PlayerHand handRight;

    [SerializeField] WeaponMode weaponMode;
    [SerializeField] GadgetMode gadgetMode;
    [SerializeField] bool isWeaponLeft = true;

    [Header("Input")]
    [SerializeField] InputActionReference _ejectInput;
    [SerializeField] InputActionReference _climbLeftInput;
    [SerializeField] InputActionReference _climbRightInput;

    VRDebug _vrDebug;
    XRCharacterController _charController;
    bool isClimbingLeft = false;
    bool isClimbingRight = false;

    bool shouldTeleportLeft = false;
    bool shouldTeleportRight = false;


    void Awake()
    {
    }

    void OnDestroy()
    {
        _ejectInput.action.performed -= Eject;
        _climbLeftInput.action.performed -= ClimbLeft;
        _climbRightInput.action.performed -= ClimbRight;
    }

    // Start is called before the first frame update
    void Start()
    {
        _vrDebug = FindObjectOfType<VRDebug>();
        _vrDebug.Log("Init");

        _charController = GetComponent<XRCharacterController>();
        
        
        // Assign Input Variables
        _ejectInput.action.performed += Eject;
        _climbLeftInput.action.performed += ClimbLeft;
        _climbRightInput.action.performed += ClimbRight;
    }

    // Update is called once per frame
    void Update()
    {
        _vrDebug.Monitor(1, handLeft.transform.localPosition.y.ToString());
        if (isClimbingLeft || isClimbingRight)
        {
            _charController.SetGravityEnabled(false);
            Climb();
        }
        else _charController.SetGravityEnabled(true);
    }

    void Eject(InputAction.CallbackContext ctx)
    {
        if (weaponMode != WeaponMode.None)
        {
            if (cartridge != null)
            {
                cartridge.Detach();
                cartridge = null;
            }
            else
            {
                _vrDebug.Log("Cartridge is Null");
            }
        }
    }

    void Climb()
    {
        bool left = isClimbingLeft && handLeft.GetCanClimb();
        bool right = isClimbingRight && handRight.GetCanClimb();
        
        if (left && right)
        {
            _charController.ClimbMove((-handLeft.GetPosDelta() - handRight.GetPosDelta()) / 2);
            if (handRight.transform.localPosition.y <= 1 && handRight.GetIsTop())
            {
                shouldTeleportRight = true;
            }

            if ((handLeft.transform.localPosition.y <= 1) && handLeft.GetIsTop())
            {
                shouldTeleportLeft = true;
            }
        }
        else if (left)
        {
            _charController.ClimbMove(-handLeft.GetPosDelta());
            if (handLeft.transform.localPosition.y <= 1 && handLeft.GetIsTop())
            {
                shouldTeleportLeft = true;
            }
        }
        else if (right)
        {
            _charController.ClimbMove(-handRight.GetPosDelta());
            if (handRight.transform.localPosition.y <= 1 && handRight.GetIsTop())
            {
                shouldTeleportRight = true;
            }
        }
    }

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

    public bool GetIsClimbing()
    {
        return (isClimbingLeft && handLeft.GetCanClimb()) || (isClimbingRight && handRight.GetCanClimb());
    }
}
