using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatVariable pttecAttackDamage;

    public float speed = 10f; 
    public float lifetime = 3f; 
    private float timer;
    private ObjectPool objectPool; 

    public GameObject hitVFXPrefab; 

   
    public void Initialize(ObjectPool pool)
    {
        if (pool == null)
        {
            Debug.LogError("ObjectPool is null! Cannot initialize LaserProjectile.");
            return;
        }

        objectPool = pool;
        timer = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MobHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(pttecAttackDamage.CurrentValue);

            Vector3 hitPosition = other.ClosestPoint(transform.position);
            Vector3 hitNormal = (hitPosition - transform.position).normalized;

            ActivateHitVFX(hitPosition, hitNormal); 

            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (objectPool != null)
        {
            objectPool.ReturnObject(gameObject); 
        }
        else
        {
            Debug.LogError("ObjectPool is null! Destroying projectile.");
            Destroy(gameObject); 
        }
    }

    private void ActivateHitVFX(Vector3 position, Vector3 normal)
    {
        if (hitVFXPrefab != null)
        {
            Quaternion hitRotation = Quaternion.LookRotation(normal);

            GameObject vfx = Instantiate(hitVFXPrefab, position, hitRotation);

            Destroy(vfx, 2f);
        }
        else
        {
            Debug.LogWarning("Hit VFX prefab is not assigned!");
        }
    }
}
