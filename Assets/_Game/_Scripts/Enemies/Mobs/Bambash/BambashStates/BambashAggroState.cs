using UnityEngine;

public class BambashAggroState : IMobStateMachine
{
    private BambashBehaviour bambash;
    private float updatePathInterval = 0.5f;
    private float pathUpdateTimer;

    public void EnterState(MobBehaviour enemy)
    {
        bambash = (BambashBehaviour)enemy;
        bambash.SetTargetPlayerDestination();
        bambash.PlayAnimation(bambash.bambashAnimationData.BambashRun);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        bambash.CheckPlayerState();

        if (!bambash.PlayerInSightRange)
        {
            bambash.TransitionToState(new BambashPatrollingState());
            return;
        }

        if (bambash.PlayerInAttackRange)
        {
            bambash.TransitionToState(new BambashDashAttackState());
            return;
        }

        // Update path to player periodically
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0)
        {
            pathUpdateTimer = updatePathInterval;
            bambash.SetTargetPlayerDestination();
        }
    }

    public void ExitState(MobBehaviour enemy) { }
}
