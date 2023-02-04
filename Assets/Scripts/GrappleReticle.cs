using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleReticle : MonoBehaviour
{
    [SerializeField] GameObject outterRing;
    [SerializeField] GameObject innerRing;
    [SerializeField] float outterSpeed = 45f;
    [SerializeField] float innerSpeed = 45f;

    // Start is called before the first frame update
    void Start()
    {
        if (!outterRing || !innerRing)
        {
            Debug.LogWarning("GrappleReticle: Rings gameObjects must be assigned in inspector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        outterRing.transform.Rotate(Vector3.forward, outterSpeed * Time.deltaTime);
        innerRing.transform.Rotate(Vector3.forward, -innerSpeed * Time.deltaTime);
    }
}
