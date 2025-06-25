using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgilixPoisionThrowState : IMobStateMachine
{
    private OgilixBehaviour ogilix;
    private bool isAttacking = false;

    public void EnterState(MobBehaviour enemy)
    {
        ogilix = (OgilixBehaviour)enemy;
        StartPoisonThrow();
    }

    public void UpdateState(MobBehaviour enemy)
    {
        if (!isAttacking)
        {
            ogilix.TransitionToState(new OgilixAggroState());
        }
    }

    public void ExitState(MobBehaviour enemy)
    {
        isAttacking = false;
    }

    private void StartPoisonThrow()
    {
        isAttacking = true;
        ogilix.StopMovement();
        ogilix.RotateTowardsPlayer();

        // Play throw animation with callback to spawn projectile
        ogilix.PlayAnimationAndExecuteAction(
            animationName: ogilix.ogilixAnimationData.OgilixPoisonThrow,
            onAnimationComplete: () =>
            {
                isAttacking = false;
            }
        );
    }
}

