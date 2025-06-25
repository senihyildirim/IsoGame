using UnityEngine;

public class SpiderBossEncounterState : SpiderBossBaseState
{
    public SpiderBossEncounterState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering Encounter State");

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.Encounter, onAnimationComplete: () =>
        {
            spiderBossStateController.TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));
        });
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Encounter State");
    }
}
