using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    [SerializeField] bool isTop = false;
    [SerializeField] float forwardDisplacement = 0.5f;
    BoxCollider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsTop()
    {
        return isTop;
    }
    
    public void TriggerTeleport(Player player)
    {
        if (isTop)
        {
            Vector3 newPos = new Vector3(
                transform.position.x,
                transform.position.y + _collider.size.y / 2,
                transform.position.z + forwardDisplacement
            );
        }
    }
}
