using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Reticle : MonoBehaviour
{
    [SerializeField] float triggerTime = 0.1f;
    [SerializeField] float returnTime = 2f;

    [Header("Straight")]
    [SerializeField] bool useStraight = false;
    [SerializeField] float straightDistance = 0.1f;
    [SerializeField] Transform north;
    [SerializeField] Transform east;
    [SerializeField] Transform south;
    [SerializeField] Transform west;

    [Header("Angled")]
    [SerializeField] bool useAngled = false;
    [SerializeField] float angledDistance = 0.1f;
    [SerializeField] Transform northEast;
    [SerializeField] Transform northWest;
    [SerializeField] Transform southEast;
    [SerializeField] Transform southWest;

    float timer;
    bool isAnimating = false;
    bool isReturning = false;
    float outTargetStraight = 0f;
    float outStartStraight = 0f;
    Vector2 outTargetAngled = Vector2.zero;
    Vector2 outStartAngled = Vector2.zero;

    float straightStart;
    Vector2 angledStart;


    void Awake()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (useStraight && (!north || !east || !south || !west))
            Debug.LogError("GameObjects for straight components not assigned");
        if (useAngled && (!northEast || !northWest || !southEast || !southWest))
            Debug.LogError("GameObjects for angled components not assigned");

        if (useStraight)
        {
            straightStart = north.localPosition.y;
        }

        if (useAngled)
        {
            angledStart = new Vector2(northEast.localPosition.x, northEast.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReturning) Out();
        else In();
    }

    public void Trigger()
    {
        timer = 0f;
        isReturning = false;
        isAnimating = true;
        if (useStraight)
        {
            outStartStraight = north.localPosition.y;
            outTargetStraight = north.localPosition.y + straightDistance;
        }

        if (useAngled)
        {
            outStartAngled = new Vector2(-northEast.localPosition.x, northEast.localPosition.y);
            outTargetAngled = new Vector2(-northEast.localPosition.x + angledDistance, northEast.localPosition.y + angledDistance);
            VRDebug.Get().Log(outTargetAngled.x.ToString() + " " + outTargetAngled.y.ToString());
        }
    }

    void Out()
    {
        if (isAnimating)
        {
            if (timer >= triggerTime)
            {
                timer = 0f;
                isReturning = true;
            }
            else
            {
                if (useStraight)
                {
                    float newPos = Mathf.Lerp(outStartStraight, outTargetStraight, timer / triggerTime);

                    north.localPosition = new Vector3(north.localPosition.x, newPos, north.localPosition.z);
                    east.localPosition = new Vector3(-newPos, east.localPosition.y, east.localPosition.z);
                    south.localPosition = new Vector3(south.localPosition.x, -newPos, south.localPosition.z);
                    west.localPosition = new Vector3(newPos, west.localPosition.y, west.localPosition.z);
                }
                if (useAngled)
                {
                    Vector2 newPos = new Vector2(
                        Mathf.Lerp(outStartAngled.x, outTargetAngled.x, timer / triggerTime),
                        Mathf.Lerp(outStartAngled.y, outTargetAngled.y, timer / triggerTime)
                    );

                    northEast.localPosition = new Vector3(-newPos.x, newPos.y, northEast.localPosition.z);
                    northWest.localPosition = new Vector3(newPos.x, newPos.y, northWest.localPosition.z);
                    southEast.localPosition = new Vector3(-newPos.x, -newPos.y, southEast.localPosition.z);
                    southWest.localPosition = new Vector3(newPos.x, -newPos.y, southWest.localPosition.z);
                }
                timer += Time.deltaTime;
            }
        }
    }

    void In()
    {
        if (timer >= returnTime)
        {
            if (useStraight)
            {
                north.localPosition = new Vector3(north.localPosition.x, straightStart, north.localPosition.z);
                east.localPosition = new Vector3(-straightStart, east.localPosition.y, east.localPosition.z);
                south.localPosition = new Vector3(south.localPosition.x, -straightStart, south.localPosition.z);
                west.localPosition = new Vector3(straightStart, west.localPosition.y, west.localPosition.z);
            }

            if (useAngled)
            {
                northEast.localPosition = new Vector3(angledStart.x, angledStart.y, northEast.localPosition.z);
                northWest.localPosition = new Vector3(-angledStart.x, angledStart.y, northWest.localPosition.z);
                southEast.localPosition = new Vector3(angledStart.x, -angledStart.y, southEast.localPosition.z);
                southWest.localPosition = new Vector3(-angledStart.x, -angledStart.y, southWest.localPosition.z);
            }

            isReturning = false;
            isAnimating = false;
        }
        else
        {
            if (useStraight)
            {
                float newPos = Mathf.Lerp(outTargetStraight, straightStart, timer / returnTime);

                north.localPosition = new Vector3(north.localPosition.x, newPos, north.localPosition.z);
                east.localPosition = new Vector3(-newPos, east.localPosition.y, east.localPosition.z);
                south.localPosition = new Vector3(south.localPosition.x, -newPos, south.localPosition.z);
                west.localPosition = new Vector3(newPos, west.localPosition.y, west.localPosition.z);
            }

            if (useAngled)
            {
                Vector2 newPos = new Vector2(
                    Mathf.Lerp(outTargetAngled.x, angledStart.x, timer / returnTime),
                    Mathf.Lerp(outTargetAngled.y, angledStart.y, timer / returnTime)
                );

                northEast.localPosition = new Vector3(-newPos.x, newPos.y, northEast.localPosition.z);
                northWest.localPosition = new Vector3(newPos.x, newPos.y, northWest.localPosition.z);
                southEast.localPosition = new Vector3(-newPos.x, -newPos.y, southEast.localPosition.z);
                southWest.localPosition = new Vector3(newPos.x, -newPos.y, southWest.localPosition.z);
            }
            timer += Time.deltaTime;
        }
    }
}
