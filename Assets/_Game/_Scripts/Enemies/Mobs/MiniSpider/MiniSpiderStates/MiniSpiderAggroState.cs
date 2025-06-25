using UnityEngine;

public class MiniSpiderAggroState : IMobStateMachine
{
    private MiniSpiderBehaviour miniSpider;
    private float updatePathInterval = 0.2f;
    private float pathUpdateTimer;

    public void EnterState(MobBehaviour enemy)
    {
        miniSpider = (MiniSpiderBehaviour)enemy;
        miniSpider.SetTargetPlayerDestination();
        miniSpider.PlayAnimation(miniSpider.miniSpiderAnimationData.MiniSpiderRun);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        miniSpider.CheckPlayerState();

        // If in attack range, transition to attack state
        if (miniSpider.PlayerInAttackRange)
        {
            miniSpider.TransitionToState(new MiniSpiderAttackState());
            return;
        }

        // Update path to player frequently
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0)
        {
            pathUpdateTimer = updatePathInterval;
            miniSpider.SetTargetPlayerDestination();
        }
    }

    public void ExitState(MobBehaviour enemy)
    {
        // Nothing special to clean up
    }
}