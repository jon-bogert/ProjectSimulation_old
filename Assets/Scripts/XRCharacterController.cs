using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Microsoft.Win32.SafeHandles;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRCharacterController : MonoBehaviour
{
    [Header("Continuous Movement")]
    [SerializeField] float moveSpeed = 2f;

    [Header("Snap Turn")]
    [SerializeField] float activateValue = 0.5f;
    [SerializeField] float resetValue = 0.1f;
    [SerializeField] float turnAmt = 30f;
    
    [Header("Jump")]
    [SerializeField] float jumpAmt = 7f;
    [SerializeField] bool canDoubleJump = false;

    [Header("Fall Dampeners")]
    [SerializeField] bool fallDampenersOn = true;
    [SerializeField] float fallDamageMinVelocity = -4f;
    [SerializeField] float fallDampenerDistance = 1f;
    [SerializeField] float fallDampenerFactor = 0.2f;
    
    [Header("Input Actions")]
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _turnInput;
    [SerializeField] InputActionReference _jumpInput;

    
    //Members
    Vector3 _moveVector;
    bool _turnTrig = false;
    bool _hasDoubleJumped = false;

    //Private references
    VRDebug _vrDebug;

    CharacterController _charController;
    XROrigin _xrOrigin;
    Camera _camera;
    PlayerGravity _gravity;
    Player _player;

    // Start is called before the first frame update
    void Start()
    {
        //Assign References
        _vrDebug = FindObjectOfType<VRDebug>();
        _charController = GetComponent<CharacterController>();
        _xrOrigin = GetComponent<XROrigin>();
        _gravity = GetComponent<PlayerGravity>();
        _camera = _xrOrigin.Camera;
        _player = GetComponent<Player>();
        
        //Subscribe Input Actions
        _moveInput.action.performed += MoveInput;
        _turnInput.action.performed += TurnInput;
        _jumpInput.action.performed += JumpInput;
        
        //Start
        ControllerUpdate();
    }

    void OnDestroy()
    {
        //Unsubscribe Input Actions
        _moveInput.action.performed -= MoveInput;
        _turnInput.action.performed -= TurnInput;
        _jumpInput.action.performed -= JumpInput;
    }

    // Update is called once per frame
    void Update()
    {
        ControllerUpdate();
        Move();
        FallDampeners();
        
        if (_charController.isGrounded && _hasDoubleJumped) _hasDoubleJumped = false;
    }

    void Move()
    {
        if (!_player.GetIsClimbing())
        {
            Vector3 forward =
                Vector3.Normalize(new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z));
            Vector3 right = Vector3.Normalize(new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z));

            Vector3 direction = right * _moveVector.x + forward * _moveVector.z;
            //_vrDebug.Monitor(3, direction.ToString());
            _charController.Move(direction * (moveSpeed * Time.deltaTime));
        }
    }

    public void ClimbMove(Vector3 amt)
    {
        _charController.Move(amt);
    }

    public void GrappleMove(Vector3 pos)
    {
        Vector3 amt = new Vector3(
            pos.x - transform.position.x,
            pos.y - transform.position.y,
            pos.z - transform.position.z
            );
        _charController.Move(amt);
    }

    void FallDampeners()
    {
        if (fallDampenersOn && _gravity.GetMovementY() < 0)
        {
            bool inRange = Physics.Raycast(_player.transform.position, Vector3.down, out RaycastHit hitInfo, fallDampenerDistance);
            if (inRange && _gravity.GetMovementY() <= fallDamageMinVelocity && !_charController.isGrounded)
            {
                _gravity.MultMovementY(fallDampenerFactor);
            }
        }
        else if (_gravity.GetMovementY() <= fallDamageMinVelocity)
        {
            Debug.Log("Fall Damage");
        }
    }

    void MoveInput(InputAction.CallbackContext ctx)
    {
        _moveVector = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
    }

    void TurnInput(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<Vector2>().x;
        if (value >= activateValue && !_turnTrig)
        {
            transform.Rotate(0f, turnAmt, 0f);
            _turnTrig = true;
        }
        else if (value <= -activateValue && !_turnTrig)
        {
            transform.Rotate(0f, -turnAmt, 0f);
            _turnTrig = true;
        }
        else if (value < resetValue && value > -resetValue && _turnTrig)
        {
            _turnTrig = false;
        }
    }

    void JumpInput(InputAction.CallbackContext ctx)
    {
        if (_charController.isGrounded || (canDoubleJump && !_hasDoubleJumped))
        {
            if (!_charController.isGrounded) _hasDoubleJumped = true;
            _gravity.SetMovementY(jumpAmt);
        }
    }

    void ControllerUpdate()
    {
        Vector3 position = _xrOrigin.Camera.transform.localPosition;
        _charController.height = position.y;
        Vector3 newCenter = new Vector3(position.x, _charController.height * 0.5f, position.z);
        _charController.center = newCenter;
    }

    public void SetGravityEnabled(bool setTo)
    {
        if (_gravity)
            _gravity.ToggleGravity(setTo);
    }
}
