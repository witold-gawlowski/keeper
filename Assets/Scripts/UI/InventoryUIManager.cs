using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItemUIScript> inventoryIcons;
    private void Start()
    {
        UpdateInventory();
    }
    private void UpdateInventory()
    {
        ClearInventory();
        var inventory = InventoryManager.Instance.GetInventory();
        foreach (var i in inventory)
        {
            if (i.Value > 0)
            {
                AddItem(i.Key, i.Value);
            }
        }
    }
    private void AddItem(BlockSO block, int count)
    {
        foreach (var icon in inventoryIcons)
        {
            if (!icon.gameObject.activeSelf)
            {
                icon.gameObject.SetActive(true);
                icon.SetCount(count);
                icon.SetSprite(block.PrefabBlockScript.GetSprite());
                return;
            }
        }
    }
    private void ClearInventory()
    {
        foreach (var icon in inventoryIcons)
        {
            icon.gameObject.SetActive(false);
        }
    }
}
