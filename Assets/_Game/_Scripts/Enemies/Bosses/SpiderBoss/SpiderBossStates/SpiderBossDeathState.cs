using UnityEngine;

public class SpiderBossDeathState : SpiderBossBaseState
{
    public SpiderBossDeathState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Boss has died");

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.Death, onAnimationComplete: () =>
        {
            spiderBossStateController.gameObject.SetActive(false);
        });
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Death State");
    }
}

