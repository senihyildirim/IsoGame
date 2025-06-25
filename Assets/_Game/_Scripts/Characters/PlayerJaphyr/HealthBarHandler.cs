using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private Slider healthBar;    // The main health bar that changes instantly
    [SerializeField] private Slider easeHealthBar; // The delayed easing health bar
    [SerializeField] private TextMeshProUGUI healthText; // Optional: Display the health value

    // Optional: Animation settings
    [SerializeField] private float easeDelay = 0.5f; // Delay before ease health bar starts
    [SerializeField] private float easeDuration = 1.0f; // Duration of the ease bar animation

    public void InitializeHealthBar(float maxHealth)
    {
        // Initialize both health bars to full health
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        easeHealthBar.maxValue = maxHealth;
        easeHealthBar.value = maxHealth;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthText.text = $"{currentHealth} / {maxHealth}";

        healthBar.value = currentHealth;
        easeHealthBar.DOValue(currentHealth, easeDuration).SetDelay(easeDelay);
    }
}
