using UnityEngine;

public class SpiderBossAnimationMethods : MonoBehaviour
{
    [SerializeField] private SpiderBossAnimationData spiderBossAnimationData;
    private SpiderBossStateController spiderBossStateController;

    private void Start()
    {
        spiderBossStateController = GetComponentInParent<SpiderBossStateController>();
    }

    public void PlayClimbEvent()
    {
        SpiderBossUpState spiderBossUpState = new SpiderBossUpState(spiderBossStateController, spiderBossAnimationData);
        spiderBossUpState.ClimbEvent();
    }

    public void PlayThrowWebEvent()
    {
        SpiderBossThrowWebState spiderBossThrowWebState = new SpiderBossThrowWebState(spiderBossStateController, spiderBossAnimationData);
        spiderBossThrowWebState.ThrowWebEvent();
    }

    public void PlayCallSpidersEvent()
    {
        SpiderBossCallSpidersState spiderBossCallSpidersState = new SpiderBossCallSpidersState(spiderBossStateController, spiderBossAnimationData);
        spiderBossCallSpidersState.CallSpidersEvent();
    }

    public void PlaySandTrapEvent()
    {
        SpiderBossSandTrapState spiderBossSandTrapState = new SpiderBossSandTrapState(spiderBossStateController, spiderBossAnimationData);
        spiderBossSandTrapState.SandTrapEvent();
    }
}
