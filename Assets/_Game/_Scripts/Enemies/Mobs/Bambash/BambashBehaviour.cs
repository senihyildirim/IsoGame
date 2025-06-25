using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class BambashBehaviour : MobBehaviour
{
    [Header("Bambash Settings")]
    public BambashAnimationData bambashAnimationData;

    [Header("Dash Attack Settings")]
    [SerializeField] private float dashDamage = 20f;
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashCooldown = 3f;
    [SerializeField] VisualEffect dashVFX;

    private bool isDashing = false;
    private Sequence dashSequence;

    protected override void Start()
    {
        base.Start();
        dashVFX.SetFloat("Lifetime", dashDuration);
        TransitionToState(new BambashPatrollingState());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDashing) return;

        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            // Stop dashing immediately
            isDashing = false;

            // Kill the dash sequence
            if (dashSequence != null)
            {
                dashSequence.Kill(false);  // false means don't complete the tween
                dashSequence = null;
            }

            dashVFX.Stop();
            playerHealth.TakeDamage(dashDamage);

            PlayAnimationAndExecuteAction(animationName: bambashAnimationData.BambashHit, onAnimationComplete: () =>
            {
                PlayAnimationAndExecuteAction(animationName: bambashAnimationData.BambashDizzy, onAnimationComplete: () =>
                {
                    TransitionToState(new BambashPatrollingState());
                });
            });
        }
    }

    public void StartDash()
    {
        isDashing = true;
        dashVFX.Play();
        RotateTowardsPlayer();

        // Calculate dash target position
        Vector3 dashDirection = (PlayerEvents.RaiseGetPlayerPosition() - transform.position).normalized;
        dashDirection.y = 0f;

        // Kill any existing dash sequence
        if (dashSequence != null)
        {
            dashSequence.Kill(false);
        }

        // Perform the dash movement
        dashSequence = MovementTweener.PushInDirection(
            target: transform,
            direction: dashDirection,
            distance: dashSpeed,
            duration: dashDuration,
            easeType: Ease.InQuad,
            onComplete: () =>
            {
                isDashing = false;
                dashSequence = null;
                StartCoroutine(OnDashComplete());
            }
        );
    }

    private IEnumerator OnDashComplete()
    {
        PlayAnimation(bambashAnimationData.BambashIdle);
        yield return new WaitForSeconds(dashCooldown);
        TransitionToState(new BambashPatrollingState());
    }

    private void OnDestroy()
    {
        // Clean up any running sequence when destroyed
        if (dashSequence != null)
        {
            dashSequence.Kill(false);
            dashSequence = null;
        }
    }
}
