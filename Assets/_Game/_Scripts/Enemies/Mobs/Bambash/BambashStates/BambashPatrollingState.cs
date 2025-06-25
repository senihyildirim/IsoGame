using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambashPatrollingState : IMobStateMachine
{
    private BambashBehaviour bambash;
    private float minWaitTime = 1f;
    private float maxWaitTime = 3f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public void EnterState(MobBehaviour enemy)
    {
        bambash = (BambashBehaviour)enemy;
        isWaiting = false;
        SetNewPatrolDestination(bambash);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        bambash.CheckPlayerState();

        if (bambash.PlayerInSightRange)
        {
            bambash.TransitionToState(new BambashAggroState());
            return;
        }

        // Handle patrolling behavior
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                SetNewPatrolDestination(bambash);
            }
            return;
        }

        // Check if reached destination
        if (bambash.HasReachedDestination())
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
            bambash.StopMovement();
            bambash.PlayAnimation(bambash.bambashAnimationData.BambashIdle);
        }
    }

    public void ExitState(MobBehaviour enemy)
    {
        isWaiting = false;
    }

    private void SetNewPatrolDestination(BambashBehaviour bambash)
    {
        Vector3 newDestination = bambash.GetRandomPatrolPoint();
        bambash.SetNewDestination(newDestination);
        bambash.ResumeMovement();
        bambash.SetSpeed(bambash.walkSpeed);
        bambash.PlayAnimation(bambash.bambashAnimationData.BambashWalk);
    }
}