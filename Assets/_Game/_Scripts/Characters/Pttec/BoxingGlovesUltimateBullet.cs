using System.Collections;
using UnityEngine;

public class BoxingGlovesUltimateBullet : MonoBehaviour
{
    public float speed = 5f; // Speed of the horizontal motion
    public float arcHeight = 5f; // Maximum height of the arc
    public GameObject startExplosionVFXPrefab; // Explosion effect prefab
    public GameObject explosionVFXPrefab; // Explosion effect prefab
    public float explosionVFXLifetime = 3f; // Lifetime of explosion VFX
    public float explosionRadius = 5f; // Radius of the area damage
    public int damageAmount = 100; // Amount of damage dealt to each enemy

    private Vector3 targetPosition; // The target position

    public void Initialize(Vector3 targetPosition)
    {
        //GameObject startExplosion = Instantiate(startExplosionVFXPrefab, transform.position, Quaternion.identity);
        //Destroy(startExplosion, explosionVFXLifetime);

        //this.targetPosition = targetPosition;

        // Start the parabolic motion coroutine
        StartCoroutine(ParabolicMotion(targetPosition));
    }

    private IEnumerator ParabolicMotion(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;

        // Calculate the duration of the motion based on the horizontal distance and speed
        float horizontalDistance = Vector3.Distance(new Vector3(startPosition.x, 0, startPosition.z), new Vector3(targetPosition.x, 0, targetPosition.z));
        float duration = horizontalDistance / speed;

        float elapsedTime = 0f;

        // Previous position for calculating direction
        Vector3 previousPosition = startPosition;

        while (elapsedTime < duration)
        {
            // Calculate the linear interpolation (horizontal motion)
            float t = elapsedTime / duration;
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Add the parabolic height offset
            float height = Mathf.Sin(Mathf.PI * t) * arcHeight; // Parabolic arc
            currentPosition.y += height;

            // Update the glove's position
            transform.position = currentPosition;

            // Calculate the direction based on the movement
            Vector3 direction = (currentPosition - previousPosition).normalized;
            if (direction != Vector3.zero) // Prevent errors when direction is zero
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // Update the previous position
            previousPosition = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the glove lands exactly at the target position
        transform.position = targetPosition;

        // Trigger the explosion and apply area damage
        TriggerExplosion(targetPosition);

        // Destroy the glove after impact
        Destroy(gameObject);
    }

    private void TriggerExplosion(Vector3 explosionCenter)
    {
        // Instantiate the explosion VFX
        if (explosionVFXPrefab != null)
        {
            GameObject explosion = Instantiate(explosionVFXPrefab, explosionCenter, Quaternion.identity);
            Destroy(explosion, explosionVFXLifetime);
        }

        // Exclude the targeting circle from explosion detection using a specific tag or layer
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, explosionRadius, ~0, QueryTriggerInteraction.Collide);

        Debug.Log($"Found {colliders.Length} colliders in explosion range.");

        foreach (Collider collider in colliders)
        {
            // Skip the targeting circle if it has a specific tag
            if (collider.CompareTag("TargetCircle"))
            {
                continue;
            }

            // Check for MobHealth component directly
            if (collider.TryGetComponent<MobHealth>(out var enemyHealth))
            {
                Debug.Log($"Enemy with MobHealth detected: {collider.name}");

                // Apply damage to the enemy
                enemyHealth.TakeDamage(damageAmount);
            }
            else
            {
                Debug.Log($"No MobHealth on: {collider.name}");
            }
        }

        Debug.Log("UltimateBullet explosion applied area damage!");
    }

    private void OnDrawGizmos()
    {
        // Set the Gizmos color for the explosion radius
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f); // Orange with some transparency
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
