using UnityEngine;

[CreateAssetMenu(fileName = "MiniSpiderAnimationData", menuName = "Mobs/MiniSpiderAnimationData")]
public class MiniSpiderAnimationData : ScriptableObject
{
    [Header("Basic Animations")]
    public string MiniSpiderSpawn = "MiniSpiderSpawn";
    public string MiniSpiderIdle = "MiniSpiderIdle";
    public string MiniSpiderRun = "MiniSpiderRun";
    public string MiniSpiderAttack = "MiniSpiderAttack";
}