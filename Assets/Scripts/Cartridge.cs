using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{
    //Inspector
    [SerializeField] Transform _attachedTo;
    [SerializeField] float ejectForce = 100f;

    //Private References
    Rigidbody _rigidbody;
    
    //Private Members
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _attachedTo.position;
        transform.SetParent(_attachedTo);

        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Detach()
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(transform.rotation * Vector3.left * ejectForce, ForceMode.Impulse);
    }
}
