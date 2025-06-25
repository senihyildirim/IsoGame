using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.VFX;

[RequireComponent(typeof(CharacterController))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDistance = 10f; // Distance the player will dash
    [SerializeField] private float cooldownTime = 2f; // Cooldown duration in seconds
    [SerializeField] private VisualEffect dashVFX;

    private CharacterController characterController;
    private AnimatorBrain animatorBrain;

    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity; // Initialize to a large negative value to ensure the first dash can happen

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animatorBrain = GetComponent<AnimatorBrain>();
    }

    // This method is called by the PlayerInput in editor
    public void OnDash(InputAction.CallbackContext context)
    {
        // Check if the cooldown period has expired and the player is not currently dashing
        if (context.performed && !isDashing && !animatorBrain.IsLocked(animatorBrain.UPPER_BODY_LAYER) && Time.time >= lastDashTime + cooldownTime)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time; // Set the time when the dash starts

        float distanceTraveled = 0f;

        animatorBrain.Play(PlayerAnimations.DASH, animatorBrain.UPPER_BODY_LAYER, true, false);

        dashVFX.gameObject.SetActive(true);
        dashVFX.Play();
        StartCoroutine(DisableDashVFX());

        while (distanceTraveled < dashDistance)
        {
            Vector3 dashDirection = transform.forward; // Assuming the dash is in the forward direction
            Vector3 move = dashDirection * dashSpeed * Time.deltaTime;

            characterController.Move(move);
            distanceTraveled += move.magnitude;

            yield return null; // Wait for the next frame
        }

        animatorBrain.Play(PlayerAnimations.IDLE, animatorBrain.UPPER_BODY_LAYER, false, true, 0.5f);

        isDashing = false;
    }

    private IEnumerator DisableDashVFX()
    {
        yield return new WaitForSeconds(0.5f);
        dashVFX.gameObject.SetActive(false);
    }
}
