using UnityEngine;

[CreateAssetMenu(fileName = "CacoonkAnimationData", menuName = "Mobs/CacoonkAnimationData")]
public class CacoonkAnimationData : ScriptableObject
{
    [Header("Basic Animations")]
    public string CacoonkIdle = "CacoonkIdle";
    public string CacoonkWalk = "CacoonkWalk";
    public string CacoonkRun = "CacoonkRun";

    [Header("Attack")]
    public string CacoonkSpiderSpawn = "CacoonkSpiderSpawn";
}
