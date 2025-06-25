using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerRange
{
    Short,
    Mid,
    Long,
    Longer,
    OutOfRange
}

public abstract class BossStateController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;
    [SerializeField] protected string currentStateName;
    [SerializeField] protected PlayerRange currentPlayerRange;

    [Header("Animation")]
    [SerializeField] protected Animator animator;

    [Header("Attack Ranges")]
    public float shortRangeDistance = 10f;
    public float midRangeDistance = 20f;
    public float longRangeDistance = 30f;
    public float longerRangeDistance = 40f;

    [HideInInspector] public Collider bossCollider;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    protected BossStateMachine stateMachine;
    protected string currentAnimationState;

    private IEnumerator executeAfterDelayCoroutine;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Initialize Animator
        bossCollider = GetComponent<Collider>(); // Initialize Collider
        navMeshAgent = GetComponent<NavMeshAgent>(); // Initialize NavMeshAgent

        stateMachine = new BossStateMachine();
        currentPlayerRange = GetPlayerRange();
    }

    protected virtual void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
        UpdateCurrentStateName();
        UpdateCurrentPlayerRange();
    }

    public virtual void TransitionToState(BossBaseState newState)
    {
        stateMachine.ChangeState(newState);
        UpdateCurrentStateName();
    }

    #region Animation
    public virtual void PlayAnimation(string animationName, bool isOverride = false, float crossFadeTime = 0.1f)
    {
        PlayAnimationAndExecuteAction(animationName, isOverride, crossFadeTime);
    }

    public virtual void PlayAnimationAndExecuteAction(string animationName, bool isOverride = false, float crossFadeTime = 0.1f, System.Action onAnimationComplete = null)
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

    protected virtual void UpdateCurrentStateName()
    {
        currentStateName = stateMachine.CurrentState != null ? stateMachine.CurrentState.GetType().Name : "No State";
    }

    protected virtual void UpdateCurrentPlayerRange()
    {
        currentPlayerRange = GetPlayerRange();
    }

    public virtual PlayerRange GetPlayerRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerEvents.RaiseGetPlayerPosition());

        if (distanceToPlayer <= shortRangeDistance)
        {
            return PlayerRange.Short;
        }
        else if (distanceToPlayer <= midRangeDistance)
        {
            return PlayerRange.Mid;
        }
        else if (distanceToPlayer <= longRangeDistance)
        {
            return PlayerRange.Long;
        }
        else if (distanceToPlayer <= longerRangeDistance)
        {
            return PlayerRange.Longer;
        }
        else
        {
            return PlayerRange.OutOfRange;
        }
    }

    public virtual bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, PlayerEvents.RaiseGetPlayerPosition()) < range;
    }

    public virtual void MoveToTarget(Vector3 target)
    {
        if (target != null && navMeshAgent != null)
        {
            navMeshAgent.SetDestination(target);
        }
    }

    public virtual bool HasReachedDestination()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shortRangeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, midRangeDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, longRangeDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, longerRangeDistance);
    }
}

