using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(Seeker))]
public class MobBehaviour : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;
    [SerializeField] private string currentStateName;
    [SerializeField] private string currentAnimationState;

    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    [SerializeField] protected float patrolAreaRadius;
    [SerializeField] protected float sightAreaRadius;
    [SerializeField] protected float attackAreaRadius;

    private Transform mainPlayer;
    private Animator animator;
    private IEnumerator executeAfterDelayCoroutine;
    private IMobStateMachine currentState;
    private AIPath aiPath;
    private Seeker seeker;
    private Vector3 patrolAreaCenter;
    private bool playerInSightRange;
    public bool PlayerInSightRange { get { return playerInSightRange; } }
    private bool playerInAttackRange;
    public bool PlayerInAttackRange { get { return playerInAttackRange; } }

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= OnPlayerDeath;
    }

    protected virtual void Start()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        animator = GetComponentInChildren<Animator>();

        mainPlayer = PlayerEvents.RaiseGetPlayerTransform();
        patrolAreaCenter = transform.position;

        SetSpeed(walkSpeed);
    }

    protected virtual void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void TransitionToState(IMobStateMachine newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentStateName = currentState.GetType().Name;
        currentState.EnterState(this);
    }

    public virtual void CheckPlayerState()
    {
        if (mainPlayer == null || !mainPlayer.gameObject.activeInHierarchy)
        {
            playerInSightRange = false;
            playerInAttackRange = false;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, mainPlayer.position);
        playerInSightRange = distanceToPlayer <= sightAreaRadius;
        playerInAttackRange = distanceToPlayer <= attackAreaRadius;
    }

    #region Pathfinding Movement
    public Vector3 GetRandomPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolAreaRadius;
        Vector3 randomPoint = patrolAreaCenter + new Vector3(randomCircle.x, transform.position.y, randomCircle.y);

        // Get nearest point on graph
        var nearest = AstarPath.active.GetNearest(randomPoint);

        // If point is too far from patrol area, use current position
        if (Vector3.Distance(nearest.position, patrolAreaCenter) > patrolAreaRadius * 1.5f)
        {
            return transform.position;
        }

        return nearest.position;
    }

    public void SetNewDestination(Vector3 target)
    {
        if (seeker.IsDone())
        {
            if (debugLogs)
            {
                // Check if positions are on the graph
                var startNode = AstarPath.active.GetNearest(transform.position).node;
                var endNode = AstarPath.active.GetNearest(target).node;

                if (startNode == null)
                {
                    Debug.LogError($"Start position {transform.position} is not on any graph!");
                    return;
                }

                if (endNode == null)
                {
                    Debug.LogError($"Target position {target} is not on any graph!");
                    return;
                }
            }

            // Debug.Log($"Setting new destination: {target}");
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            // Debug.Log($"Path found from {transform.position} to target");
            aiPath.SetPath(p);
        }
        else
        {
            // Debug.LogError($"Path calculation error: {p.errorLog}\nStart: {transform.position}");
        }
    }

    public bool HasReachedDestination()
    {
        return aiPath.reachedDestination;
    }

    public void ResumeMovement()
    {
        aiPath.canMove = true;
    }

    public void StopMovement()
    {
        aiPath.canMove = false;
    }

    public void SetSpeed(float speed)
    {
        aiPath.maxSpeed = speed;
    }

    public void SetTargetPlayerDestination()
    {
        Vector3 targetPosition = PlayerEvents.RaiseGetPlayerPosition();
        SetNewDestination(targetPosition);
        ResumeMovement();
        SetSpeed(runSpeed);
    }

    public void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (PlayerEvents.RaiseGetPlayerPosition() - transform.position).normalized;
        directionToPlayer.y = 0; // Keep rotation only on Y axis
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
    #endregion

    #region Animation
    public void PlayAnimation(string animationName, bool isOverride = false, float crossFadeTime = 0.1f)
    {
        PlayAnimationAndExecuteAction(animationName, isOverride, crossFadeTime);
    }

    public void PlayAnimationAndExecuteAction(string animationName, bool isOverride = false, float crossFadeTime = 0.1f, System.Action onAnimationComplete = null)
    {
        if (animator == null) return;
        if (!isOverride && currentAnimationState == animationName) return;

        StopExecuteAfterDelay();
        animator.CrossFade(animationName, crossFadeTime);
        currentAnimationState = animationName;

        if (onAnimationComplete != null)
        {
            float animationLength = GetAnimationLength(animationName);
            executeAfterDelayCoroutine = ExecuteAfterDelay(animationLength, onAnimationComplete);
            StartCoroutine(executeAfterDelayCoroutine);
        }
    }

    private IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        executeAfterDelayCoroutine = null;
        action?.Invoke();
    }

    private void StopExecuteAfterDelay()
    {
        if (executeAfterDelayCoroutine != null)
        {
            StopCoroutine(executeAfterDelayCoroutine);
            executeAfterDelayCoroutine = null;
        }
    }

    public float GetAnimationLength(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        Debug.Log("Animation not found: " + animationName);
        return 1f; // Default length if not found
    }
    #endregion

    public void Die()
    {
        // Disable the A* pathfinding components
        aiPath.enabled = false;
        seeker.enabled = false;

        // Disable the EnemyBehavior script
        this.enabled = false;

        // Disable the collider
        GetComponent<BoxCollider>().enabled = false;

        // Play death animation
        PlayAnimation("Death");

        gameObject.tag = "Untagged";
    }

    private void OnPlayerDeath()
    {
        // When player dies
    }

    protected virtual void OnDrawGizmos()
    {
        // Draw patrol area if radius > 0
        if (patrolAreaRadius > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, patrolAreaRadius);
        }

        // Draw sight range if radius > 0
        if (sightAreaRadius > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightAreaRadius);
        }

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackAreaRadius);
    }
}
