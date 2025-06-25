using UnityEngine;

public class SpiderBossCallSpidersState : SpiderBossBaseState
{
    private const int SPAWN_COUNT = 10;
    private const float SPAWN_RADIUS = 5f;
    private const float SPAWN_HEIGHT_OFFSET = 5f;

    public SpiderBossCallSpidersState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering Call Spiders State");

        spiderBossStateController.PlayAnimationAndExecuteAction(animationName: spiderBossAnimationData.RockAttack, onAnimationComplete: () =>
        {
            spiderBossStateController.TransitionToState(new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData));
        });
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Call Spiders State");
    }

    public void CallSpidersEvent()
    {
        SpawnSpiders();
    }

    private void SpawnSpiders()
    {
        for (int i = 0; i < SPAWN_COUNT; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            SpawnSingleSpider(spawnPosition);
        }

        Debug.Log($"Spawned {SPAWN_COUNT} spiders");
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 bossPosition = spiderBossStateController.transform.position;
        Vector3 randomOffset = Random.insideUnitSphere * SPAWN_RADIUS;

        // Keep the spawn height consistent
        randomOffset.y = SPAWN_HEIGHT_OFFSET;

        return bossPosition + randomOffset;
    }

    private void SpawnSingleSpider(Vector3 spawnPosition)
    {
        GameObject.Instantiate(
            spiderBossStateController.spiderPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}
