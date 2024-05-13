using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadTracker : MonoBehaviour
{
    public float lastJumpPad = 0f;
    private GameObject lastJumpPadRef;

    public void setLastJumpPadRef(GameObject x) 
    {
        lastJumpPadRef = x;
    }
    public GameObject getLastJumpPadRef() 
    {
        return lastJumpPadRef;
    }

    void Update()
    {
        SmoothMouseControl mouseController = GetComponent<SmoothMouseControl>();
        if (mouseController != null && mouseController.IsGrounded)
        {
            lastJumpPad = 0f;
        }
    }
}
