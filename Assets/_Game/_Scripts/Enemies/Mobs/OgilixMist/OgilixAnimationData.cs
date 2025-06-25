using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OgilixAnimationData", menuName = "Mobs/OgilixAnimationData")]
public class OgilixAnimationData : ScriptableObject
{
    [Header("Basic Animations")]
    public string OgilixIdle = "OgilixIdle";
    public string OgilixWalk = "OgilixWalk";
    public string OgilixRun = "OgilixRun";
    public string OgilixPoisonThrow = "OgilixPoisonThrow";
}
