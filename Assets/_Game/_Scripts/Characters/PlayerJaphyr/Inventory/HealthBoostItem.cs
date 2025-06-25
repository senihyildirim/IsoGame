using UnityEngine;

[CreateAssetMenu(fileName = "New Health Boost Item", menuName = "Inventory/Health Boost Item")]
public class HealthBoostItem : InventoryItemBase
{
    [Header("Instant Health Boost")]
    public bool applyInstantHeal;      // Should instant heal be applied?
    public float instantHealAmount;      // Amount of health to instantly restore

    [Header("Health Regeneration Over Time")]
    public bool applyRegenOverTime;    // Should health regenerate over time?
    public float regenAmountPerTick;     // How much health to restore each tick
    public float regenTickInterval;    // Interval between regeneration ticks (seconds)
    public float regenDuration;        // Total duration of health regeneration (seconds)

    [Header("Base Health Increase")]
    public bool applyBaseHealthIncrease; // Should base max health be increased?
    public float baseHealthIncreaseAmount; // Amount to increase base max health

    public override void Use()
    {
        // Apply instant health boost if applicable
        if (applyInstantHeal)
        {
            PlayerEvents.RequestHeal(instantHealAmount);
        }

        // Apply health regeneration over time if applicable
        if (applyRegenOverTime)
        {
            PlayerEvents.RequestHealthRegen(regenAmountPerTick, regenTickInterval, regenDuration);
        }

        // Apply base health increase if applicable
        if (applyBaseHealthIncrease)
        {
            PlayerEvents.RequestMaxHealthIncrease(baseHealthIncreaseAmount);
        }
    }
}
