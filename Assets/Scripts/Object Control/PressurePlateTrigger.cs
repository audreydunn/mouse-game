using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressurePlateTrigger : MonoBehaviour
{
    public Animator plateAnimator;
    public Animator otherAnimator;
    public int requiredCheeses = 1;
    public TextMeshPro numberText;
    public string animationBool;
    public float flashInterval = 0.5f; // Time between flashes in seconds
    public bool activateFlash = false;
    public Material greenGelMaterial;
    public Material fadedGreenGelMaterial;

    private bool doActivate = true;
    private bool plateActivatedExit = false;
    private float timer = 0;
    private int flashState = 0;

    void Start()
    {
        if (!plateAnimator)
        {
            plateAnimator = GetComponent<Animator>();
        }
        if (!otherAnimator)
        {
            doActivate = false;
        }

        if (numberText) {
            if (requiredCheeses == 9 || requiredCheeses == 6) 
            {
                numberText.text = "<u>" + requiredCheeses.ToString() + "</u>";
            }
            else 
            {
                numberText.text = requiredCheeses.ToString();
            }
        }
    }

    // Code for causing object to flash
    void Update()
    {
        if (activateFlash) 
        {
            timer += Time.deltaTime;

            if (timer >= flashInterval)
            {
                if (flashState == 0) 
                {
                    GetComponent<Renderer>().material = fadedGreenGelMaterial;
                    flashState = 1;
                }
                else if (flashState == 1) 
                {
                    GetComponent<Renderer>().material = greenGelMaterial;
                    flashState = 0;
                }
                timer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheeseCollector cheeseCollector = other.GetComponent<CheeseCollector>();
            if (cheeseCollector != null && cheeseCollector.cheeseCount >= requiredCheeses)
            {
                if (!plateActivatedExit) 
                {
                    plateAnimator.SetBool("play", true);
                    EventManager.TriggerEvent<PressurePlateEvent, Vector3>(other.transform.position);

                    // Handle turning off the flash
                    activateFlash = false;
                    GetComponent<Renderer>().material = greenGelMaterial;
                }

                if (doActivate) 
                {
                    otherAnimator.SetBool(animationBool, true);
                }
            }
            else
            {
                Debug.Log("Not enough cheese collected.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheeseCollector cheeseCollector = other.GetComponent<CheeseCollector>();
            if (cheeseCollector != null && cheeseCollector.cheeseCount >= requiredCheeses)
            {
                if (!plateActivatedExit) 
                {
                    plateAnimator.SetBool("play", false);
                }

                if (doActivate) 
                {
                    otherAnimator.SetBool(animationBool, false);
                }
                plateActivatedExit = true;
            }
        }
    }
}
