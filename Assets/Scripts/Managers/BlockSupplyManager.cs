using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSupplyManager : Singleton<BlockSupplyManager>
{
    public System.Action<BlockSO, int> blockCountChangedEvent;
    private Dictionary<BlockSO, int> availableBlocks;
    private List<BlockSO> blockOrder;
    public BlockSO SelectedBlockToSpawn { get; private set; }
    public void Init()
    {
        InitAvailableBlocks();
        CreateBlockOrder();
        SetInitialSelectedBlock();
    }
    public void ConsumeSelected()
    {
        var oldCount = availableBlocks[SelectedBlockToSpawn];
        var newCount = oldCount - 1;
        availableBlocks[SelectedBlockToSpawn] = newCount;
        blockCountChangedEvent(SelectedBlockToSpawn, newCount);
        if (newCount == 0)
        {
            SelectedBlockToSpawn = GetNextAvailableBlockSO(SelectedBlockToSpawn);
        }
    }
    public void RecoverBlock(BlockSO type)
    {
        availableBlocks[type]++;
        blockCountChangedEvent(type, availableBlocks[type]);
    }
    void SetInitialSelectedBlock()
    {
        SelectedBlockToSpawn = blockOrder[0];
    }
    BlockSO GetNextAvailableBlockSO(BlockSO current)
    {
        //if is last gets first available to the left
        bool passed = false;
        BlockSO lastAvaliable = null;
        foreach (var bSO in blockOrder)
        {
            if (bSO == current)
            {
                passed = true;
                continue;
            }
            if (availableBlocks[bSO] > 0)
            {
                lastAvaliable = bSO;
                if (passed)
                {
                    return bSO;
                }
            }
        }
        return lastAvaliable;
    }
    void InitAvailableBlocks()
    {
        availableBlocks = new Dictionary<BlockSO, int>(InventoryManager.Instance.BlockCounts);
    }
    void CreateBlockOrder()
    {
        blockOrder = new List<BlockSO>(availableBlocks.Keys);
        //LogOrder();
    }
    void LogOrder()
    {
        Debug.Log("order:");
        foreach (var b in blockOrder)
        {
            Debug.Log(b.name);
        }
    }
}
