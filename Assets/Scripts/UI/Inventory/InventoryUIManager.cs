using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : Singleton<InventoryUIManager>
{
    public System.Action continuePressedEvent;
    public System.Action backPressedEvent;

    [SerializeField] private LootItemsUIScript inventoryItemsUIScript;
    private void Awake()
    {
        UpdateInventory();
    }
    public void OnBackPressed()
    {
        backPressedEvent?.Invoke();
    }
    public void OnContinuePressed()
    {
        continuePressedEvent?.Invoke();
    }
    private void UpdateInventory()
    {
        inventoryItemsUIScript.Clear();
        var inventory = InventoryManager.Instance;
        foreach (var i in inventory.BlockCounts)
        {
            if (i.Value > 0)
            {
                var blockPrefab = i.Key.PrefabBlockScript;
                var sprite = blockPrefab.GetSprite();
                inventoryItemsUIScript.AddBlockItem(sprite, i.Value);
            }
        }
        var diggerCount = inventory.DiggerCount;
        var componentOrbCount = inventory.ComponentOrbCount;
        inventoryItemsUIScript.AddDiggerItem(diggerCount);
        inventoryItemsUIScript.AddComponentOrbCountItem(componentOrbCount);
    }
}

