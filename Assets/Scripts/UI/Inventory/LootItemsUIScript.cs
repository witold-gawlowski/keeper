using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemsUIScript : InventoryItemPanelScript
{
    [SerializeField] private Sprite diggerIcon;
    [SerializeField] private Color diggerIconColor;
    [SerializeField] private Sprite componentOrbIcon;
    [SerializeField] private Color componentOrbColor;
    public void AddDiggerItem(int count)
    {
        AddItem(diggerIcon, count, diggerIconColor);
    }
    public void AddComponentOrbCountItem(int count)
    {
        AddItem(componentOrbIcon, count, componentOrbColor);
    }
  
}
