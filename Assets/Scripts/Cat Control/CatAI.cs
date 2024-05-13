using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool mouseSeen;
    private bool mouseHeard;
    private bool mouseChased;
    private bool reachedLastKnownMousePosition;
    private int curAlertLevel;
    private int idleCycles;
    public GameObject player;
    public float catTriggerDistance = 10f;
    public GameObject[] waypoints;
    public int curWaypoint;
    public float catTimer;
    public int baseAlertLevel = 0;
    public float sleepChance = 0.1f;
    public float wakeUpChance = 0.2f;
    public float minSleepTime = 1f;
    public float minWaypointTime = 1f;
    public float searchTime = 5f;
    public int idleCyclesToLowerAlert = 3;
    public float maxObstacleThicknessForHearing = 2f;

    private enum State
    {
        Sleeping,  // Random chance after x seconds when idle? maybe 100% chance if mouse leaves catnip for the cat in one of its spots?
        Idle,
        IdleWalk, //moving to next idle waypoint
        Search, // Heard something or lost sight of mouse, look for mouse 
        Chase,  // seeing mouse
    };

    private State currentState;

    private void Awake()
    {
        mouseSeen = false;
        mouseHeard = false;
        mouseChased = false;
        reachedLastKnownMousePosition = false;
        curAlertLevel = baseAlertLevel;
        idleCycles = 0;
        catTimer = 0f;
        curWaypoint = -1;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Must set an initial state for the cat
        currentState = State.Idle;
        curWaypoint = -1;
        setNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        CatSense();
        catTimer += Time.deltaTime;
        switch (currentState)
        {
            case State.Sleeping:
                // Transition to idle state randomly after some sleep time
                if (catTimer > minSleepTime)
                {
                    if (Random.Range(0, 1f) <= wakeUpChance)
                    {
                        animator.SetTrigger("WakeUp");
                        currentState = State.Idle;
                        ++curAlertLevel;
                        catTimer = 0f;
                        Debug.Log("Transition from Sleeping to Idle");
                    }
                }
                if (mouseHeard) //if players makes sufficient noise, wake up and search
                {
                    // Mouse has been heard, start searching
                    animator.SetTrigger("WakeUp");
                    currentState = State.Search;
                    curAlertLevel += 2; //more alert bc straight from sleep to search
                    catTimer = 0;
                    Debug.Log("Transition from Sleeping to Search");
                }
                break;
            
            case State.Idle:
                if (mouseHeard)
                {
                    // Mouse has been heard, start searching
                    currentState = State.Search;
                    catTimer = 0;
                    Debug.Log("Transition from Idle to Search");
                }
                else 
                {
                    if (catTimer > minWaypointTime)
                    {
                        ++idleCycles;
                        if (curAlertLevel > baseAlertLevel && idleCycles > idleCyclesToLowerAlert)
                        {
                            --curAlertLevel;
                            idleCycles = 0;
                        }
                        // go to next point or maybe start sleeping
                        catTimer = 0;
                        if (curAlertLevel <= 1 && Random.Range(0, 1f) <= sleepChance)
                        {
                            animator.SetTrigger("GoToSleep");
                            currentState = State.Sleeping;
                            --curAlertLevel;
                            Debug.Log("Transition from Idle to Sleeping");
                        }
                        else
                        {
                            // Start idle walking to random waypoints
                            setNextWaypoint();
                            currentState = State.IdleWalk;
                            Debug.Log("Transition from Idle to IdleWalk");
                        }
                    }
                }
                break;

            case State.IdleWalk:
                // Scale the forward motion to quarter speed for idle walking
                UpdateMotionParameters(0.25f);

                if (mouseHeard)
                {
                    // Mouse has been heard, start searching
                    catTimer = 0;
                    currentState = State.Search;
                    Debug.Log("Transition from IdleWalk to Search");
                }
                //else continue to waypoint, then idle
                else
                {
                    if (navMeshAgent.remainingDistance <= 1f && !navMeshAgent.pathPending)
                    {
                        // animator.SetTrigger("Idle");  stop walking
                        catTimer = 0;
                        currentState = State.Idle;
                        Debug.Log("Transition from IdleWalk to Idle");
                    }
                }
                break;

            case State.Search:
                // Scale the forward motion to helf speed for searching
                UpdateMotionParameters(0.5f);

                if (mouseSeen)
                {
                    // Target mouse if it is seen
                    catTimer = 0f;
                    currentState = State.Chase;
                    Debug.Log("Transition from Search to Chase");
                }
                else
                {
                    if (mouseHeard)
                    {
                        // Move toward mouse if it is heard
                        // animator.SetTrigger("Walk");  start walking
                        navMeshAgent.SetDestination(player.transform.position);
                        reachedLastKnownMousePosition = false;
                    }
                    else
                    {
                        // continue moving to last known position
                        if(!reachedLastKnownMousePosition && navMeshAgent.remainingDistance <= 1f && !navMeshAgent.pathPending)
                        {
                            reachedLastKnownMousePosition = true;
                            catTimer = 0;
                        }
                        if (reachedLastKnownMousePosition)
                        {
                            // animator.SetTrigger("Idle");  stop walking
                            if (catTimer > searchTime)
                            {
                                // back to a waypoint
                                // animator.SetTrigger("Walk");  start walking
                                currentState = State.IdleWalk;
                                curWaypoint = Random.Range(0, waypoints.Length);
                                setNextWaypoint();
                                catTimer = 0;
                                Debug.Log("Transition from Search to IdleWalk");
                                if (mouseChased && curAlertLevel < 5)
                                {
                                    ++curAlertLevel;
                                }
                                mouseChased = false;
                            }
                        }
                    }
                }
                break;

            case State.Chase:
                mouseChased = true;

                // Scale the forward motion to full speed for chase
                UpdateMotionParameters(1f);

                if (mouseSeen)
                {
                    // animator.SetTrigger("Run");  start running
                    navMeshAgent.SetDestination(player.transform.position);
                    reachedLastKnownMousePosition = false;
                }
                else
                {
                    if (navMeshAgent.remainingDistance <= 1f && !navMeshAgent.pathPending)
                    {
                        // animator.SetTrigger("Idle");  stop running
                        reachedLastKnownMousePosition = true;
                        currentState = State.Search;
                        catTimer = 0;
                        Debug.Log("Transition from Chase to Search");
                    }
                }
                break;
        }

    }

    // Updates the x and y velocity of the cat using a scaling factor
    // A factor of 1 would be full speed, a factor of 0.5 would be half speed, etc.
    private void UpdateMotionParameters(float scalingFactor)
    {
        float normalizedY = navMeshAgent.velocity.y / (navMeshAgent.speed * scalingFactor);
        float normalizedX = navMeshAgent.velocity.x / (navMeshAgent.angularSpeed * scalingFactor);
        animator.SetFloat("Vel_Y", normalizedY);
        animator.SetFloat("Vel_X", normalizedX);
    }

    // Determines if the mouse is within a certain range from the cat AI
    private void CatSense()
    {
        // Distance between cat and mouse
        float catToMouseDist = Vector3.Distance(this.transform.position, player.transform.position);

        // Ignore collisions with the player
        int playerMask = LayerMask.GetMask("Player");

        // Check if mouse is within some certain mininum distance from cat
        if (catToMouseDist < catTriggerDistance * (1 + 0.2 * curAlertLevel))
        {
            // Cast a ray from cat to mouse, ignoring collisions with player. True means ray hit an obstacle.
            Ray catToMouseRay = new(this.transform.position, Vector3.Normalize(player.transform.position - this.transform.position));
            if (Physics.Raycast(catToMouseRay, out RaycastHit hit, catToMouseDist, playerMask))
            {
                // No direct line of sight, so check obstacle thickness to see if cat should be able to hear mouse
                Ray mouseToCatRay = new(player.transform.position, Vector3.Normalize(this.transform.position - player.transform.position)); // secondary ray to get obstacle thickness
                Physics.Raycast(mouseToCatRay, out RaycastHit hit2, catToMouseDist);

                float obstacleThickness = catToMouseDist - hit2.distance - hit.distance;
                catToMouseDist -= 2 * (catToMouseDist - hit2.distance - hit.distance); // 2*obstacle width, as if noise absorbsion, unless there is a better way
                // TODO:
                // if player is running -> distance = distance*2
                // if player is standing still -> distance = 0 OR define a mouseBreathingNoiseDistance;
                if (catToMouseDist > catTriggerDistance * (1 + 0.2 * curAlertLevel))
                {
                    // Cat heard activity, go to check
                    mouseSeen = false;
                    mouseHeard = true;
                    //return true;
                }
                else
                {
                    //mouse at triggerDistance, but no line of sight and obstacles block noise
                    mouseSeen = false;
                    mouseHeard = false;
                    //return false;
                }
            }
            else  
            {
                // visual contact. Chase mouse
                mouseSeen = true;
                mouseHeard = true;
                //return true;
            }
        }
        else
        {
            mouseSeen = false;
            mouseHeard = false;
            //return false;
        }
    }

    private void setNextWaypoint()
    {
        ++curWaypoint;
        if (waypoints.Length == 0 || waypoints is null)
        {
            Debug.Log("Error 404: waypoint list not found");
        }
        else
        {
            if (curWaypoint >= waypoints.Length)
            {
                curWaypoint = 0;
            }
        }
        navMeshAgent.SetDestination(waypoints[curWaypoint].transform.position);
    }

}
