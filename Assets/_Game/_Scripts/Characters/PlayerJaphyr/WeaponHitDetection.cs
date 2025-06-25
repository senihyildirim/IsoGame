using UnityEngine;
using DG.Tweening;

public class WeaponHitDetection : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatVariable japhyrBasicAttackDamage;

    [Header("Hit Detection Settings")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private CameraShake cameraShake;

    [Header("Hit Reaction Settings")]
    public float shakeDuration = 0.1f; // Short duration for the shake
    public float shakeStrength = 2.0f; // Stronger vibration for the shake
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
    public float hitBackDistance = 0.5f;
    public float hitBackDuration = 0.2f;

    private Vector3 originalPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MobHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(japhyrBasicAttackDamage.CurrentValue);

            Vector3 hitPoint = other.ClosestPoint(transform.position);

            ShowHitEffect(hitPoint);
            // PlayHitSound();
            // PlayHitReaction(other.transform);

            // Adjust the shake settings for the hit reaction
            cameraShake.ShakeCamera(0.2f, 0.2f, 50.0f);
        }

        if (other.TryGetComponent<SpiderBossHealth>(out var spiderBossHealth))
        {
            spiderBossHealth.TakeDamage(japhyrBasicAttackDamage.CurrentValue);

            Vector3 hitPoint = other.ClosestPoint(transform.position);

            ShowHitEffect(hitPoint);
            PlayHitSound();
            // PlayHitReaction(other.transform);

            cameraShake.ShakeCamera(0.2f, 0.2f, 50.0f);
        }
    }

    private void ShowHitEffect(Vector3 hitPoint)
    {
        GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);

        ParticleSystem particleSystem = hitEffect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
            Destroy(hitEffect, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        else
            Destroy(hitEffect, 2f);
    }

    private void PlayHitSound()
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    private void PlayHitReaction(Transform enemyTransform)
    {
        originalPosition = enemyTransform.position;

        enemyTransform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
            .OnComplete(() => enemyTransform.position = originalPosition);
    }
}
