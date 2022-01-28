using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPanelScript : MonoBehaviour
{
    [SerializeField] protected Color blockIconColor;
    [SerializeField] protected Transform itemsParent;
    public void Clear()
    {
        foreach (Transform icon in itemsParent)
        {
            if (icon != itemsParent)
            {
                icon.gameObject.SetActive(false);
            }
        }
    }
    public InventoryItemUIScript AddBlockItem(Sprite s, int count)
    {
        return AddItem(s, count, blockIconColor);
    }
    protected InventoryItemUIScript AddItem(Sprite s, int count, Color c)
    {
        InventoryItemUIScript result = null;
        foreach (Transform icon in itemsParent)
        {
            if (icon != itemsParent && icon.gameObject.activeSelf == false)
            {
                icon.gameObject.SetActive(true);
                result = icon.GetComponent<InventoryItemUIScript>();
                result.SetSprite(s);
                result.SetCount(count);
                result.SetColor(c);
                break;
            }
        }
        return result;
    }
}
