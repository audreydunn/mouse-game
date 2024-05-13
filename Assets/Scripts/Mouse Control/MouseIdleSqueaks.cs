using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controls a random amount of time for the mouse to squeak while its in the idle animation state
 */
public class MouseIdleSqueaks : MonoBehaviour
{
    private Animator animator;
    public float minDelay = 1f; // Minimum delay before triggering the audio event
    public float maxDelay = 3f; // Maximum delay before triggering the audio event

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IdleAudioRoutine());
    }

    private IEnumerator IdleAudioRoutine()
    {
        while (true)
        {
            // Check if the character is in the idle animation state
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle Turn"))
            {
                // Wait for a random interval of time before triggering the audio event
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);

                // Trigger the audio event
                EventManager.TriggerEvent<MouseSqueakEvent, Vector3>(this.transform.position);
            }

            yield return null;
        }
    }
}
