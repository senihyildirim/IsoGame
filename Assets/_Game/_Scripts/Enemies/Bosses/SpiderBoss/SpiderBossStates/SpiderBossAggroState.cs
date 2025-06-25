using System.Collections;
using UnityEngine;

public class SpiderBossAggroState : SpiderBossBaseState
{
    public SpiderBossAggroState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering Aggro State");
    }

    public override void LogicUpdate()
    {
        if (spiderBossStateController.GetPlayerRange() == PlayerRange.OutOfRange)
        {
            spiderBossStateController.PlayAnimation(spiderBossAnimationData.Walk);
            spiderBossStateController.MoveToTarget(PlayerEvents.RaiseGetPlayerPosition());
        }
        else
        {
            spiderBossStateController.StartCoroutine(TransitionToAttackStateWithDelay(1f));
        }
    }

    public override void Exit()
    {
        spiderBossStateController.navMeshAgent.ResetPath();
        Debug.Log("Exiting Aggro State");
    }

    private IEnumerator TransitionToAttackStateWithDelay(float delay)
    {
        spiderBossStateController.PlayAnimation(spiderBossAnimationData.IdleWithAttack);
        yield return new WaitForSeconds(delay);
        spiderBossStateController.TransitionToState(new SpiderBossAttackState(spiderBossStateController, spiderBossAnimationData));
    }
}
