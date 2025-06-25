using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgilixPatrollingState : IMobStateMachine
{
    private OgilixBehaviour ogilix;

    private float minWaitTime = 1f;
    private float maxWaitTime = 3f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public void EnterState(MobBehaviour enemy)
    {
        ogilix = (OgilixBehaviour)enemy;
        isWaiting = false;
        SetNewPatrolDestination(ogilix);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        ogilix.CheckPlayerState();

        if (ogilix.PlayerInSightRange)
        {
            ogilix.TransitionToState(new OgilixAggroState());
            return;
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                SetNewPatrolDestination(ogilix);
            }
            return;
        }

        if (ogilix.HasReachedDestination())
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
            ogilix.StopMovement();
            ogilix.PlayAnimation(ogilix.ogilixAnimationData.OgilixIdle);
        }
    }

    public void ExitState(MobBehaviour enemy)
    {
        isWaiting = false;
    }

    private void SetNewPatrolDestination(OgilixBehaviour ogilix)
    {
        Vector3 newDestination = ogilix.GetRandomPatrolPoint();
        ogilix.SetNewDestination(newDestination);
        ogilix.ResumeMovement();
        ogilix.SetSpeed(ogilix.walkSpeed);
        ogilix.PlayAnimation(ogilix.ogilixAnimationData.OgilixWalk);
    }
}
