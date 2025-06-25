using System.Collections.Generic;
using UnityEngine;

public class ShotgunUltiBullet : MonoBehaviour
{
    public float speed = 10f;                // Speed of the projectile
    public float range = 20f;               // Distance before explosion
    public float explosionDuration = 2f;    // Duration of the explosion
    public LayerMask targetLayer;           // Layer for collision detection

    private Vector3 startPosition;          // Starting position to calculate range
    private bool hasExploded = false;       // Track whether the explosion has occurred
    public int damageAmount = 100; // Amount of damage dealt to each enemy

    // References to visual/functional components
    public GameObject explosionEffect;      // Visual effect prefab for explosion
    public GameObject projectileVisual;     // Visual representation of the projectile
    public List<GameObject> explosionColliders; // List of explosion colliders

    void Start()
    {
        startPosition = transform.position; // Store starting position
    }

    void Update()
    {
        if (!hasExploded)
        {
            // Move the projectile forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // Check if it has reached its max range
            if (Vector3.Distance(startPosition, transform.position) >= range)
            {
                TriggerExplosion();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with: {other.name}");
        // Detect collisions during the first phase (projectile phase)
        //if (!hasExploded && other.CompareTag("Enemy"))
        if (!hasExploded && other.TryGetComponent<MobHealth>(out var enemyHealth))
        {
            Debug.Log("Projectile hit an enemy!");
            enemyHealth.TakeDamage(damageAmount);
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        hasExploded = true;
        Debug.Log("hasExploded = " + hasExploded);

        // Disable projectile visual and movement
        projectileVisual.SetActive(false);
        speed = 0f;

        // Enable explosion effects and colliders
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, transform.rotation);

        foreach (GameObject colliderObject in explosionColliders)
        {
            colliderObject.SetActive(true);
        }
        // Destroy after explosion duration
        Destroy(gameObject, explosionDuration);
    }
}
