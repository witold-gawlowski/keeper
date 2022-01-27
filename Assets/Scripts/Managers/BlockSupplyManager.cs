using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSupplyManager : Singleton<BlockSupplyManager>
{
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
        availableBlocks[SelectedBlockToSpawn]--;
        if (availableBlocks[SelectedBlockToSpawn] == 0)
        {
            SelectedBlockToSpawn = GetNextAvailableBlockSO(SelectedBlockToSpawn);
        }
    }
    public void RecoverBlock(BlockSO type)
    {
        availableBlocks[type]++;
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
    }
}
