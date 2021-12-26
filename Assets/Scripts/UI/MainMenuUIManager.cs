using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    public System.Action onLevelSelectionButtonPressEvent;
    [SerializeField] private List<InventoryItemUIScript> inventoryIcons;
    private void Awake()
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
            AddItem(i);
        }
    }
    void AddItem(BlockSO block)
    {
        foreach (var icon in inventoryIcons)
        {
            if (!icon.gameObject.activeSelf)
            {
                icon.gameObject.SetActive(true);
                icon.SetCount(block.countInInventory);
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
