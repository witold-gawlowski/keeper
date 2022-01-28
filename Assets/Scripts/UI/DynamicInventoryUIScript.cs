using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryUIScript : Singleton<DynamicInventoryUIScript>
{
    [SerializeField] BlockInventoryItemPanelScript itemsScript;
    private Dictionary<BlockSO, InventoryItemUIScript> blockTypeToUIElemMap;
    private void Start()
    {
        itemsScript.Clear();
        CreateInitialUIBlockCounts();
    }
    private void OnEnable()
    {
        BlockSupplyManager.Instance.blockCountChangedEvent += SetCount;
    }
    private void CreateInitialUIBlockCounts()
    {
        blockTypeToUIElemMap = new Dictionary<BlockSO, InventoryItemUIScript>();
        var startingCounts = InventoryManager.Instance.BlockCounts;
        foreach (var countData in startingCounts)
        {
            var blockSO = countData.Key;
            var count = countData.Value;
            var inventoryUIItem = CreateBlockCountItem(blockSO, count);
            blockTypeToUIElemMap.Add(countData.Key, inventoryUIItem);
        }
    }
    public void SetCount(BlockSO block, int count)
    {
        var itemGO = blockTypeToUIElemMap[block].gameObject;
        var itemScript = itemGO.GetComponent<InventoryItemUIScript>();
        if (count == 0)
        {
            itemGO.SetActive(false);
        }
        else
        {
            itemGO.SetActive(true);
            itemScript.SetCount(count);
        }
    }
    private InventoryItemUIScript CreateBlockCountItem(BlockSO block, int initialCount)
    {
        var blockPrefab = block.PrefabBlockScript;
        var sprite = blockPrefab.GetSprite();
        return itemsScript.AddBlockItem(sprite, initialCount);
    }
}
