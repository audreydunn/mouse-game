using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovingPlatform : MonoBehaviour
{
    public int requiredCheeses = 1;
    public GameObject objectToAnimate;
    private Animator animator;
    public string animationTrigger;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheeseCollector cheeseCollector = other.GetComponent<CheeseCollector>();
            if (cheeseCollector != null && cheeseCollector.cheeseCount >= requiredCheeses)
            {
                objectToAnimate.GetComponent<Animator>().SetTrigger(animationTrigger);
                animator.SetTrigger("PressButton");
            }
            else
            {
                Debug.Log("Not enough cheese collected.");
            }
        }
    }
}