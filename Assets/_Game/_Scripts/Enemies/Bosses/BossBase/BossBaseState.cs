public abstract class BossBaseState
{
    private BossStateController bossStateController;
    private BossAnimationData bossAnimationData;

    protected BossBaseState(BossStateController bossStateController, BossAnimationData bossAnimationData)
    {
        this.bossStateController = bossStateController;
        this.bossAnimationData = bossAnimationData;
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void Exit() { }
}
