using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private List<BlockSO> blockSOs;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        GameManager.Instance.mainSceneStartedEvent += SubscribeToMainSceneEvents;
        GameManager.Instance.mainSceneEndedEvent += UnsubscrubeFromMainSceneEvents;
    }
    public List<BlockSO> GetInventory()
    {
        return blockSOs;
    }
    void SubscribeToMainSceneEvents()
    {
        GameManager.Instance.mainSceneStartedEvent += HandleLevelCompleted;
    }
    void UnsubscrubeFromMainSceneEvents()
    {
        GameManager.Instance.mainSceneEndedEvent -= HandleLevelCompleted;
    }
    void HandleLevelCompleted()
    {
        AddReward();
    }
    void AddReward()
    {
        var reward = GameManager.Instance.SelectedMapData.reward;
        foreach(var rewardItem in reward)
        {
            rewardItem.block.countInInventory += rewardItem.count;
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.mainSceneStartedEvent -= SubscribeToMainSceneEvents;
        GameManager.Instance.mainSceneEndedEvent -= UnsubscrubeFromMainSceneEvents;
    }
}
