using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemsUIScript : BlockInventoryItemPanelScript
{
    [SerializeField] private Sprite diggerIcon;
    [SerializeField] private Color diggerIconColor;

    public void AddDiggerItem(int count)
    {
        AddItem(diggerIcon, count, diggerIconColor);
    }
  
}
