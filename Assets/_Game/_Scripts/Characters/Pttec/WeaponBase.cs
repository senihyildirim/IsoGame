using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Properties")]
    public string weaponName = "Weapon"; // Weapon name
    public float attackPower = 20f; // Attack power
    public float attackSpeed = 0.5f; // Attack speed
    public float energyPerShot = 10f; // Energy consumed per shot
    public float ultimateChargePerShot = 5f; // Ultimate charge gained per shot
    public float NeedChargeUlti = 100f; // Ultimate charge required
    public float range = 10f; // Weapon-specific range

    [Header("References")]
    public Transform firePoint; // Firing point
    public ObjectPool projectilePool; // Projectile pool
    private Coroutine lookAtEnemyCoroutine;

    // Abstract method for firing
    public abstract void Fire();

    // Abstract method for fire vfx
    public abstract void PlayFireVFX();

    // Abstract method for using ultimate
    public abstract void UseUltimate();

    // Find the closest enemy within the weapon's range
    public Transform FindClosestEnemy()
    {
        // Include triggers using QueryTriggerInteraction.Collide
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, ~0, QueryTriggerInteraction.Collide);

        Debug.Log($"Found {colliders.Length} colliders in range.");

        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            // Check for MobHealth component directly
            if (collider.TryGetComponent<MobHealth>(out var enemyHealth))
            {

                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }
            else
            {
            }
        }

        return closestEnemy; // Return the closest enemy (or null if none found)
    }



    protected void LookAtEnemy(Transform enemy)
    {
        if (enemy != null)
        {
            // Stop any ongoing rotation coroutine to prevent conflicts
            if (lookAtEnemyCoroutine != null)
            {
                StopCoroutine(lookAtEnemyCoroutine);
            }

            // Start a new coroutine to rotate towards the enemy
            lookAtEnemyCoroutine = StartCoroutine(SmoothLookAtEnemy(enemy));
        }
        else
        {
            // If there's no enemy, stop controlling the rotation
            if (lookAtEnemyCoroutine != null)
            {
                StopCoroutine(lookAtEnemyCoroutine);
                lookAtEnemyCoroutine = null;
            }
        }
    }

    private IEnumerator SmoothLookAtEnemy(Transform enemy)
    {
        while (enemy != null && WeaponController.isFiring) // Keep rotating while firing
        {
            // Calculate direction to the enemy
            Vector3 direction = (enemy.position - transform.parent.parent.position).normalized;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly interpolate towards the target rotation
            transform.parent.parent.rotation = Quaternion.Slerp(
                transform.parent.parent.rotation,
                targetRotation,
                Time.deltaTime * 5f // Adjust interpolation speed
            );

            yield return null; // Wait for the next frame
        }

        // Clear the coroutine reference and let FollowCharacter resume control
        lookAtEnemyCoroutine = null;
    }


}


