using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacoonkAnimationTickMethods : MonoBehaviour
{
    private CacoonkBehaviour cacoonk;

    public void Initialize(CacoonkBehaviour cacoonk)
    {
        this.cacoonk = cacoonk;
    }

    public void SpawnMiniSpiders()
    {
        cacoonk.SpawnMiniSpiders();
    }

    public void PlaySpawnMiniSpidersVFX()
    {
        cacoonk.PlaySpawnMiniSpidersVFX();
    }

    public void StopSpawnMiniSpidersVFX()
    {
        cacoonk.StopSpawnMiniSpidersVFX();
    }
}
