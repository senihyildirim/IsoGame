using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Boost Item", menuName = "Inventory/Speed Boost Item")]
public class SpeedBoostItem : InventoryItemBase
{
    [Header("Speed Boost Settings")]
    public float speedIncrease;       // Amount to increase the speed
    public bool isTemporary;          // Should the speed boost be temporary?
    public float duration;            // Duration of the speed boost if temporary (in seconds)

    public override void Use()
    {
        PlayerEvents.RequestSpeedBoost(speedIncrease, isTemporary, duration);
    }
}
