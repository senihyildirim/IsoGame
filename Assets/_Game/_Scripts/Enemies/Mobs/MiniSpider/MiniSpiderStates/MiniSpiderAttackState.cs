using System.Collections;
using UnityEngine;

public class MiniSpiderAttackState : IMobStateMachine
{
    private MiniSpiderBehaviour miniSpider;
    private bool isAttacking = false;
    private float attackCooldown = 1f;

    public void EnterState(MobBehaviour enemy)
    {
        miniSpider = (MiniSpiderBehaviour)enemy;
        miniSpider.StopMovement();
        PerformAttack();
    }

    public void UpdateState(MobBehaviour enemy)
    {
        miniSpider.CheckPlayerState();

        // If not in attack range anymore, go back to chasing
        if (!miniSpider.PlayerInAttackRange)
        {
            miniSpider.TransitionToState(new MiniSpiderAggroState());
            return;
        }

        // If not currently attacking, start a new attack
        if (!isAttacking)
        {
            PerformAttack();
        }
    }

    public void ExitState(MobBehaviour enemy)
    {
        isAttacking = false;
    }

    private void PerformAttack()
    {
        isAttacking = true;

        miniSpider.RotateTowardsPlayer();

        miniSpider.PlayAnimationAndExecuteAction(
            animationName: miniSpider.miniSpiderAnimationData.MiniSpiderAttack,
            onAnimationComplete: () =>
            {
                miniSpider.StartCoroutine(AttackCooldown());
            }
        );
    }

    private IEnumerator AttackCooldown()
    {
        miniSpider.PlayAnimation(miniSpider.miniSpiderAnimationData.MiniSpiderIdle);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

}