using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpPadTrigger : MonoBehaviour
{
    public int requiredCheeses = 1;
    public float jumpForce = 100f;
    public float forwardForce = 10f;
    public TextMeshPro numberText;
    public float flashInterval = 0.5f; // Time between flashes in seconds
    public bool activateFlash = false;
    public Material greenGelMaterial;
    public Material fadedGreenGelMaterial;

    private float timer = 0;
    private int flashState = 0;

    void Start()
    {
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
        if (other.GetComponent<Rigidbody>() != null)
        {
            if (other.CompareTag("Player"))
            {
                CheeseCollector cheeseCollector = other.GetComponent<CheeseCollector>();
                if (cheeseCollector != null && cheeseCollector.cheeseCount >= requiredCheeses)
                {
                    float extraJumpForce = 0f;

                    JumpPadTracker padTracker = other.GetComponent<JumpPadTracker>();
                    if (padTracker != null)
                    {
                        if (padTracker.lastJumpPad == 0f && gameObject != padTracker.getLastJumpPadRef()) 
                        {
                            extraJumpForce = padTracker.lastJumpPad;
                        }
                        padTracker.lastJumpPad = jumpForce;
                        padTracker.setLastJumpPadRef(gameObject);
                    }

                    jumpObject(other, extraJumpForce);

                    // Handle turning off the flash
                    activateFlash = false;
                    GetComponent<Renderer>().material = greenGelMaterial;
                }
                else
                {
                    Debug.Log("Not enough cheese collected.");
                }
            }
            else 
            {
                jumpObject(other, 0f);
            }
        }
    }

    void jumpObject(Collider other, float addJumpForce) 
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        EventManager.TriggerEvent<BoingEvent, Vector3>(this.transform.position);

        // Apply an upwards force to the object
        rb.AddForce(Vector3.up * (jumpForce + addJumpForce) + Vector3.forward * forwardForce, ForceMode.Impulse);
    }

}
