using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BoxingGlovesWeapon : WeaponBase
{

    public Transform[] firePoints; // Multiple firing points

    public ParticleSystem leftGunVFX; // VFX reference for the left gun
    public ParticleSystem rightGunVFX; // VFX reference for the right gun

    public GameObject ultimateStartVFXPrefab; // VFX for the ultimate impact

    public GameObject ultimateImpactVFXPrefab; // VFX for the ultimate impact

    public GameObject WeaponHeader; // The weapon header to toggle visibility
    public GameObject bigBoxingGlovePrefab; // Prefab for the ultimate
    public GameObject ultimateTargetCirclePrefab; // Prefab for the targeting circle

    private GameObject activeTargetCircle; // Instance of the targeting circle
    private Vector3 selectedTargetPosition;
    private bool isChoosingTarget = false;

    public override void Fire()
    {
        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
        {
            Debug.LogWarning("No enemies in range!");
            return;
        }

        LookAtEnemy(closestEnemy); // Rotate towards the enemy

        StartCoroutine(SpawnProjectilesWithDelay(closestEnemy)); // Spawn projectiles with delay

        StartCoroutine(TemporarilyHideWeaponHeader()); // Temporarily hide the weapon header
    }

    private IEnumerator SpawnProjectilesWithDelay(Transform closestEnemy)
    {
        foreach (Transform firePoint in firePoints)
        {
            SpawnProjectile(firePoint, closestEnemy);
            yield return new WaitForSeconds(0.2f); // Delay of 0.2 seconds between each projectile
        }
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
        if (isChoosingTarget)
        {
            // Confirm the target when R is pressed again
            ConfirmUltimateTarget();
            return;
        }

        // Start targeting mode
        StartUltimateTargeting();
    }

    private void StartUltimateTargeting()
    {
        Debug.Log("Starting ultimate targeting...");
        isChoosingTarget = true;

        // Instantiate the targeting circle
        activeTargetCircle = Instantiate(ultimateTargetCirclePrefab);
        Cursor.visible = true; // Make the cursor visible for targeting
    }

    private void ConfirmUltimateTarget()
    {
        Debug.Log("Target confirmed, launching ultimate...");
        isChoosingTarget = false;

        // Destroy the targeting circle
        if (activeTargetCircle != null)
        {
            Destroy(activeTargetCircle);
        }

        // Launch the ultimate attack to the selected position
        LaunchUltimateAtTarget(selectedTargetPosition);

        // Reset the cursor visibility
        Cursor.visible = false;
    }

    private void LaunchUltimateAtTarget(Vector3 targetPosition)
    {
        // Instantiate the big glove
        GameObject bigGlove = Instantiate(bigBoxingGlovePrefab, transform.position, Quaternion.identity);

        if (ultimateImpactVFXPrefab != null)
        {
            Instantiate(ultimateStartVFXPrefab, transform.position, Quaternion.identity);
        }

        // Pass the target position to the UltimateBullet script
        BoxingGlovesUltimateBullet ultimateBullet = bigGlove.GetComponent<BoxingGlovesUltimateBullet>();
        if (ultimateBullet != null)
        {
            ultimateBullet.Initialize(targetPosition);
        }
        else
        {
            Debug.LogError("UltimateBullet script is missing on the big glove prefab!");
        }
    }

    private void Update()
    {
        if (isChoosingTarget)
        {
            HandleTargetSelection();
        }
    }

    private void HandleTargetSelection()
    {
        // Use a raycast to detect the mouse position on the ground with the "Ground" tag
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            // Update the targeting circle's position
            selectedTargetPosition = hit.point;
            if (activeTargetCircle != null)
            {
                activeTargetCircle.transform.position = selectedTargetPosition;
            }
        }
    }

    private void SpawnProjectile(Transform firePoint, Transform target)
    {
        GameObject projectile = projectilePool.GetObject();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(target.position - firePoint.position);

        projectile.transform.localScale = Vector3.one;

        // Mirror the projectile if it is fired from the second fire point
        if (firePoint == firePoints[1]) // Assuming firePoints[1] is the second fire point
        {
            Vector3 scale = projectile.transform.localScale;
            scale.x = -scale.x; // Mirror the projectile by flipping the x-axis
            projectile.transform.localScale = scale;
            leftGunVFX.Play();
        }
        else
        {
            rightGunVFX.Play();
        }

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

    private IEnumerator TemporarilyHideWeaponHeader()
    {
        if (WeaponHeader != null)
        {
            WeaponHeader.SetActive(false); // Hide the weapon header
            yield return new WaitForSeconds(0.1f); // Wait for a brief moment (adjust as needed)
            WeaponHeader.SetActive(true); // Re-enable the weapon header
        }
        else
        {
            Debug.LogWarning("WeaponHeader is not assigned!");
        }
    }

    private void LateUpdate()
    {
        if (isChoosingTarget && activeTargetCircle != null)
        {
            // Ensure the targeting circle updates properly
            HandleTargetSelection();
        }
    }
}
