using UnityEngine;

public class CacoonkPatrollingState : IMobStateMachine
{
    private CacoonkBehaviour cacoonk;
    private float minWaitTime = 1f;
    private float maxWaitTime = 3f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public void EnterState(MobBehaviour enemy)
    {
        cacoonk = (CacoonkBehaviour)enemy;
        isWaiting = false;
        SetNewPatrolDestination(cacoonk);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        cacoonk.CheckPlayerState();

        if (cacoonk.PlayerInSightRange)
        {
            cacoonk.TransitionToState(new CacoonkAggroState());
            return;
        }

        // Handle patrolling behavior
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                SetNewPatrolDestination(cacoonk);
            }
            return;
        }

        // Check if reached destination
        if (cacoonk.HasReachedDestination())
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
            cacoonk.StopMovement();
            cacoonk.PlayAnimation(cacoonk.cacoonkAnimationData.CacoonkIdle);
        }
    }

    public void ExitState(MobBehaviour enemy)
    {
        isWaiting = false;
    }

    private void SetNewPatrolDestination(CacoonkBehaviour cacoonk)
    {
        Vector3 newDestination = cacoonk.GetRandomPatrolPoint();
        cacoonk.SetNewDestination(newDestination);
        cacoonk.ResumeMovement();
        cacoonk.SetSpeed(cacoonk.walkSpeed);
        cacoonk.PlayAnimation(cacoonk.cacoonkAnimationData.CacoonkWalk);
    }
}