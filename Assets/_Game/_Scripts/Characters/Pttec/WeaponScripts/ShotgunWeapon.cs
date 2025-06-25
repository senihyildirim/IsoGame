using UnityEngine;
using UnityEngine.VFX;

public class ShotgunWeapon : WeaponBase
{
    public Transform[] firePoints; // Multiple firing points

    public VisualEffect leftGunVFX; // VFX reference for the left gun
    public VisualEffect rightGunVFX; // VFX reference for the right gun

    public Transform ultifirePoint;
    public GameObject shotgunUltiPrefab;
    public GameObject ultimateImpactVFXPrefab; // VFX for the ultimate impact

    public override void Fire()
    {
        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
        {
            Debug.LogWarning("No enemies in range!");
            return;
        }

        LookAtEnemy(closestEnemy); // Rotate towards the enemy

        foreach (Transform firePoint in firePoints)
        {
            SpawnProjectile(firePoint, closestEnemy);
        }

        PlayFireVFX();
    }

    public override void PlayFireVFX()
    {
        if (leftGunVFX != null)
        {
            leftGunVFX.Play();
        }

        if (rightGunVFX != null)
        {
            rightGunVFX.Play();
        }
    }

    public override void UseUltimate()
    {
        Debug.Log($"{weaponName} Ultimate activated!");

        // Only play VFX for ultimate
        if (ultimateImpactVFXPrefab != null)
        {
            GameObject impactVFX = Instantiate(ultimateImpactVFXPrefab, transform.position, Quaternion.identity);
            Destroy(impactVFX, 3f); // Destroy after 3 seconds
        }
        // ShotgunUlti prefab'ını ateşle
        if (shotgunUltiPrefab != null && ultifirePoint != null)
        {
            // ShotgunUlti prefab'ını firePoint'te instantiate et
            Instantiate(shotgunUltiPrefab, ultifirePoint.position, ultifirePoint.rotation);
            Debug.Log("ShotgunUlti prefab fired!");
        }
    }

    private void SpawnProjectile(Transform firePoint, Transform target)
    {
        GameObject projectile = projectilePool.GetObject();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(target.position - firePoint.position);

        LaserProjectile laserProjectile = projectile.GetComponent<LaserProjectile>();
        if (laserProjectile != null)
        {
            laserProjectile.Initialize(projectilePool);
        }
        else
        {
            Debug.LogError("LaserProjectile component not found on projectile prefab!");
        }
    }
}
