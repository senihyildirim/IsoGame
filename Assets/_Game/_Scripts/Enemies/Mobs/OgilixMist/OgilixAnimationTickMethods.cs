using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgilixAnimationTickMethods : MonoBehaviour
{
    private OgilixBehaviour ogilix;

    public void Initialize(OgilixBehaviour ogilix)
    {
        this.ogilix = ogilix;
    }

    public void ThrowPoisonProjectile()
    {
        ogilix.ThrowPoisonProjectile();
    }
}
