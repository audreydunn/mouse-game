using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Animator), typeof(Rigidbody))]
[RequireComponent(typeof(CharacterInputController))]
public class SmoothMouseControl : MonoBehaviour
{
    // Reference Animator and Rigidbody components.
    private Animator anim;
    private Rigidbody rbody;

    // Reference CharacterInputController component.
    private CharacterInputController cinput;

    // Add animationSpeed, rootMovementSpeed, and rootTurnSpeed (default values of 1f).
    public float animationSpeed = 1f;
    public float rootMovementSpeed = 1f;
    public float rootTurnSpeed = 1f;
    public float backwardsSpeed = 5f;

    // Constant input measures like axes can just have most recent value cached.
    float _inputForward = 0f;
    float _inputTurn = 0f;

    // Jump variables. Jump, Falling, Landing Reference: https://www.youtube.com/watch?v=sJvWmFYSQFY
    public float jumpForce = 5f;
    private float fallingSpeed;
    private bool jumpEnabled;
    private float fallForwardMultiplier;

    // Attach a Stamina Bar UI element here.
    private float jumpDelay;
    private float jumpDelayCount;
    public float jumpDelayRecoverSpeed = 1.5f;
    public Slider jumpSlider;

    // Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    public float airControlSpeed = 2.0f; // Control speed of the player in the air
    public float maxAirHorizontalChange = 0.2f; // Maximum rate of change in horizontal direction


    // For checking ground contact.
    private int groundContactCount = 0;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    // Call even before the first frame update.
    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("Animator could not be found");
        rbody = GetComponent<Rigidbody>();
        if (rbody == null)
            Debug.Log("Rigid body could not be found");
        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInputController could not be found");
    }

    // Use this for any initialization.
    void Start()
    {
        Physics.defaultMaxDepenetrationVelocity = 10f;

        // Enable initial jump.
        jumpDelay = 2f;
        jumpDelayCount = 2f;
        jumpSlider.value = jumpDelayCount;
        jumpEnabled = true;

        // Initialize backwards movement speed modifier.
        backwardsSpeed = 5f;
    }

    // Detect if spacebar clicked.
    private void Update()
    {
        if (cinput.enabled)
        {
            _inputForward = cinput.Forward;
            _inputTurn = cinput.Turn;

            // Check for jump input. Make sure the mouse is grounded and is not moving backwards before jumping.
            // Also, make sure the jump button is off the 2 second cooldown. Also, some fine tuning.
            if (Input.GetButtonDown("Jump") && IsGrounded 
                && jumpEnabled)
            {
                Jump();
            }
        }

        // Detect if walking backwards.
        DetectWalkingBack();

        // Implement jump delay timer.
        JumpTimer();

        // Account for subtlely moving when falling.
        MoveOnFall();
    }

    void FixedUpdate()
    {
        // Set the Animator Component�s speed to the animationSpeed scalar.
        anim.speed = animationSpeed;

        // onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        // Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        // Therefore, an additional raycast approach is used to check for close ground
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.85f, 0f, out closeToJumpableGround);

        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        anim.SetBool("isFalling", !isGrounded);

        // If jumping or falling, set isJumping to false, to transition to falling state.
        fallingSpeed = rbody.velocity.y;
        if (anim.GetBool("isFalling") && fallingSpeed > 0)
        {
            anim.SetBool("isJumping", false);
        }
        else if (anim.GetBool("isJumping") && fallingSpeed > 0)
        {
            anim.SetBool("isJumping", false);
        }
    }


    // This is a physics callback
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == "ground")
        {
            ++groundContactCount;
            //EventManager.TriggerEvent<MinionLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }
    }

    void Jump()
    {
        // Apply vertical force for jump
        rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        anim.SetBool("isJumping", true);

        // Begin cooldown timer.
        jumpDelayCount = 0f;
    }

    void JumpTimer()
    {
        // Implement jump delay timer. Set to 2 seconds.
        if (jumpDelayCount <= jumpDelay)
        {
            // Increment the timer if not 2 seconds yet.
            jumpDelayCount += Time.deltaTime * jumpDelayRecoverSpeed;
            jumpEnabled = false;
        }
        else
        {
            jumpEnabled = true;
        }
        jumpSlider.value = jumpDelayCount;
    }

    void DetectWalkingBack()
    {
        // Detect if walking backwards.
        if (anim.GetFloat("vely") <= -0.025f)
        {
            anim.SetBool("isWalkingBack", true);
        }
        else
        {
            anim.SetBool("isWalkingBack", false);
        }
    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.85f, 0f, out closeToJumpableGround);

        if (isGrounded)
        {
            // Use root motion as is if on the ground        
            newRootPosition = anim.rootPosition;
        }
        else
        {
            // Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        // Use rotational root motion as is
        newRootRotation = anim.rootRotation;

        // Increase backwards walking movement here
        if (anim.GetBool("isWalkingBack"))
        {
            // Scale the difference in position going backwards to make the character go faster or slower
            newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, backwardsSpeed);
        }
        else
        {
            // Scale the difference in position to make the character go faster or slower
            newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);
        }

        // Scale the difference in rotation to make the character go faster or slower
        newRootRotation = Quaternion.SlerpUnclamped(this.transform.rotation, newRootRotation, rootTurnSpeed);

        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }

    void MoveOnFall()
    {
        // Account for moving while falling.
        if (anim.GetBool("isFalling"))
        {
            // Detect if the inputs are around non-zero (Player is moving forward and/or turning).
            if (rbody.velocity.y < 0 || rbody.velocity.y > 0 && (_inputForward != 0f || _inputTurn != 0f))
            {
                // Calculate the desired air movement direction based on inputs, normalized to ensure consistent speed.
                Vector3 airMovementDirection = transform.forward * _inputForward + transform.right * _inputTurn;
                airMovementDirection = airMovementDirection.normalized * airControlSpeed; // airControlSpeed controls the air movement sensitivity.

                // Apply a force that attempts to move the player in the desired direction, 
                // but doesn't affect the vertical (y-axis) velocity.
                Vector3 currentHorizontalVelocity = new Vector3(rbody.velocity.x, 0.0f, rbody.velocity.z);
                Vector3 desiredHorizontalVelocity = new Vector3(airMovementDirection.x, 0.0f, airMovementDirection.z);

                // Calculate the difference between the current and desired velocity vectors,
                // then apply that difference as a force to adjust the player's velocity towards the desired direction.
                Vector3 velocityChange = desiredHorizontalVelocity - currentHorizontalVelocity;
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAirHorizontalChange, maxAirHorizontalChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAirHorizontalChange, maxAirHorizontalChange);

                rbody.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }
    }
}
