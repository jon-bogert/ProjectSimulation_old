using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlasmaCutter : MonoBehaviour
{

    [SerializeField] float cutLength = 0.1f;
    
    [Header("References")]
    [SerializeField] Transform muzzle;
    
    [Header("Input")]
    [SerializeField] InputActionReference useInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useInput.action.ReadValue<float>() > 0.5f
            && Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, cutLength)) // Cutting
        {
            if (hitInfo.collider.gameObject.CompareTag("Cuttable"))
            {
                hitInfo.collider.gameObject.SetActive(false);
            }
        }
    }
}
