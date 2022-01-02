using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    public System.Action onLevelSelectionButtonPressEvent;
    [SerializeField] private List<InventoryItemUIScript> inventoryIcons;
    private void Start()
    {
        UpdateInventory();
    }
    public void OnStartPress()
    {
        onLevelSelectionButtonPressEvent();
    }
    void UpdateInventory()
    {
        ClearInventory();
        var inventory = InventoryManager.Instance.GetInventory();
        foreach (var i in inventory)
        {
            AddItem(i.Key, i.Value);
        }
    }
    void AddItem(BlockSO block, int count)
    {
        foreach (var icon in inventoryIcons)
        {
            if (!icon.gameObject.activeSelf)
            {
                icon.gameObject.SetActive(true);
                icon.SetCount(count);
                icon.SetSprite(block.BlockScript.GetSprite());
                return;
            }
        }
    }
    void ClearInventory()
    {
        foreach (var icon in inventoryIcons)
        {
            icon.gameObject.SetActive(false);
        }
    }
}
