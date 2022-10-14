using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] float health = 30f;

    Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amt)
    {
        health -= amt;
        if (health <= 0f)
            Death();
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
