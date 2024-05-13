using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ghostCat : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currWaypoint = -1;
    private float AttackInterval = 4.0f;
    private float AttackBar = 0f;
    private float AttackAni = 0f;
    private int AttackFlag = 0;
    private float AttackLength = 1f;
    private float PatrolSpeed = 3f;
    private float ChaseSpeed = 4.5f;
    private enum AIState
    {
        Idle,
        MovingToWaypoint,
        Chase,
        Attack,
        Fade
    }
    private AIState currentState;
    public GameObject[] waypoints;
    public GameObject mouse;
    public GameObject toy;
    public GameObject mat;
    public GameObject beam;
    public GameObject reward;
    private float distanceToToy = 0f;
    private float distanceToMouse = 0f;
    private float StartChaseDistance = 12f;
    private float SafeDistance = 16f;
    private float FadeDistance = 3f;
    private Renderer objectRenderer;
    private Color initialColor;
    private float newAlpha;

    // Start is called before the first frame update
    void Start()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        objectRenderer = this.GetComponent<Renderer>();
        initialColor = objectRenderer.material.color;
        newAlpha = objectRenderer.material.color.a;
        beam.gameObject.SetActive(false);
        reward.gameObject.SetActive(false);
        currentState = AIState.Idle;
        setNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        switch (currentState)
        {
            case AIState.Idle:
                break;
            case AIState.MovingToWaypoint:
                animator.SetTrigger("Idle");
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Chase");
                distanceToMouse = Vector3.Distance(this.transform.position, mouse.transform.position);
                if (distanceToMouse < StartChaseDistance)
                {
                    currentState = AIState.Chase;
                    break;
                }
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                {
                    setNextWaypoint();
                }
                break;
            case AIState.Chase:
                animator.SetTrigger("Chase");
                animator.ResetTrigger("Idle");
                animator.ResetTrigger("Attack");
                EventManager.TriggerEvent<GhostCatChaseEvent, Vector3>(this.transform.position);
                ChaseState();
                break;
            case AIState.Attack:
                GhostAttack();
                break;
            case AIState.Fade:
                GhostFade();
                break;
            default:
                break;
        }
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        if (AttackBar <= AttackInterval)
        {
            AttackBar += Time.deltaTime;
        }
        distanceToToy = Vector3.Distance(toy.transform.position, mat.transform.position);
        if (distanceToToy < FadeDistance)
        {
            navMeshAgent.SetDestination(toy.transform.position);
            currentState = AIState.Fade;
        }
    }

    private void setNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("There are no waypoints");
            return;
        }

        currWaypoint = (currWaypoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
        navMeshAgent.speed = PatrolSpeed;
        currentState = AIState.MovingToWaypoint;
    }

    private void ChaseState()
    {
        if (mouse == null)
        {
            Debug.LogError("There is no mouse to chase");
            return;
        }

        distanceToMouse = Vector3.Distance(transform.position, mouse.transform.position);
        navMeshAgent.SetDestination(mouse.transform.position);
        navMeshAgent.speed = ChaseSpeed;

        if (!navMeshAgent.pathPending && distanceToMouse < 3f && AttackBar > AttackInterval)
        {
            currentState = AIState.Attack;
        }
        if (distanceToMouse > SafeDistance)
        {
            currentState = AIState.MovingToWaypoint;
        }
    }

    private void GhostAttack()
    {
        if(AttackFlag == 0)
        {
            animator.SetTrigger("Attack");
            animator.ResetTrigger("Chase");
            animator.ResetTrigger("Idle");
            EventManager.TriggerEvent<GhostCatAttackEvent, Vector3>(this.transform.position);
            AttackBar = 0f;
            AttackAni = 0f;
            HealthTracker health = mouse.GetComponent<HealthTracker>();
            if (health != null)
            {
                health.LoseHealth(1);
            }
            AttackFlag = 1;
        }
        if(AttackFlag == 1)
        {
            if (AttackAni < AttackLength)
            {
                AttackAni += Time.deltaTime;
            }
            if (AttackAni > AttackLength)
            {
                distanceToMouse = Vector3.Distance(transform.position, mouse.transform.position);
                if (distanceToMouse < SafeDistance)
                {
                    currentState = AIState.Chase;
                    AttackFlag = 0;
                }
                else
                {
                    currentState = AIState.MovingToWaypoint;
                    AttackFlag = 0;
                }
            }
        }
    }

    private void GhostFade()
    {
        float distance = Vector3.Distance(transform.position, toy.transform.position);
        beam.gameObject.SetActive(true);
        if (distance < 5f)
        {
            EventManager.TriggerEvent<GhostCatFadeEvent, Vector3>(this.transform.position);
            newAlpha = newAlpha - 0.0002f; 

            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);

            objectRenderer.material.color = newColor;

            if (newAlpha <= 0)
            {
                this.gameObject.SetActive(false);
                beam.gameObject.SetActive(false);
                toy.gameObject.SetActive(false);
                reward.gameObject.SetActive(true);
            }
        }
    }
}
