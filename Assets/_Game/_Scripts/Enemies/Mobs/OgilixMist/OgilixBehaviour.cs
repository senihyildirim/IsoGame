using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OgilixBehaviour : MobBehaviour
{
    [Header("Ogilix Settings")]
    public OgilixAnimationData ogilixAnimationData;

    [Header("Poison Settings")]
    [SerializeField] private GameObject poisonProjectile;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float poisonProjectileSpeed = 7f;
    [SerializeField] private float poisonProjectileDamage = 10f;
    [SerializeField] private float throwHeight = 1f;

    private OgilixAnimationTickMethods ogilixAnimationTickMethods;
    private bool isThrowingPoison = false;

    protected override void Start()
    {
        base.Start();
        TransitionToState(new OgilixPatrollingState());

        ogilixAnimationTickMethods = GetComponentInChildren<OgilixAnimationTickMethods>();
        ogilixAnimationTickMethods.Initialize(this);
    }

    protected override void Update()
    {
        base.Update();

        if (isThrowingPoison)
        {
            RotateTowardsPlayer();
        }
    }

    // Called by animation event through OgilixAnimationTickMethods
    public void ThrowPoisonProjectile()
    {
        if (poisonProjectile == null || projectileSpawnPoint == null) return;

        isThrowingPoison = true;

        // Calculate throw direction
        Vector3 targetPosition = PlayerEvents.RaiseGetPlayerPosition();
        Vector3 throwDirection = (targetPosition - projectileSpawnPoint.position).normalized;

        // Spawn projectile
        GameObject projectile = Instantiate(poisonProjectile, projectileSpawnPoint.position, Quaternion.identity);

        if (projectile.TryGetComponent<VisualEffect>(out VisualEffect vfx))
        {
            vfx.Play();
        }

        // Set up projectile properties
        if (projectile.TryGetComponent<BulletPoison>(out BulletPoison bulletPoison))
        {
            bulletPoison.Initialize(poisonProjectileDamage);
        }

        // Calculate end position for the throw
        Vector3 endPosition = projectileSpawnPoint.position + throwDirection * poisonProjectileSpeed;

        // Throw the projectile in an arc
        MovementTweener.ThrowToPosition(
            target: projectile.transform,
            endPosition: endPosition,
            height: throwHeight,
            duration: poisonProjectileSpeed / 10f,
            onComplete: () =>
            {
                isThrowingPoison = false;

                if (projectile != null)
                {
                    Destroy(projectile);
                }
            }
        );

    }
}
