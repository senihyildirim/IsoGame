using UnityEngine;

public class SpiderBossDisabledState : SpiderBossBaseState
{
    public SpiderBossDisabledState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Start with Disabled State");
    }

    public override void LogicUpdate()
    {
        if (spiderBossStateController.IsPlayerInRange(spiderBossStateController.longerRangeDistance))
        {
            spiderBossStateController.TransitionToState(new SpiderBossEncounterState(spiderBossStateController, spiderBossAnimationData));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Disabled State");
    }
}
