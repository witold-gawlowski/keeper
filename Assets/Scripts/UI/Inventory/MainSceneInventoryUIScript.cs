using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInventoryUIScript : Singleton<MainSceneInventoryUIScript>
{
    [SerializeField] InventoryItemPanelScript itemsScript;
    private Dictionary<BlockSO, MainSceneInventoryButtonScript> blockTypeToUIItemMap;
    public void Init()
    {
        itemsScript.Clear();
        CreateInitialUIBlockCounts();
    }
    private void OnEnable()
    {
        BlockSupplyManager.Instance.blockCountChangedEvent += SetCount;
    }
    public List<MainSceneInventoryButtonScript> GetIneventoryButtons()
    {
        return new List<MainSceneInventoryButtonScript>(blockTypeToUIItemMap.Values);
    }
    private void CreateInitialUIBlockCounts()
    {
        blockTypeToUIItemMap = new Dictionary<BlockSO, MainSceneInventoryButtonScript>();
        var startingCounts = InventoryManager.Instance.BlockCounts;
        foreach (var countData in startingCounts)
        {
            var blockSO = countData.Key;
            var count = countData.Value;
            var inventoryUIItem = CreateBlockCountItem(blockSO, count);
            inventoryUIItem.Init(blockSO);
            blockTypeToUIItemMap.Add(countData.Key, inventoryUIItem);
        }
    }
    public void SetCount(BlockSO block, int count)
    {
        var itemGO = blockTypeToUIItemMap[block].gameObject;
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
    private MainSceneInventoryButtonScript CreateBlockCountItem(BlockSO block, int initialCount)
    {
        var blockPrefab = block.PrefabBlockScript;
        var sprite = blockPrefab.GetSprite();
        var blockUIItem = itemsScript.AddBlockItem(sprite, initialCount);
        return blockUIItem as MainSceneInventoryButtonScript;
    }
}
