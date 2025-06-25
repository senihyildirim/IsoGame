using UnityEngine;

public class SpiderBossSandTrapState : SpiderBossBaseState
{
    private const float SPAWN_RADIUS = 10f;    // Radius around the player
    private const int NUMBER_OF_TRAPS = 3;    // Number of traps to spawn
    private const float TRAP_Y_OFFSET = 0f;   // Fixed Y position for traps

    public SpiderBossSandTrapState(SpiderBossStateController spiderBossStateController, SpiderBossAnimationData spiderBossAnimationData)
        : base(spiderBossStateController, spiderBossAnimationData) { }

    public override void Enter()
    {
        Debug.Log("Entering Sand Trap State");
        spiderBossStateController.PlayAnimation(spiderBossAnimationData.SandTrap);
    }

    public override void LogicUpdate() { }

    public override void Exit()
    {
        Debug.Log("Exiting Sand Trap State");
    }

    public void SandTrapEvent()
    {
        SpawnTrapsAroundPlayer();

        spiderBossStateController.TransitionToState(
            new SpiderBossAggroState(spiderBossStateController, spiderBossAnimationData)
        );
    }

    private void SpawnTrapsAroundPlayer()
    {
        Vector3 playerPosition = PlayerEvents.RaiseGetPlayerPosition();

        for (int i = 0; i < NUMBER_OF_TRAPS; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition(playerPosition);
            SpawnSingleTrap(spawnPosition);
        }
    }

    private Vector3 CalculateSpawnPosition(Vector3 playerPosition)
    {
        Vector3 randomOffset = Random.insideUnitSphere * SPAWN_RADIUS;
        randomOffset.y = TRAP_Y_OFFSET;

        return playerPosition + randomOffset;
    }

    private void SpawnSingleTrap(Vector3 spawnPosition)
    {
        GameObject.Instantiate(
            spiderBossStateController.sandTrapPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}
