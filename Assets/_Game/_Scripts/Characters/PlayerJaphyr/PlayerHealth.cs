using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatVariable japhyrHealth;

    [Header("UI")]
    // TODO: Change the health bar handler
    [SerializeField] private HealthBarHandler healthBar;

    public static event Action<float, float> OnHealthChanged;

    private Coroutine regenCoroutine; // To handle health regeneration over time

    private void OnEnable()
    {
        PlayerEvents.OnHealRequested += Heal;
        PlayerEvents.OnHealthRegenRequested += HandleHealthRegen;
        PlayerEvents.OnMaxHealthIncreaseRequested += IncreaseBaseHealth;
    }

    private void OnDisable()
    {
        PlayerEvents.OnHealRequested -= Heal;
        PlayerEvents.OnHealthRegenRequested -= HandleHealthRegen;
        PlayerEvents.OnMaxHealthIncreaseRequested -= IncreaseBaseHealth;
    }

    private void Start()
    {
        // Initialize health bar
        healthBar.InitializeHealthBar(japhyrHealth.DefaultValue);

        // Trigger the initial UI update
        OnHealthChanged += healthBar.UpdateHealthBar;
        OnHealthChanged?.Invoke(japhyrHealth.CurrentValue, japhyrHealth.DefaultValue);
    }

    public void TakeDamage(float damage)
    {
        japhyrHealth.CurrentValue -= damage;
        japhyrHealth.CurrentValue = Mathf.Clamp(japhyrHealth.CurrentValue, 0, japhyrHealth.DefaultValue);
        OnHealthChanged?.Invoke(japhyrHealth.CurrentValue, japhyrHealth.DefaultValue);

        if (japhyrHealth.CurrentValue <= 0)
        {
            Die();
        }
    }

    // Method to apply instant healing
    public void Heal(float amount)
    {
        japhyrHealth.CurrentValue += amount;
        japhyrHealth.CurrentValue = Mathf.Clamp(japhyrHealth.CurrentValue, 0, japhyrHealth.DefaultValue);
        OnHealthChanged?.Invoke(japhyrHealth.CurrentValue, japhyrHealth.DefaultValue);
    }

    // Handler for health regeneration requests
    private void HandleHealthRegen(float amountPerTick, float interval, float duration)
    {
        StartHealthRegen(amountPerTick, interval, duration);
    }

    // Method to start health regeneration over time
    private void StartHealthRegen(float amountPerTick, float tickInterval, float duration)
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine); // Stop any ongoing regeneration coroutine
        }
        regenCoroutine = StartCoroutine(HealthRegenCoroutine(amountPerTick, tickInterval, duration));
    }

    // Coroutine to regenerate health over time
    private IEnumerator HealthRegenCoroutine(float amountPerTick, float tickInterval, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Heal(amountPerTick);  // Apply health regeneration
            yield return new WaitForSeconds(tickInterval);  // Wait for the next tick
            elapsedTime += tickInterval;
        }

        regenCoroutine = null;  // Clear coroutine reference when done
    }

    // Method to increase base (maximum) health
    public void IncreaseBaseHealth(float amount)
    {
        japhyrHealth.DefaultValue += amount;  // Increase the player's maximum health
        japhyrHealth.CurrentValue = Mathf.Clamp(japhyrHealth.CurrentValue, 0, japhyrHealth.DefaultValue);  // Optionally adjust current health
        healthBar.InitializeHealthBar(japhyrHealth.DefaultValue);  // Update the health bar to reflect new max health
        OnHealthChanged?.Invoke(japhyrHealth.CurrentValue, japhyrHealth.DefaultValue);  // Trigger UI update
    }

    private void Die()
    {
        Debug.Log("Player died.");
        PlayerEvents.RaisePlayerDeath();
    }
}
