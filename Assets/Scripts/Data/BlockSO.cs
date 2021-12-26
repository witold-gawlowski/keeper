using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Block", menuName = "ScriptableObjects/Block")]
public class BlockSO : ScriptableObject
{
    public GameObject prefab;
    public int countInInventory = 0;
    public BlockScript BlockScript
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
    [SerializeField] private BlockScript _blockScript;
}
