// Konami Code trigger

using UnityEngine;
using UnityEngine.InputSystem;

public class KCode : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] InputActionReference _inputUp;
    [SerializeField] InputActionReference _inputDown;
    [SerializeField] InputActionReference _inputLeft;
    [SerializeField] InputActionReference _inputRight;
    [SerializeField] InputActionReference _inputB;
    [SerializeField] InputActionReference _inputA;

    [Header("Trigger Reference")]
    [SerializeField] GameObject _triggerObject; // Change "GameObject" to class type

    int count = 0;

    void Trigger()
    {
        Debug.Log("KCode Triggered");
        // Place trigger function call here
    }

    void Awake()
    {
        _inputUp.action.performed += Up;
        _inputDown.action.performed += Down;
        _inputLeft.action.performed += Left;
        _inputRight.action.performed += Right;
        _inputB.action.performed += B;
        _inputA.action.performed += A;
    }

    void OnDestroy()
    {
        _inputUp.action.performed -= Up;
        _inputDown.action.performed -= Down;
        _inputLeft.action.performed -= Left;
        _inputRight.action.performed -= Right;
        _inputB.action.performed -= B;
        _inputA.action.performed -= A;
    }

    void Up(InputAction.CallbackContext ctx)
    {
        if (count == 0 || count == 1)
        {
            ++count;
        }
        else ResetCount();
    }
    void Down(InputAction.CallbackContext ctx)
    {
        if (count == 2 || count == 3)
        {
            ++count;
        }
        else ResetCount();
    }
    void Left(InputAction.CallbackContext ctx)
    {
        if (count == 4 || count == 6)
        {
            ++count;
        }
        else ResetCount();
    }
    void Right(InputAction.CallbackContext ctx)
    {
        if (count == 5 || count == 7)
        {
            ++count;
        }
        else ResetCount();
    }
    void B(InputAction.CallbackContext ctx)
    {
        if (count == 8)
        {
            ++count;
        }
        else ResetCount();
    }
    void A(InputAction.CallbackContext ctx)
    {
        if (count == 9)
        {
            Trigger();
        }
        
        ResetCount();
    }

    void ResetCount()
    {
        count = 0;
    }
}
