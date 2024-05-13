using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    public Animator spikeTrapAnimator;
    public int initSpikeDelay = 0; // time until spike cycle begins
    public int loopSpikeDelay = 10; // time until spikes return to animation
    public Material greenGelMaterial;
    public Material redGelMaterial;
    public GameObject side_1;
    public GameObject side_2;
    public GameObject side_3;
    public GameObject side_4;

    private bool firstLoop = true;
    private bool firstLoop2 = true;

    void Start()
    {
        if (!spikeTrapAnimator)
        {
            spikeTrapAnimator = GetComponent<Animator>();
        }
        StartCoroutine(PlayAnimation());
    }

    void Update()
    {
        if (spikeTrapAnimator.GetCurrentAnimatorStateInfo(0).IsName("MoveSpike"))
        {
            side_1.GetComponent<Renderer>().material = redGelMaterial;
            side_2.GetComponent<Renderer>().material = redGelMaterial;
            side_3.GetComponent<Renderer>().material = redGelMaterial;
            side_4.GetComponent<Renderer>().material = redGelMaterial;
        }
        else 
        {
            side_1.GetComponent<Renderer>().material = greenGelMaterial;
            side_2.GetComponent<Renderer>().material = greenGelMaterial;
            side_3.GetComponent<Renderer>().material = greenGelMaterial;
            side_4.GetComponent<Renderer>().material = greenGelMaterial;
        }
    }

    // It just works.
    // Uses coroutine to delay animation on spikes
    IEnumerator PlayAnimation()
    {
        while (true)
        {

            if (!firstLoop) 
            {
                yield return new WaitForSeconds(loopSpikeDelay / 2);
            }
            else 
            {
                firstLoop = false;
            }
            spikeTrapAnimator.SetBool("play", false);
            yield return new WaitForSeconds(loopSpikeDelay / 2);

            if (firstLoop2) 
            {
                firstLoop2 = false;
                yield return new WaitForSeconds(initSpikeDelay / 2);
            }

            spikeTrapAnimator.SetBool("play", true);

        }
    }

    void OnTriggerEnter(Collider c)
    {
        // Decrease mouse/player health
        HealthTracker health = c.attachedRigidbody.gameObject.GetComponent<HealthTracker>();
        if (health != null)
        {
            health.LoseHealth(1);
        }
    }

    void OnTriggerExit(Collider c)
    {
        
    }

    void playSound()
    {
        EventManager.TriggerEvent<SpikesEvent, Vector3>(this.transform.position);
    }
}
