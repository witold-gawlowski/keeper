using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScheduler : GlobalManager<LevelScheduler>
{
    public List<MapData> CurrentLevelData { get; private set; }

    [SerializeField] private List<LevelGroupSO> levelGroups;

    [SerializeField] private int minRewardItemsCount = 1;
    [SerializeField] private int maxRewardItemsCount = 3;
    private List<MapSO> allMapsSorted;
    private void Start()
    {
        CreateCurrentLevelData();
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleLevelFinished;
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelFinished;
    }

    private void HandleLevelFinished()
    {
        CreateCurrentLevelData();
    }
    public void CreateCurrentLevelData()
    {
        CurrentLevelData = new List<MapData>();
        var levelGroup = GetLevelGroup();
        var currentLevelPool = levelGroup.maps;
        var currentLevel = GameStateManager.Instance.Level;
        var mapsPerLevel = levelGroup.mapsPerLevel;
        var maps = Helpers.GetRandomSubset(currentLevelPool, mapsPerLevel);
        for (int i = 0; i < mapsPerLevel; i++)
        {
            var mapData = maps[i];
            var rewards = CreateReward(levelGroup);
            var data = new MapData(mapData, rewards);
            CurrentLevelData.Add(data);
        }
    }
    private List<RewardItem> CreateReward(LevelGroupSO levelGroup)
    {
        var result = new List<RewardItem>();
        var count = Random.Range(minRewardItemsCount, maxRewardItemsCount);
        for(int i = 0; i<count; i++)
        {
            var rewardBlockSO = levelGroup.GetBlock();
            var rewardMultiplicity = 
                Random.Range(rewardBlockSO.minRewardItemMultiplicity, rewardBlockSO.maxRewadItemMultiplicity);
            var rewardItem = new RewardItem(rewardBlockSO, rewardMultiplicity);
            result.Add(rewardItem);
        }
        return result;
    }
    private LevelGroupSO GetLevelGroup()
    {
        var level = GameStateManager.Instance.Level;
        var currentGroupMaxLevel = 0;
        foreach(var g in levelGroups)
        {
            currentGroupMaxLevel += g.length;
            if(currentGroupMaxLevel >= level)
            {
                return g;
            }
        }
        return null;
    }
}
