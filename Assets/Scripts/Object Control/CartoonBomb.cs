using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonBomb : MonoBehaviour
{
    Animator animator;

    // Use timer to determine when cartoon bomb explodes.
    private float timer = 0f;
    private bool isTimer = false;
    public float explosionTimer = 3f;
    private bool hasExploded = false;
    public GameObject player;

    // Parameters for explosion force
    public float explosionForce = 100f;
    public float explosionRadius = 10f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Use this for the countdown timer for when cartoon bomb explodes.
    void FixedUpdate()
    {
        if (isTimer)
        {
            timer += Time.deltaTime;

            // If time limit reached, cartoon bomb should explode and is deleted.
            if (timer > explosionTimer && hasExploded == false)
            {
                hasExploded = true;
                animator.SetBool("isTimeUp", true);
                //Explode();
                Destroy(this.gameObject, 2);
            }
        }
    }

    // If the player is within the radius.
    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null && c.gameObject.tag == "Player")
        {
            animator.SetBool("isClose", true);

            // Set timer on.
            isTimer = true;
        }
    }

    // If the player exits the proximity radius.
/*    void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null && c.gameObject.tag == "Player")
        {
            animator.SetBool("isClose", false);

            // Set timer off and reset.
            isTimer = false;
            timer = 0f;
        }
    }*/

    void Explode()
    {
        EventManager.TriggerEvent<ExplosionEvent, Vector3>(gameObject.transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log("Explosion force applied to: " + hit);
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            if (hit.CompareTag("Player"))
            {
                HealthTracker health = player.GetComponent<HealthTracker>();
                if (health != null)
                {
                    health.LoseHealth(1);
                }
            }
        }
    }
}
