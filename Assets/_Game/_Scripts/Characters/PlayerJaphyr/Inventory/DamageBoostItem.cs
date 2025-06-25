using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Damage Boost Item", menuName = "Inventory/Damage Boost Item")]
public class DamageBoostItem : InventoryItemBase
{
    [SerializeField] private FloatVariable japhyrAttackDamage;

    [Header("Damage Boost Settings")]
    public float damageIncrease;   // Amount to increase the damage
    public bool isTemporary;       // Is the damage boost temporary?
    public float duration;         // Duration of the damage boost (if temporary)

    public override void Use()
    {
        PlayerEvents.RequestDamageBoost(damageIncrease, isTemporary, duration);
    }
}
