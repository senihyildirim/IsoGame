using UnityEngine;

public class BulletPoison : MonoBehaviour
{
    private float damage;
    private bool isInitialized = false;

    public void Initialize(float damage)
    {
        this.damage = damage;
        isInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isInitialized) return;

        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}