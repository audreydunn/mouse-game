using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyCheese : MonoBehaviour
{
    public GameObject mouse;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthTracker health = mouse.GetComponent<HealthTracker>();
            if (health != null)
            {
                if (health.curHealth < health.maxHealth)
                {
                    int amount = health.maxHealth - health.curHealth;
                    health.GainHealth(amount);
                }
            }
        }
        EventManager.TriggerEvent<mouseHealEvent, Vector3>(this.transform.position);
        this.gameObject.SetActive(false);
    }

}
