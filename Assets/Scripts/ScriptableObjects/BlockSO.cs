using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Block", menuName = "ScriptableObjects/Block")]
public class BlockSO : ScriptableObject
{
    public GameObject prefab;
    public int initialCountInInventory = 0;
    public float regularity;
    public int minRewardItemMultiplicity = 1;
    public int maxRewardItemMultiplicity = 3;
    public BlockScript PrefabBlockScript
    {
        get
        {
            if(_blockScript == null)
            {
                _blockScript = prefab.GetComponent<BlockScript>();
            }
            return _blockScript;
        }
        private set { _blockScript = value; }
    }
    private BlockScript _blockScript;
}
