using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonBombDamage : MonoBehaviour
{
    // Initialize the damage cooldown timer reached and damage allowed.
    private float damageDelay = 5.0f;
    private float damageDelayCount = 5.0f;
    private bool damageAllowed = true;


    // TODO: Fix how this gets triggered. Currently this is getting triggered if the mouse touches any part of the trap instead of just the trigger area.
    // Maybe move this script to the component where the trigger area is on the prefab?
    private void OnTriggerEnter(Collider c)
    {
        if (!(c.attachedRigidbody is null))
        {
            if (damageAllowed)
            {
                // Decrease mouse/player health
                HealthTracker health = c.attachedRigidbody.gameObject.GetComponent<HealthTracker>();

                if (health != null)
                {
                    health.LoseHealth(1);

                    // Restart cooldown timer.
                    damageDelayCount = 0f;
                }
            }
        }
    }

    void DamageTimer()
    {
        if (damageDelayCount <= damageDelay)
        {
            damageDelayCount += Time.deltaTime;
            damageAllowed = false;
        }
        else
        {
            damageAllowed = true;
        }
    }
}
