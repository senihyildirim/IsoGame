using UnityEngine;

public class CacoonkAggroState : IMobStateMachine
{
    private CacoonkBehaviour cacoonk;
    private float updatePathInterval = 0.5f;
    private float pathUpdateTimer;

    public void EnterState(MobBehaviour enemy)
    {
        cacoonk = (CacoonkBehaviour)enemy;
        cacoonk.SetTargetPlayerDestination();
        cacoonk.PlayAnimation(cacoonk.cacoonkAnimationData.CacoonkRun);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        cacoonk.CheckPlayerState();

        if (!cacoonk.PlayerInSightRange)
        {
            cacoonk.TransitionToState(new CacoonkPatrollingState());
            return;
        }

        if (cacoonk.PlayerInAttackRange)
        {
            cacoonk.TransitionToState(new CacoonkSpiderSpawnState());
            return;
        }

        // Update path to player periodically
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0)
        {
            pathUpdateTimer = updatePathInterval;
            cacoonk.SetTargetPlayerDestination();
        }
    }

    public void ExitState(MobBehaviour enemy) { }
}