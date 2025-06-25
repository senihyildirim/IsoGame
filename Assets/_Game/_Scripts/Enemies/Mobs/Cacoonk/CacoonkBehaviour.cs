using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class CacoonkBehaviour : MobBehaviour
{
    [Header("Cacoonk Settings")]
    public CacoonkAnimationData cacoonkAnimationData;

    [Header("Spider Spawn Settings")]
    [SerializeField] private GameObject miniSpiderPrefab;
    [SerializeField] private Transform spiderSpawnPoint;
    [SerializeField] private int spiderSpawnCount = 10;
    [SerializeField] private VisualEffect spawnVFX;

    private float spiderSpawnDelay;
    private CacoonkAnimationTickMethods cacoonkAnimationTickMethods;

    protected override void Start()
    {
        base.Start();
        TransitionToState(new CacoonkPatrollingState());
        float spiderSpawnAnimationLength = GetAnimationLength(cacoonkAnimationData.CacoonkSpiderSpawn);
        spiderSpawnDelay = spiderSpawnAnimationLength / spiderSpawnCount / 2f;

        cacoonkAnimationTickMethods = GetComponentInChildren<CacoonkAnimationTickMethods>();
        cacoonkAnimationTickMethods.Initialize(this);
    }

    public void SpawnMiniSpiders()
    {
        StartCoroutine(SpawnMiniSpidersCoroutine());
    }

    private IEnumerator SpawnMiniSpidersCoroutine()
    {
        for (int i = 0; i < spiderSpawnCount; i++)
        {
            Instantiate(miniSpiderPrefab, spiderSpawnPoint.position, transform.rotation);
            yield return new WaitForSeconds(spiderSpawnDelay);
        }
    }

    public void PlaySpawnMiniSpidersVFX()
    {
        spawnVFX.Play();
    }

    public void StopSpawnMiniSpidersVFX()
    {
        spawnVFX.Stop();
    }
}
