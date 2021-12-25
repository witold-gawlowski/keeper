using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Block", menuName = "ScriptableObjects/Block")]
public class BlockSO : ScriptableObject
{
    public GameObject prefab;
    public int countInInventory = 0;
}
