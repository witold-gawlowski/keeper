using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem", order = 1)]
public class InventoryItemSO : ScriptableObject
{
    public GameObject prefab;
    public int count;
}
