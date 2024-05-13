using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheeseEventHandler : MonoBehaviour
{

    public Material greenGelMaterial;

    private UnityAction<int> collectCheeseEventListener;
    private bool TriggeredEvent = false;

    void Awake()
    {
        collectCheeseEventListener = new UnityAction<int>(collectCheeseEventHandler);
    }

    void OnEnable()
    {
        EventManager.StartListening<CollectCheeseEvent, int>(collectCheeseEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<CollectCheeseEvent, int>(collectCheeseEventListener);
    }

    void collectCheeseEventHandler(int cheeseAmount)
    {
        if (!TriggeredEvent && GetComponent<PressurePlateTrigger>() != null) 
        {
            PressurePlateTrigger plateInfo = GetComponent<PressurePlateTrigger>();
            if (cheeseAmount >= plateInfo.requiredCheeses) 
            {
                GetComponent<Renderer>().material = greenGelMaterial;
                plateInfo.activateFlash = true;
                TriggeredEvent = true;
            }
        }

        if (!TriggeredEvent && GetComponent<JumpPadTrigger>() != null) 
        {
            JumpPadTrigger jumpPadInfo = GetComponent<JumpPadTrigger>();
            if (cheeseAmount >= jumpPadInfo.requiredCheeses) 
            {
                GetComponent<Renderer>().material = greenGelMaterial;
                jumpPadInfo.activateFlash = true;
                TriggeredEvent = true;
            }
        }
        
    }

}
