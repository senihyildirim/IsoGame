using UnityEngine;

public class BambashDashAttackState : IMobStateMachine
{
    private BambashBehaviour bambash;

    public void EnterState(MobBehaviour enemy)
    {
        bambash = (BambashBehaviour)enemy;
        bambash.StopMovement();
        bambash.PlayAnimation(bambash.bambashAnimationData.BambashDashRun);
        bambash.StartDash();
    }

    public void UpdateState(MobBehaviour enemy) { }

    public void ExitState(MobBehaviour enemy) { }
}
