using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleGun : MonoBehaviour
{
    [SerializeField] float maxRange = 10f;
    [SerializeField] float travelSpeed = 2f;
    
    [Header("References")]
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject indicator;
    [SerializeField] Player player;

    [Header("Input")]
    [SerializeField] InputActionReference inputShoot;

    float timeToTravel = 0f;
    
    bool canAttach = false;
    Vector3 targetPos = Vector3.zero;

    Vector3 travelStart = Vector3.zero;
    float travelTimer = 0f;
    bool isGrappleing = false;

    bool inputIsDown = false;

    void Awake()
    {
        inputShoot.action.performed += ShootAction;
        
        if (travelSpeed == 0f) Debug.LogError("travelSpeed must not be Zero");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastUpdate();
        GrappleUpdate();
    }

    void RaycastUpdate()
    {
        if (!isGrappleing && Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, maxRange) && hitInfo.transform.gameObject.tag == "Grapple")
        {
            targetPos = hitInfo.transform.position;
            canAttach = true;

            if (travelSpeed != 0f) timeToTravel = hitInfo.distance / travelSpeed;

            indicator.SetActive(true);
            indicator.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
            indicator.transform.LookAt(muzzle);
            
        }
        else if (canAttach)
        {
            canAttach = false;
            indicator.SetActive(false);
        }
    }

    void GrappleUpdate()
    {
        if (isGrappleing && travelTimer >= timeToTravel)
        {
            Vector3 playerDelta = new Vector3(
                muzzle.position.x - player.transform.position.x,
                muzzle.position.y - player.transform.position.y,
                muzzle.position.z - player.transform.position.z
            );

            //player.GrappleMove(targetPos + playerDelta); 
            player.transform.position = targetPos + playerDelta;
        }
        if (isGrappleing)
        {
            float t = travelTimer / timeToTravel;
            Vector3 newPosition = Vector3.Lerp(travelStart, targetPos, t);

            Vector3 playerDelta = new Vector3(
                muzzle.position.x - player.transform.position.x,
                muzzle.position.y - player.transform.position.y,
                muzzle.position.z - player.transform.position.z
                );

            //player.GrappleMove(newPosition - playerDelta);
            player.transform.position = newPosition - playerDelta;
            travelTimer += Time.deltaTime;
        }
    }

    void OnDisable()
    {
        if (indicator)
            indicator.SetActive(false);
    }

    void OnDestroy()
    {
        inputShoot.action.performed -= ShootAction;
    }

    void ShootAction(InputAction.CallbackContext ctx)
    {
        inputIsDown = !inputIsDown;
        if (inputIsDown && canAttach)
        {
            FindObjectOfType<VRDebug>().Log("Attached");
            travelStart = new Vector3(muzzle.position.x, muzzle.position.y,
                muzzle.position.z);
            travelTimer = 0f;
            isGrappleing = true;
            player.SetIsGrappling(true);
        }
        else if (!inputIsDown)
        {
            isGrappleing = false;
            player.SetIsGrappling(false);
        }
    }
}
