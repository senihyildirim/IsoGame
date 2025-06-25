using UnityEngine;

public class SpiderBossSandHoseState : SpiderBossBaseState
{
    private const int NUMBER_OF_HOSES = 3;
    private const float HOSE_THROW_SPEED = 20f;
    private const float ARC_ANGLE = 60f;
    private const float HOSE_LIFETIME = 4f;

    public SpiderBossSandHoseState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering Sand Hose State");
        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.SandHoseAttack, onAnimationComplete: ThrowHosesInCircle);
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Sand Hose State");
    }

    private void ThrowHosesInCircle()
    {
        Vector3 shootPosition = spiderBossStateController.transform.position;
        Vector3 directionToPlayer = GetDirectionToPlayer(shootPosition);

        SpawnHoses(shootPosition, directionToPlayer);

        spiderBossStateController.TransitionToState(
            new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData)
        );
    }

    private Vector3 GetDirectionToPlayer(Vector3 shootPosition)
    {
        Vector3 playerPosition = PlayerEvents.RaiseGetPlayerPosition();
        return (playerPosition - shootPosition).normalized;
    }

    private void SpawnHoses(Vector3 shootPosition, Vector3 directionToPlayer)
    {
        float angleStep = ARC_ANGLE / (NUMBER_OF_HOSES - 1);
        float startAngle = -ARC_ANGLE / 2;

        for (int i = 0; i < NUMBER_OF_HOSES; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            SpawnSingleHose(shootPosition, directionToPlayer, currentAngle);
        }
    }

    private void SpawnSingleHose(Vector3 position, Vector3 direction, float angle)
    {
        GameObject hose = Object.Instantiate(spiderBossStateController.hosePrefab, position, Quaternion.identity);
        Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * direction;

        SetupHosePhysics(hose, rotatedDirection);
        Object.Destroy(hose, HOSE_LIFETIME);
    }

    private void SetupHosePhysics(GameObject hose, Vector3 direction)
    {
        Rigidbody hoseRb = hose.GetComponent<Rigidbody>();
        if (hoseRb == null)
        {
            Debug.LogError("HosePrefab does not have a Rigidbody component!");
            return;
        }

        hoseRb.isKinematic = false;
        hoseRb.useGravity = false;
        hoseRb.velocity = direction * HOSE_THROW_SPEED;
    }
}
