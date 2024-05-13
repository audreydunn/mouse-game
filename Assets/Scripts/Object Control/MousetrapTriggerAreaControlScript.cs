using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(BoxCollider))]
public class MousetrapTriggerAreaControlScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private Vector3 impulseForce;
    private Vector3 torqueForce;
    public bool triggered;
    private int triggerC;
    private int notTriggeredTime = 0;

    public float minMassForTrigger = 0.1f;
    public float minWait = 5.0f;  //at least 5 seconds since untriggered

    public float maxTorque = 2.0f;
    public float minVImpulse = 1.0f;
    public float maxVImpulse = 3.0f;

    // Initialize the damage cooldown timer reached and damage allowed.
    private float damageDelay = 9.0f;
    private float damageDelayCount = 9.0f;
    private bool damageAllowed = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        
        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");
    }

    // TODO: Fix how this gets triggered. Currently this is getting triggered if the mouse touches any part of the trap instead of just the trigger area.
    // Maybe move this script to the component where the trigger area is on the prefab?
    private void OnTriggerEnter(Collider c)
    {
        // Debug.Log("Collision");
        if (!(c.attachedRigidbody is null))
        {
            if (c.attachedRigidbody.mass > minMassForTrigger && damageAllowed)
            {
                EventManager.TriggerEvent<MouseTrapEvent, Vector3>(c.transform.position);
                triggered = true;
                ++triggerC;
                Debug.Log("MouseTrapTriggered");
                // Decrease mouse/player health
                HealthTracker health = c.attachedRigidbody.gameObject.GetComponent<HealthTracker>();

                // Restart cooldown timer.
                damageDelayCount = 0f;

                if (health != null)
                {
                    health.LoseHealth(1);
                }
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (!(c.attachedRigidbody is null))
        {
            if (c.attachedRigidbody.mass > minMassForTrigger)
            {
                --triggerC;
                if (triggerC <= 0)
                {
                    triggerC = 0;
                    triggered = false;
                    notTriggeredTime = 0;
                }
            }
        }
           
    }

    void FixedUpdate()
    {
        bool windUp = false;
        bool trigger = false;

        var animState = anim.GetCurrentAnimatorStateInfo(0);
        if (!triggered && animState.IsName("Mousetrap_IdleTriggered"))
        {
            ++notTriggeredTime;
            if (notTriggeredTime >= minWait * 50)
            {
                windUp = true;
            }
        }
        //if (animState.IsName("Mousetrap_Triggered") | animState.IsName("Mousetrap_IdleTriggered") | animState.IsName("Mousetrap_Windup"))
        //{
        //    trapSet = false;
        //}
        if (triggered && animState.IsName("Mousetrap_Idle"))
        {
            trigger = true;
            //Code below could be used to replace the bouncing part of the mouse trap trigger animation, if needed

            //impulseForce = new Vector3(0f, Random.Range(minVImpulse, maxVImpulse), Random.Range(- 0.5f * maxVImpulse, -0.5f * minVImpulse));
            //torqueForce = new Vector3(Random.Range(0.3f * maxTorque, maxTorque), 0f, 0f);
            //rbody.AddRelativeForce(impulseForce, ForceMode.Impulse);
            //rbody.AddRelativeTorque(torqueForce, ForceMode.Impulse);
        }
        anim.SetBool("TrapTriggered", trigger);
        anim.SetBool("WindUp", windUp);

        DamageTimer();
    }

    // Added a damage timer so the trigger collision does not damage the mouse infinitely.
    void DamageTimer()
    {
        if (damageDelayCount <= damageDelay)
        {
            // Increment the timer if not ~9 seconds yet.
            damageDelayCount += Time.deltaTime;
            damageAllowed = false;
        }
        else
        {
            damageAllowed = true;
        }
    }
}