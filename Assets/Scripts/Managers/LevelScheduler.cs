using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScheduler : Singleton<LevelScheduler>
{
    public List<MapData> CurrentLevelData { get; private set; }

    [SerializeField] private List<MapSO> availableMaps;
    [SerializeField] private List<BlockSO> availableBlocks;

    [SerializeField] private int mapsPerLevel = 2;
    [SerializeField] private int minRewardItemsCount = 1;
    [SerializeField] private int maxRewardItemsCount = 3;
    [SerializeField] private int minRewardItemMultiplicity = 1;
    [SerializeField] private int maxRewadItemMultiplicity = 3;

    private List<MapSO> allMapsSorted;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateCurrentLevelData()
    {
        CurrentLevelData = new List<MapData>();
        var currentLevelPool = new List<MapSO>();
        var currentLevel = GameStateManager.Instance.Level;
        foreach(var map in availableMaps)
        {
            if(currentLevel <= map.endingLevel && currentLevel >= map.startingLevel)
            {
                currentLevelPool.Add(map);
            }
        }
        var maps = Helpers.GetRandomSubset(currentLevelPool, mapsPerLevel);
        for(int i=0; i<mapsPerLevel; i++)
        {
            var mapData = maps[i];
            var rewards = CreateReward();
            var data = new MapData(mapData, rewards);
            CurrentLevelData.Add(data);
        }
    }
    public List<RewardItem> CreateReward()
    {
        var result = new List<RewardItem>();
        var count = Random.Range(minRewardItemsCount, maxRewardItemsCount);
        for(int i = 0; i<count; i++)
        {
            var randomRewardBlockIndex = Random.Range(0, availableBlocks.Count);
            var rewardBlock = availableBlocks[randomRewardBlockIndex];
            var rewardMultiplicity = Random.Range(minRewardItemMultiplicity, maxRewadItemMultiplicity);
            var rewardItem = new RewardItem(rewardBlock, rewardMultiplicity);
            result.Add(rewardItem);
        }
        return result;
    }
}
