using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossBaseState : BossBaseState
{
    protected SpiderBossStateController spiderBossStateController;
    protected SpiderBossAnimationData spiderBossAnimationData;

    protected SpiderBossBaseState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData)
    {
        this.spiderBossStateController = spiderBossStateController;
        this.spiderBossAnimationData = spiderBossAnimationData;
    }

    public override void Enter() { }
    public override void LogicUpdate() { }
    public override void Exit() { }
}
