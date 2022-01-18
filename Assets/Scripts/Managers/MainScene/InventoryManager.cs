using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : GlobalManager<InventoryManager>
{
    [SerializeField] private List<BlockSO> blockSOs;
    Dictionary<BlockSO, int> blockCounts;
    public int DiggerCount { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        ResetCounts();
    }
    public Dictionary<BlockSO, int> GetBlocks()
    {
        return blockCounts;
    }
    public void OnLevelCompleted()
    {
        RemoveUsedBlocks();
        AddReward();
    }
    public void RemoveDiggers(int diggersUsed)
    {
        DiggerCount -= diggersUsed;
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += HandleLevelFailed;
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleSurrender;
    }
    void HandleLevelFailed() => ResetCounts();
    void HandleSurrender() => ResetCounts();
    void RemoveUsedBlocks()
    {
        var usedBlocks = BlockManager.Instance.GetUsedBlocks();
        foreach(var usedItem in usedBlocks)
        {
            blockCounts[usedItem.Key] -= usedItem.Value;
        }
    }
    void AddReward()
    {
        var reward = GameStateManager.Instance.SelectedMapData.reward;
        foreach(var rewardItem in reward)
        {
            if (rewardItem is BlockRewardItem)
            {
                var blockRewardItem = rewardItem as BlockRewardItem;
                if (blockCounts.ContainsKey(blockRewardItem.block))
                {
                    blockCounts[blockRewardItem.block] += blockRewardItem.count;
                }
                else
                {
                    Debug.Log(blockRewardItem.block.name);
                    Debug.Log(blockCounts.Keys);
                }
            }
            else if(rewardItem is DiggerRewardItem)
            {
                var diggerRewardItem = rewardItem as DiggerRewardItem;
                DiggerCount += diggerRewardItem.count;
            }
        }
    }
    void ResetCounts()
    {
        blockCounts = new Dictionary<BlockSO, int>();
        foreach (var blockSO in blockSOs)
        {
            if (blockSO != null)
            {
                blockCounts.Add(blockSO, blockSO.initialCountInInventory);
            }
        }
        DiggerCount = 0;
    }
}
