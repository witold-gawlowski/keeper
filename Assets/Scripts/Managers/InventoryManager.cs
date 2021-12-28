using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private List<BlockSO> blockSOs;
    Dictionary<BlockSO, int> blockCounts;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneLoader.Instance.mainSceneStartedEvent += SubscribeToMainSceneEvents;
        SceneLoader.Instance.mainSceneEndedEvent += UnsubscribeFromMainSceneEvents;
        SceneLoader.Instance.gameStartedEvent += ResetCounts;
    }
    public Dictionary<BlockSO, int> GetInventory()
    {
        return blockCounts;
    }
    void ResetCounts()
    {
        Debug.Log("resetting counts");
        blockCounts = new Dictionary<BlockSO, int>();
        foreach(var blockSO in blockSOs)
        {
            blockCounts.Add(blockSO, blockSO.initialCountInInventory);
        }
    }
    void SubscribeToMainSceneEvents()
    {
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelCompleted;
    }
    void UnsubscribeFromMainSceneEvents()
    {
        MainSceneManager.Instance.levelCompletedEvent -= HandleLevelCompleted;
    }
    void HandleStartGame()
    {
        ResetCounts();
    }
    void HandleLevelCompleted()
    {
        RemoveUsedBlocks();
        AddReward();
    }
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
    private void OnDisable()
    {
        if (SceneLoader.Instance)
        {
            SceneLoader.Instance.mainSceneStartedEvent -= SubscribeToMainSceneEvents;
            SceneLoader.Instance.mainSceneEndedEvent -= UnsubscribeFromMainSceneEvents;
            SceneLoader.Instance.gameStartedEvent -= ResetCounts;
        }
    }
}
