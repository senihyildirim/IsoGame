using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryUIHandler : MonoBehaviour
{
    public GameObject inventoryPanel;        // The UI panel for the inventory (to toggle visibility)
    public GameObject inventoryCellPrefab;   // Prefab for an inventory cell

    private bool isInventoryVisible = false; // Track inventory visibility state
    private List<InventoryItemBase> inventoryItems = new List<InventoryItemBase>(); // Inventory items list

    // Template items for testing
    [SerializeField] private List<InventoryItemBase> items;

    private void Start()
    {
        // Add some template items for testing
        foreach (var item in items)
        {
            inventoryItems.Add(item);
        }

        // Initialize the inventory UI
        InitializeInventoryUI(inventoryItems);
    }

    private void Update()
    {
        // Toggle inventory visibility with the "I" key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var item in items)
            {
                inventoryItems.Add(item);
            }

            InitializeInventoryUI(inventoryItems);
        }
    }

    // Initialize inventory UI with items
    public void InitializeInventoryUI(List<InventoryItemBase> inventoryItems)
    {
        ClearInventoryUI(); // Clear old cells

        // Create new cells based on the inventory items
        foreach (var item in inventoryItems)
        {
            GameObject cell = Instantiate(inventoryCellPrefab, inventoryPanel.transform);

            // Update the item display
            cell.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

            // Optionally, you could assign an icon to an Image component
            cell.GetComponent<Image>().sprite = item.itemIcon;

            // Add click functionality to the button
            Button button = cell.GetComponent<Button>();
            button.onClick.AddListener(() => UseItem(item));
        }
    }

    // Toggle inventory panel visibility
    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventoryPanel.SetActive(isInventoryVisible);

        if (isInventoryVisible)
            PlayerEvents.RaiseDisablePlayerInputs();
        else
            PlayerEvents.RaiseEnablePlayerInputs();
    }

    // Clear the existing inventory cells from the grid
    private void ClearInventoryUI()
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // This method will be used to apply the item and delete it after use
    public void UseItem(InventoryItemBase item)
    {
        Debug.Log("Using item: " + item.itemName);
        item.Use();  // Apply item effects

        // Remove the item from the inventory after using it
        inventoryItems.Remove(item);

        // Refresh the inventory UI
        InitializeInventoryUI(inventoryItems); // Rebuild the UI after removing the item
    }

    // Example method to simulate adding an item (could be called from another script)
    public void AddItemToInventory(InventoryItemBase newItem)
    {
        inventoryItems.Add(newItem);
        InitializeInventoryUI(inventoryItems); // Update UI when new item is added
    }
}
