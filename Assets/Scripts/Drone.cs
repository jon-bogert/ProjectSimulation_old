
using UnityEngine;
using UnityEngine.InputSystem;

public class Drone : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float movementSmoothing = 1f;
    [SerializeField] float turnAmount = 30f;
    [SerializeField] float turnActivationValue = 0.5f;
    [SerializeField] float turnResetValue = 0.1f;
    
    [Header("References")]
    [SerializeField] Camera _camera;
    [SerializeField] GameObject rotationOffset;
    
    [Header("InputActions")]
    [SerializeField] InputActionReference inputMove;
    [SerializeField] InputActionReference inputTurnClimb;

    Rigidbody _rigidbody;
    Vector3 _velocity;
    bool _turnTrig = false;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (!_rigidbody) Debug.LogError("Drone must have Rigidbody Component");

        inputTurnClimb.action.performed += Turn;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = inputMove.action.ReadValue<Vector2>();
        Vector2 turnClimbVector = inputTurnClimb.action.ReadValue<Vector2>();
        
        //Snap Turn
        if (turnClimbVector.x >= turnActivationValue && !_turnTrig)
        {
            rotationOffset.transform.Rotate(0f, turnAmount, 0f);
            _turnTrig = true;
        }
        else if (turnClimbVector.x <= -turnActivationValue && !_turnTrig)
        {
            rotationOffset.transform.Rotate(0f, -turnAmount, 0f);
            _turnTrig = true;
        }
        else if (turnClimbVector.x < turnResetValue && turnClimbVector.x > -turnResetValue && _turnTrig)
        {
            _turnTrig = false;
        }

        //Move
        Vector3 forward =
            Vector3.Normalize(new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z));
        Vector3 right = Vector3.Normalize(new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z));
        Vector3 up = Vector3.Normalize(new Vector3(_camera.transform.up.x, 0f, _camera.transform.up.z));
        
        Vector3 direction = right * moveVector.x + forward * moveVector.y;
        Vector3 targetVelocity = direction * moveSpeed + Vector3.up * (moveSpeed * turnClimbVector.y);
        _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, movementSmoothing);

    }

    void OnDestroy()
    {
        inputTurnClimb.action.performed -= Turn;
    }

    void Turn(InputAction.CallbackContext ctx)
    {
        Vector2 turnClimbVector = ctx.ReadValue<Vector2>();
        
    }
}
