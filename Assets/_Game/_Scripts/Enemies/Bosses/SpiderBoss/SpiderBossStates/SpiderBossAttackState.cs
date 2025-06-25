using UnityEngine;

public class SpiderBossAttackState : SpiderBossBaseState
{
    private const float ROTATION_SPEED = 5.0f;
    private const float ROTATION_THRESHOLD = 5f;
    private const float ATTACK_COOLDOWN = 1f;

    private readonly SpiderBossHealth spiderBossHealth;

    private bool hasTurned = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float lastAttackTime = 0f;

    public SpiderBossAttackState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData)
    {
        spiderBossHealth = spiderBossStateController.GetComponent<SpiderBossHealth>();
    }

    public override void Enter()
    {
        Debug.Log("Entering Attack State");

        if (IsOnCooldown())
        {
            TransitionToAggroState();
            return;
        }

        lastAttackTime = Time.time;
        isAttacking = true;
    }

    public override void LogicUpdate()
    {
        if (!isAttacking) return;

        attackTimer += Time.deltaTime;

        if (attackTimer < ATTACK_COOLDOWN) return;

        if (CheckHealthStateTransitions()) return;

        if (!hasTurned)
        {
            RotateTowardsPlayer();
            return;
        }

        ExecuteAttackBasedOnRange();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        isAttacking = false;
        attackTimer = 0f;
    }

    private bool IsOnCooldown()
    {
        return Time.time - lastAttackTime < ATTACK_COOLDOWN;
    }

    private bool CheckHealthStateTransitions()
    {
        if (spiderBossHealth.currentHealthState == SpiderBossHealthState.Low &&
            spiderBossStateController.canEnterUpState)
        {
            TransitionToState(new SpiderBossUpState(spiderBossStateController, spiderBossAnimationData));
            spiderBossStateController.canEnterUpState = false;
            return true;
        }

        if (spiderBossHealth.currentHealthState == SpiderBossHealthState.Medium &&
            spiderBossStateController.canEnterShieldState)
        {
            TransitionToState(new SpiderBossSandShieldState(spiderBossStateController, spiderBossAnimationData));
            spiderBossStateController.canEnterShieldState = false;
            return true;
        }

        return false;
    }

    private void ExecuteAttackBasedOnRange()
    {
        switch (spiderBossStateController.GetPlayerRange())
        {
            case PlayerRange.Short:
                ExecuteShortRangeAttack();
                break;
            case PlayerRange.Mid:
                ExecuteMidRangeAttack();
                break;
            case PlayerRange.Long:
                ExecuteLongRangeAttack();
                break;
            case PlayerRange.Longer:
                ExecuteDiveAttack();
                break;
        }
    }

    private void ExecuteShortRangeAttack()
    {
        spiderBossStateController.PlayAnimationAndExecuteAction(
            animationName: spiderBossAnimationData.BasicAttack,
            onAnimationComplete: TransitionToAggroState
        );
    }

    private void ExecuteMidRangeAttack()
    {
        if (Random.value < 0.5f)
        {
            TransitionToState(new SpiderBossSandHoseState(spiderBossStateController, spiderBossAnimationData));
        }
        else
        {
            TransitionToState(new SpiderBossSandTrapState(spiderBossStateController, spiderBossAnimationData));
        }
    }

    private void ExecuteLongRangeAttack()
    {
        if (Random.value < 0.5f)
        {
            TransitionToState(new SpiderBossThrowWebState(spiderBossStateController, spiderBossAnimationData));
        }
        else
        {
            TransitionToState(new SpiderBossCallSpidersState(spiderBossStateController, spiderBossAnimationData));
        }
    }

    private void ExecuteDiveAttack()
    {
        //TransitionToState(new SpiderBossDiveGroundState(spiderBossStateController, spiderBossAnimationData));
        TransitionToState(new SpiderBossThrowWebState(spiderBossStateController, spiderBossAnimationData));
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerPosition = PlayerEvents.RaiseGetPlayerPosition();
        Vector3 directionToPlayer = (playerPosition - spiderBossStateController.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        spiderBossStateController.transform.rotation = Quaternion.Slerp(
            spiderBossStateController.transform.rotation,
            targetRotation,
            Time.deltaTime * ROTATION_SPEED
        );

        if (Quaternion.Angle(spiderBossStateController.transform.rotation, targetRotation) < ROTATION_THRESHOLD)
        {
            CompleteRotation();
        }
    }

    private void CompleteRotation()
    {
        hasTurned = true;
    }

    private void TransitionToAggroState()
    {
        TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));
    }

    private void TransitionToState(SpiderBossBaseState newState)
    {
        spiderBossStateController.TransitionToState(newState);
    }
}
