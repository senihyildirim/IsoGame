using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSpiderBehaviour : MobBehaviour
{
    [Header("MiniSpider Settings")]
    public MiniSpiderAnimationData miniSpiderAnimationData;

    [Header("Throw Settings")]
    [SerializeField] private float throwDistance = 3f;
    [SerializeField] private float throwDuration = 0.5f;

    public float ThrowDistance => throwDistance;
    public float ThrowDuration => throwDuration;

    protected override void Start()
    {
        base.Start();
        TransitionToState(new MiniSpiderSpawnState());
    }

    // Override to show only attack range
    protected override void OnDrawGizmos()
    {
        // Only draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackAreaRadius);
    }
}
