using UnityEngine;

public class CacoonkSpiderSpawnState : IMobStateMachine
{
    private CacoonkBehaviour cacoonk;
    private float minWaitAfterSpawn = 3f;
    private float maxWaitAfterSpawn = 5f;
    private float waitTimer = 0f;
    private bool isTimerStarted = false;

    public void EnterState(MobBehaviour enemy)
    {
        cacoonk = (CacoonkBehaviour)enemy;

        waitTimer = Random.Range(minWaitAfterSpawn, maxWaitAfterSpawn);

        cacoonk.StopMovement();
        cacoonk.RotateTowardsPlayer();

        cacoonk.PlayAnimationAndExecuteAction(animationName: cacoonk.cacoonkAnimationData.CacoonkSpiderSpawn, onAnimationComplete: () =>
        {
            isTimerStarted = true;
            cacoonk.PlayAnimation(cacoonk.cacoonkAnimationData.CacoonkIdle);
        });
    }

    public void UpdateState(MobBehaviour enemy)
    {
        if (!isTimerStarted) return;

        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
        {
            cacoonk.TransitionToState(new CacoonkPatrollingState());
        }
    }

    public void ExitState(MobBehaviour enemy)
    {

    }
}