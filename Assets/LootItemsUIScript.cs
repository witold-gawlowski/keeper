using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemsUIScript : MonoBehaviour
{
    [SerializeField] private Sprite diggerIcon;
    [SerializeField] private Color blockIconColor;
    [SerializeField] private Color diggerIconColor;
    [SerializeField] Transform rewardImageParent;
    public void Clear()
    {
        foreach (Transform icon in rewardImageParent)
        {
            if (icon != rewardImageParent)
            {
                icon.gameObject.SetActive(false);
            }
        }
    }
    public void AddBlockItem(Sprite s, int count)
    {
        AddItem(s, count, blockIconColor);
    }
    public void AddDiggerItem(int count)
    {
        AddItem(diggerIcon, count, diggerIconColor);
    }
    private void AddItem(Sprite s, int count, Color c)
    {
        foreach (Transform icon in rewardImageParent)
        {
            if (icon != rewardImageParent && icon.gameObject.activeSelf == false)
            {
                icon.gameObject.SetActive(true);
                var rewardIconScript = icon.GetComponent<InventoryItemUIScript>();
                rewardIconScript.SetSprite(s);
                rewardIconScript.SetCount(count);
                rewardIconScript.SetColor(c);
                return;
            }
        }
    }
}
