using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class InventoryManager : GlobalManager<InventoryManager>
{
    [SerializeField] private int startingDiggerCount = 15;
    [SerializeField] private int startingComponentOrbCount = 5;

    public Dictionary<BlockSO, int> BlockCounts { get; private set; }
    public int DiggerCount { get; private set; }
    public int ComponentOrbCount { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        ResetCounts();
    }
    public void OnLevelCompleted()
    {
        RemoveUsedComponents();
        RemoveUsedBlocks();
        AddReward();
    }
    public void RemoveDiggers(int diggersUsed)
    {
        DiggerCount -= diggersUsed;
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneManager.Instance.levelFailedEvent += HandleLevelFailed;
    }
    protected override void SubscribeToMenuSceneEvents()
    {
        MainMenuUIManager.Instance.startNewGameEvent += HandleStartNewGame;
    }
    void HandleLevelFailed()
    {
        RemoveUsedBlocks();
        RemoveUsedComponents();
    }
    void HandleStartNewGame() => ResetCounts();
    void RemoveUsedBlocks()
    {
        var usedBlocks = BlockManager.Instance.GetUsedBlocks();
        foreach(var usedItem in usedBlocks)
        {
            BlockCounts[usedItem.Key] -= usedItem.Value;
            Assert.IsTrue(BlockCounts[usedItem.Key] >= 0);
            if(BlockCounts[usedItem.Key] == 0)
            {
                BlockCounts.Remove(usedItem.Key);
            }
        }
    }
    void RemoveUsedComponents()
    {
        var usedCOmponents = CoherencyManager.Instance.ComponentCount;
        ComponentOrbCount -= usedCOmponents;
    }
    void AddReward()
    {
        var reward = GameStateManager.Instance.SelectedMapData.reward;
        foreach(var rewardItem in reward)
        {
            if (rewardItem is BlockRewardItem)
            {
                var blockRewardItem = rewardItem as BlockRewardItem;
                if (BlockCounts.ContainsKey(blockRewardItem.block))
                {
                    BlockCounts[blockRewardItem.block] += blockRewardItem.count;
                }
                else
                {
                    BlockCounts.Add(blockRewardItem.block, blockRewardItem.count);
                }
            }
            else if(rewardItem is DiggerRewardItem)
            {
                var diggerRewardItem = rewardItem as DiggerRewardItem;
                DiggerCount += diggerRewardItem.count;
            }
            else if (rewardItem is ComponentOrbRewardItem)
            {
                var componentOrbRewardItem = rewardItem as ComponentOrbRewardItem;
                ComponentOrbCount += componentOrbRewardItem.count;
            }
        }
    }
    void ResetCounts()
    {
        var startingBlocks = BlockDictionary.Instance.GetStartingBlocks();
        BlockCounts = new Dictionary<BlockSO, int>();
        foreach (var blockSO in startingBlocks)
        {
            if (blockSO != null)
            {
                BlockCounts.Add(blockSO, blockSO.initialCountInInventory);
            }
        }
        DiggerCount = startingDiggerCount;
        ComponentOrbCount = startingComponentOrbCount;
    }
}
