using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public float startMovingDistance = 4f;
    public float stopMovingDistance = 8f;
    public float moveSpeed = 4f;
    public GameObject mouse;
    private AIState currentState;
    private float distanceToMouse = 0f;
    private enum AIState
    {
        Flee,
        Stay
    }

    void Start()
    {
        currentState = AIState.Stay;
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Stay:
                transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
                distanceToMouse = Vector3.Distance(this.transform.position, mouse.transform.position);
                if (distanceToMouse < startMovingDistance)
                {
                    currentState = AIState.Flee;
                }
                break;
            case AIState.Flee:
                transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
                Vector3 direction = transform.position - mouse.transform.position;
                direction.y = 0;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

                distanceToMouse = Vector3.Distance(this.transform.position, mouse.transform.position);
                if (distanceToMouse > stopMovingDistance)
                {
                    currentState = AIState.Stay;
                }
                break;
            default:
                break;
        }
    }
}
