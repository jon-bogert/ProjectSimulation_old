using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject rightController;
    [SerializeField] Cartridge cartridge;

    [Header("Input")]
    [SerializeField] InputActionReference _ejectInput;

    VRDebug _vrDebug;


    void Awake()
    {
    }

    void OnDestroy()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _vrDebug = FindObjectOfType<VRDebug>();
        _vrDebug.Log("Init");
        
        
        // Assign Input Variables
        _ejectInput.action.performed += Eject;
    }

    // Update is called once per frame
    void Update()
    {
        //_vrDebug.Monitor(1, rightController.transform.rotation.eulerAngles.z.ToString());
    }

    void Eject(InputAction.CallbackContext ctx)
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
