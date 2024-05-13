using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWalkingEmitter : MonoBehaviour
{
    public void ExecuteFootstep()
    {

        EventManager.TriggerEvent<MouseWalkingEvent, Vector3>(transform.position);
    }
}
