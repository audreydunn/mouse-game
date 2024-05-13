using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableCheese : MonoBehaviour
{
    private Animator animator;
    public string animationBool = "PlayerNearby";
    private bool isAnimationPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter animation collision");
        if (other.attachedRigidbody != null && other.CompareTag("Player"))
        {
            // Play animation
            animator.SetBool(animationBool, true);
            isAnimationPlaying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && other.CompareTag("Player"))
        {
            StartCoroutine(DelayedStopAnimation());
        }
    }

    IEnumerator DelayedStopAnimation()
    {
        yield return new WaitUntil(() => !isAnimationPlaying);
        animator.SetBool(animationBool, false);
    }

    public void OnAnimationComplete()
    {
        isAnimationPlaying = false;
    }
}
