using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAIForrestEdits : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private GameObject catBody;
    private bool mouseSeen;
    private bool mouseHeard;
    public GameObject player;
    public float catHearingDistance = 10f;
    public GameObject[] waypoints;
    public int curWaypoint;
    public float maxObstacleThicknessForHearing = 2f;
    private float timeLeftInPassiveState;   // Remaining time in either the sleeping or patrolling state
    public float passiveStateTime = 4f;
    public float attackDistance = 5f;       // Distance from which the cat will attempt a pounce attack
    private VelocityReporter velReporter;
    private Vector3 predictedMousePosition;
    public GameObject catTarget;
    public float waitTimeAfterAttack = 10f;
    private bool attackWaitTimeExpired = true;
    private bool isAttacking = false;
    private bool hasCollided = false;
    [SerializeField] private UIManager eventSystem;

    public enum State
    {
        Sleeping,   // Sleeping at a waypoint
        Patrolling, // Walking between waypoints
        Searching,  // Walking toward where the mouse was heard
        Chasing,    // Running toward mouse location
    };

    public State currentState;

    private void Awake()
    {
        mouseSeen = false;
        mouseHeard = false;
        curWaypoint = -1;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        velReporter = player.GetComponent<VelocityReporter>();
        catBody = this.transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Must set an initial state for the cat
        timeLeftInPassiveState = passiveStateTime;
        currentState = State.Patrolling;
        animator.SetTrigger("MoveForward");
        curWaypoint = -1;
        setNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {

        // Don't do any updates while attacking logic is happening
        if (isAttacking)
        {
            return;
        }

        CatSense();

        // Check if cat should be moving or not
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree - Forward"))
        {
            navMeshAgent.isStopped = false;
        }
        else
        {
            navMeshAgent.isStopped = true;
        }

        switch (currentState)
        {
            case State.Sleeping:

                // Sometimes cat gets stuck in Idle animation here when it should be sitting down
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("SitDown");
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sleeping"))
                {
                    EventManager.TriggerEvent<CatSleepingEvent, Vector3>(this.transform.position);

                    timeLeftInPassiveState -= Time.deltaTime;
                    if (mouseSeen)
                    {
                        // Go straight to chasing state if mouse seen
                        timeLeftInPassiveState = passiveStateTime;
                        animator.SetTrigger("WakeUp");
                        //animator.SetTrigger("MoveForwardSkipIdle");
                        animator.SetTrigger("MoveForward");
                        EventManager.TriggerEvent<CatChirpEvent, Vector3>(this.transform.position);
                        currentState = State.Chasing;
                        catChasingMessage();
                    }
                    else if ((mouseHeard && !mouseSeen) || timeLeftInPassiveState <= 0)
                    {
                        // Sleeping time is up or the mouse is in range
                        timeLeftInPassiveState = passiveStateTime;
                        animator.SetTrigger("WakeUp");
                        animator.SetTrigger("MoveForward");
                        currentState = State.Patrolling;
                    }
                }
                break;

            case State.Patrolling:

                timeLeftInPassiveState -= Time.deltaTime;

                // On Enter
                UpdateMotionParameters(0.5f);

                if (mouseHeard && !mouseSeen)
                {
                    // On Exit
                    timeLeftInPassiveState = passiveStateTime;
                    navMeshAgent.SetDestination(player.transform.position);
                    UpdateIndicator(catTarget, player.transform.position);
                    EventManager.TriggerEvent<CatChirpEvent, Vector3>(this.transform.position);
                    currentState = State.Searching;
                    catSearchingMessage();
                }
                else if (mouseSeen)
                {
                    // On Exit
                    timeLeftInPassiveState = passiveStateTime;
                    PredictMouseLocation();
                    navMeshAgent.SetDestination(predictedMousePosition);
                    UpdateIndicator(catTarget, predictedMousePosition);
                    EventManager.TriggerEvent<CatChirpEvent, Vector3>(this.transform.position);
                    currentState = State.Chasing;
                    catChasingMessage();
                }
                else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    // Made it to a way point
                    if (timeLeftInPassiveState <= 0)
                    {
                        // Patrolling time is up
                        timeLeftInPassiveState = passiveStateTime;
                        animator.SetFloat("Forward_Vel", 0);
                        animator.SetFloat("Vel_X", 0);
                        animator.SetTrigger("SitDown");
                        animator.SetTrigger("GoToSleep");
                        currentState = State.Sleeping;
                    }
                    else
                    {
                        // Continue walking between waypoints if mouse is not seen nor heard
                        setNextWaypoint();
                    }
                }
                break;

            case State.Searching:
                UpdateMotionParameters(0.75f);
                if (mouseSeen)
                {
                    // If mouse is seen, switch to chase
                    PredictMouseLocation();
                    navMeshAgent.SetDestination(predictedMousePosition);
                    UpdateIndicator(catTarget, predictedMousePosition);
                    currentState = State.Chasing;
                    catChasingMessage();
                }

                // Sometimes cat gets stuck in Idle animation here when it should be moving forward
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("MoveForward");
                }

                else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    // Reached last heard mouse position, decide what to do next
                    if (!mouseHeard && !mouseSeen)
                    {
                        // Lost the mouse, go back to patrolling
                        navMeshAgent.SetDestination(waypoints[curWaypoint].transform.position);
                        UpdateIndicator(catTarget, waypoints[curWaypoint].transform.position);
                        currentState = State.Patrolling;
                        catReturnToPatrol();
                    }
                    else if (mouseHeard)
                    {
                        // Mouse is still heard, go to the next place you hear it
                        navMeshAgent.SetDestination(player.transform.position);
                        UpdateIndicator(catTarget, player.transform.position);
                    }
                }
                break;

            case State.Chasing:
                UpdateMotionParameters(0.8f);

                // Continually update target if chasing
                PredictMouseLocation();
                navMeshAgent.SetDestination(predictedMousePosition);
                UpdateIndicator(catTarget, predictedMousePosition);

                // Decide if close enough to attack
                if (navMeshAgent.remainingDistance < attackDistance)
                {
                    //navMeshAgent.isStopped = true;

                    // Orient the cat to face the player
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed);

                    if (!isAttacking)
                    {
                        StartCoroutine(AttackAndWait());
                    }
                }

                if (!mouseSeen && mouseHeard)
                {
                    // On Exit
                    navMeshAgent.SetDestination(player.transform.position);
                    UpdateIndicator(catTarget, player.transform.position);
                    currentState = State.Searching;
                    catSearchingMessage();
                }
                else if (!mouseSeen && !mouseHeard)
                {
                    // On Exit
                    navMeshAgent.SetDestination(waypoints[curWaypoint].transform.position);
                    UpdateIndicator(catTarget, waypoints[curWaypoint].transform.position);
                    currentState = State.Patrolling;
                    catReturnToPatrol();
                }

                break;
        }
        //Debug.Log("Current State: " + currentState);
        //Debug.Log("Mouse Heard: " + mouseHeard);
        //Debug.Log("Mouse Seen: " + mouseSeen);

    }

    // Waits for the attack animation to play before letting the agent move on the nav mesh again
    IEnumerator AttackAndWait()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        Debug.Log("Attack animation triggered");
        EventManager.TriggerEvent<CatGrowlEvent, Vector3>(this.transform.position);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump/Attack"))
        {
            navMeshAgent.isStopped = true;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            // Attack animation is finished here
            animator.ResetTrigger("Attack");
            if (!hasCollided)
            {
                // Cat did not hit mouse, keep walking around
                animator.SetTrigger("MoveForward");
                navMeshAgent.isStopped = false;
            }

        }
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player") && attackWaitTimeExpired)
        {
            hasCollided = true;
            attackWaitTimeExpired = false;
            isAttacking = true;

            // Mouse has been captured
            HealthTracker health = player.GetComponent<HealthTracker>();
            if (health != null)
            {
                health.LoseHealth(1);
            }
            animator.SetTrigger("SitDown");
            animator.ResetTrigger("Attack");

            if (health.curHealth > 0)
            {
                StartCoroutine(WaitAfterAttack());
            }
        }

    }

    IEnumerator WaitAfterAttack()
    {
        yield return new WaitForSeconds(waitTimeAfterAttack);

        attackWaitTimeExpired = true;
        isAttacking = false;
        hasCollided = false;
        navMeshAgent.isStopped = false;
        animator.SetTrigger("MoveForward");
        //animator.SetTrigger("MoveForwardSkipIdle");
        currentState = State.Patrolling;
        

    }

    // Predicts the future mouse location based on a velocity heuristic
    private void PredictMouseLocation()
    {
        if (velReporter == null)
        {
            Debug.LogError("Velocity Reporter not assigned");
            return;
        }

        // Predict future mouse location
        float catToMouseDist = Vector3.Distance(this.transform.position, player.transform.position);
        float lookAheadTime = catToMouseDist / navMeshAgent.speed;
        lookAheadTime = Mathf.Clamp(lookAheadTime, 0.0f, 1.0f);
        predictedMousePosition = player.transform.position + lookAheadTime * velReporter.velocity;

        // Check if predicted position goes outside the nav mesh area
        NavMeshHit hit;
        bool blocked = NavMesh.Raycast(player.transform.position, predictedMousePosition, out hit, NavMesh.AllAreas);
        if (blocked)
        {
            // Predicted posotion is outside navmesh so use the closest point
            predictedMousePosition = hit.position;
        }

    }

    // Updates the x and y velocity of the cat using a scaling factor
    // A factor of 1 would be full speed, a factor of 0.5 would be half speed, etc.
    private void UpdateMotionParameters(float scalingFactor)
    {
        float forwardVel = Vector3.Dot(navMeshAgent.velocity, transform.forward);

        //float normalizedY = navMeshAgent.velocity / (navMeshAgent.speed * scalingFactor);
        float normalizedX = navMeshAgent.velocity.x / (navMeshAgent.angularSpeed * scalingFactor);
        animator.SetFloat("Forward_Vel", forwardVel * scalingFactor);
        animator.SetFloat("Vel_X", normalizedX);
    }

    // Determines if the mouse is within a certain range from the cat AI
    private void CatSense()
    {
        // Distance from cat to mouse
        float catToMouseDist = Vector3.Distance(catBody.transform.position, player.transform.position);

        // Ignore collisions with the player or cat
        int playerMask = ~(1 << LayerMask.NameToLayer("Player"));
        int catMask = ~(1 << LayerMask.NameToLayer("Cat"));
        int mask = playerMask & catMask;


        // Check if mouse is within some certain mininum distance from cat AND player is near the floor
        if (catToMouseDist < catHearingDistance && player.transform.position.y < 2)
        {
            // Cast a ray from cat to mouse, ignoring collisions with player. True means ray hit an obstacle.
            Vector3 mouseDir = Vector3.Normalize(player.transform.position - catBody.transform.position);
            Ray catToMouseRay = new(catBody.transform.position, mouseDir);

            Debug.DrawLine(catBody.transform.position, catBody.transform.position + mouseDir * catToMouseDist, Color.red, 0.5f);
            if (Physics.Raycast(catToMouseRay, out RaycastHit hit, catToMouseDist, mask))
            {
                // No direct line of sight, so check obstacle thickness to see if cat should be able to hear mouse
                Ray mouseToCatRay = new(player.transform.position, Vector3.Normalize(this.transform.position - player.transform.position));
                Physics.Raycast(mouseToCatRay, out RaycastHit hit2, catToMouseDist);

                float obstacleThickness = catToMouseDist - hit2.distance - hit.distance;

                if (obstacleThickness < maxObstacleThicknessForHearing)
                {
                    // Cat heard activity, go to check
                    mouseSeen = false;
                    mouseHeard = true;
                }
                else
                {
                    // Mouse is within triggerDistance, but no line of sight and obstacles block noise
                    mouseSeen = false;
                    mouseHeard = false;
                }
            }
            else  
            {
                // visual contact. Chase mouse
                mouseSeen = true;
                mouseHeard = true;
            }
        }
        else
        {
            // Mouse is outside of trigger distance
            mouseSeen = false;
            mouseHeard = false;
        }
    }

    private void setNextWaypoint()
    {
        if (waypoints.Length == 0 || waypoints is null)
        {
            Debug.Log("Error 404: waypoint list not found");
            return;
        }

        curWaypoint = (curWaypoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[curWaypoint].transform.position);
        UpdateIndicator(catTarget, waypoints[curWaypoint].transform.position);
    }

    // Updates the position of a look ahead indicator
    private void UpdateIndicator(GameObject indicator, Vector3 position)
    {
        indicator.transform.position = position;
    }

    private void catChasingMessage()
    {
        eventSystem.addMessageUIM("The cat started chasing you. Run!!");
    }

    private void catSearchingMessage()
    {
        eventSystem.addMessageUIM("The cat heard you. quick, hide or get out of its range!");
    }

    private void catReturnToPatrol()
    {
        eventSystem.addMessageUIM("The cat got bored, it is returning to patrol");
    }
}
