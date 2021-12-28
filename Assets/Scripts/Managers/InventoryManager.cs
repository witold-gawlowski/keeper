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
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelCompleted;
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += HandleLevelFailed;
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleSurrender;
    }
    void HandleLevelCompleted()
    {
        RemoveUsedBlocks();
        AddReward();
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
        Debug.Log("resetting counts");
        blockCounts = new Dictionary<BlockSO, int>();
        foreach (var blockSO in blockSOs)
        {
            blockCounts.Add(blockSO, blockSO.initialCountInInventory);
        }
    }
}
