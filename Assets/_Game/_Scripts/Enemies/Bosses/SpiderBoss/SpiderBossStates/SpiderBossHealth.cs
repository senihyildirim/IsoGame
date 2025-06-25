using UnityEngine;

public enum SpiderBossHealthState
{
    High,   // Above 66%
    Medium, // Between 33% and 66%
    Low     // Below 33%
}

public class SpiderBossHealth : MonoBehaviour
{
    [SerializeField] private float initialHealth = 300f; // Adjust initial health in the inspector
    [SerializeField] private float maxHealth = 300f;     // Adjust max health in the inspector

    public bool isShieldActive = false; // Check if the shield is active
    public SpiderBossHealthState currentHealthState;

    private SpiderBossStateController spiderBossStateController;
    private float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            initialHealth = currentHealth;
            UpdateHealthState();

            if (currentHealth <= 0)
            {
                spiderBossStateController.HandleSpiderBossDeath();
            }
        }
    }

    private void Start()
    {
        spiderBossStateController = GetComponent<SpiderBossStateController>();
        currentHealth = initialHealth; // Initialize health with the inspector-defined value
        UpdateHealthState(); // Set initial health state
    }

    public void TakeDamage(float damage)
    {
        if (isShieldActive)
        {
            Debug.Log("Shield is active! SpiderBoss takes no damage.");
            return;
        }

        CurrentHealth -= damage; // Use property to trigger update
    }

    public void RegenerateHealth(float amount)
    {
        CurrentHealth += amount; // Use property to trigger update
    }

    private void UpdateHealthState()
    {
        float healthPercentage = currentHealth / maxHealth;

        if (healthPercentage > 0.66f)
        {
            currentHealthState = SpiderBossHealthState.High;
        }
        else if (healthPercentage > 0.33f)
        {
            currentHealthState = SpiderBossHealthState.Medium;
        }
        else
        {
            currentHealthState = SpiderBossHealthState.Low;
        }

        // Debugging output to check health state
        Debug.Log("SpiderBoss health state: " + currentHealthState);
    }

    public bool IsHealthFull()
    {
        return currentHealth >= maxHealth;
    }
}
