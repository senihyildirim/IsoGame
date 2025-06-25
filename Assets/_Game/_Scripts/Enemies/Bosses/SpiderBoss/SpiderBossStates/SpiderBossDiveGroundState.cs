using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SpiderBossDiveGroundState : SpiderBossBaseState
{
    private const float DIVE_DISTANCE = 10f;   // Distance to dive underground
    private const float MOVE_SPEED = 5f;       // Underground movement speed
    private const float POSITION_THRESHOLD = 0.1f;  // Threshold for position checking

    private Vector3 playerPosition;
    private Collider spiderBossCollider;
    private NavMeshAgent navMeshAgent;

    public SpiderBossDiveGroundState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData)
    {
        playerPosition = PlayerEvents.RaiseGetPlayerPosition();
        spiderBossCollider = spiderBossStateController.bossCollider;
        navMeshAgent = spiderBossStateController.navMeshAgent;
    }

    public override void Enter()
    {
        Debug.Log("Entering Dive Ground State");

        if (spiderBossCollider != null)
            spiderBossCollider.enabled = false;

        if (navMeshAgent != null)
            navMeshAgent.enabled = false;

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.DiveIn, onAnimationComplete: () =>
        {
            spiderBossStateController.StartCoroutine(DiveSequence());
        });
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Dive Ground State");

        if (spiderBossCollider != null)
            spiderBossCollider.enabled = true;

        if (navMeshAgent != null)
            navMeshAgent.enabled = true;
    }

    private IEnumerator DiveSequence()
    {
        yield return StartDive();
        yield return MoveUnderground();
        yield return SurfaceAtPlayer();

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.DiveOut, onAnimationComplete: () =>
        {
            spiderBossStateController.TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));
        });
    }

    private IEnumerator StartDive()
    {
        Vector3 divePosition = CalculateDivePosition();
        yield return MoveToPosition(divePosition);
    }

    private IEnumerator MoveUnderground()
    {
        Vector3 targetPosition = CalculateUndergroundTargetPosition();
        yield return MoveToPosition(targetPosition);
    }

    private IEnumerator SurfaceAtPlayer()
    {
        Vector3 surfacePosition = CalculateSurfacePosition();
        yield return MoveToPosition(surfacePosition);
    }

    private Vector3 CalculateDivePosition()
    {
        return new Vector3(
            spiderBossStateController.transform.position.x,
            spiderBossStateController.transform.position.y - DIVE_DISTANCE,
            spiderBossStateController.transform.position.z
        );
    }

    private Vector3 CalculateUndergroundTargetPosition()
    {
        return new Vector3(
            playerPosition.x,
            spiderBossStateController.transform.position.y,
            playerPosition.z
        );
    }

    private Vector3 CalculateSurfacePosition()
    {
        return new Vector3(
            spiderBossStateController.transform.position.x,
            spiderBossStateController.transform.position.y + DIVE_DISTANCE,
            spiderBossStateController.transform.position.z
        );
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(spiderBossStateController.transform.position, targetPosition) > POSITION_THRESHOLD)
        {
            spiderBossStateController.transform.position = Vector3.MoveTowards(
                spiderBossStateController.transform.position,
                targetPosition,
                MOVE_SPEED * Time.deltaTime
            );
            yield return null;
        }
    }
}
