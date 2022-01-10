using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : GlobalManager<InventoryManager>
{
    [SerializeField] private List<BlockSO> blockSOs;
    Dictionary<BlockSO, int> blockCounts;
    protected override void Awake()
    {
        base.Awake();
        ResetCounts();
    }
    public Dictionary<BlockSO, int> GetInventory()
    {
        return blockCounts;
    }
    public void HandleLevelCompleted()
    {
        RemoveUsedBlocks();
        AddReward();
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
            if (blockCounts.ContainsKey(rewardItem.block))
            {
                blockCounts[rewardItem.block] += rewardItem.count;
            }
            else
            {
                Debug.Log(rewardItem.block.name);
                Debug.Log(blockCounts.Keys);
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
    }
}
