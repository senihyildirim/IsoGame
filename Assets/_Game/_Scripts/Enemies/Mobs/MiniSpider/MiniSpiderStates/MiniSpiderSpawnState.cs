using UnityEngine;
using DG.Tweening;

public class MiniSpiderSpawnState : IMobStateMachine
{
    private MiniSpiderBehaviour miniSpider;

    public void EnterState(MobBehaviour enemy)
    {
        miniSpider = (MiniSpiderBehaviour)enemy;
        miniSpider.StopMovement();

        // Throw with spin effect
        MovementTweener.ThrowInDirection(
            target: miniSpider.transform,
            direction: miniSpider.transform.forward,
            distance: miniSpider.ThrowDistance,
            height: 0f,
            duration: miniSpider.ThrowDuration,
            onComplete: () =>
            {
                miniSpider.TransitionToState(new MiniSpiderAggroState());
            }
        );

        miniSpider.PlayAnimation(miniSpider.miniSpiderAnimationData.MiniSpiderSpawn);
    }

    public void UpdateState(MobBehaviour enemy)
    {
        // State handled by DOTween sequence
    }

    public void ExitState(MobBehaviour enemy)
    {
        // Nothing to clean up
    }
}