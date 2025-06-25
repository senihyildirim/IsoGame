using UnityEngine;

[CreateAssetMenu(fileName = "BambashAnimationData", menuName = "Mobs/BambashAnimationData")]
public class BambashAnimationData : ScriptableObject
{
    [Header("Basic Animations")]
    public string BambashIdle = "BambashIdle";
    public string BambashWalk = "BambashWalk";
    public string BambashRun = "BambashRun";
    public string BambashDashRun = "BambashDashRun";
    public string BambashHit = "BambashHit";
    public string BambashDizzy = "BambashDizzy";
}
