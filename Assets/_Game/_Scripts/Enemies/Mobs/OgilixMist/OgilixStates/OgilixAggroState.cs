using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgilixAggroState : IMobStateMachine
{
    private OgilixBehaviour ogilix;
    private float updatePathInterval = 0.5f;
    private float pathUpdateTimer;

    public void EnterState(MobBehaviour enemy)
    {
        ogilix = (OgilixBehaviour)enemy;
        ogilix.SetTargetPlayerDestination();
        ogilix.PlayAnimation(ogilix.ogilixAnimationData.OgilixRun);
    }

    public void UpdateState(MobBehaviour enemy)
    {

        ogilix.CheckPlayerState();

        if (!ogilix.PlayerInSightRange)
        {
            ogilix.TransitionToState(new OgilixPatrollingState());
            return;
        }

        if (ogilix.PlayerInAttackRange)
        {
            ogilix.TransitionToState(new OgilixPoisionThrowState());
            return;
        }

        // Update path to player periodically
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0)
        {
            pathUpdateTimer = updatePathInterval;
            ogilix.SetTargetPlayerDestination();
        }
    }

    public void ExitState(MobBehaviour enemy) { }
}
