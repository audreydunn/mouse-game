using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCheese : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            // Play sound and delete object
            CheeseCollector cheese = c.attachedRigidbody.gameObject.GetComponent<CheeseCollector>();
            HealthTracker health = c.attachedRigidbody.gameObject.GetComponent<HealthTracker>();
            CheesePerRoomTracker roomCheese = c.attachedRigidbody.gameObject.GetComponent<CheesePerRoomTracker>();
            if (cheese != null)
            {
                int cheeseAmount = cheese.CollectCheese();
                roomCheese.CollectCheese();
                EventManager.TriggerEvent<CollectCheeseEvent, int>(cheeseAmount);
                EventManager.TriggerEvent<ChewingEvent, Vector3>(c.transform.position);
                health.GainHealth(1);
                Destroy(transform.parent.gameObject);
            }

        }

    }
}
