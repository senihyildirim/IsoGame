using UnityEngine;

public abstract class InventoryItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    // Define an abstract method to use the item
    public abstract void Use();
}
