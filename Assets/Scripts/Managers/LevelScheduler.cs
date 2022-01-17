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

    protected override void SubscribeToMenuSceneEvents()
    {
        MainMenuUIManager.Instance.startNewGameEvent += HandleNewGameStartedEvent;
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleLevelFinished;
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelFinished;
    }

    private void HandleNewGameStartedEvent()
    {
        CreateCurrentLevelData();
    }
    private void HandleLevelFinished()
    {
        CreateCurrentLevelData();
    }
    private void CreateCurrentLevelData()
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
    private List<IRewardItem> CreateReward(LevelGroupSO levelGroup)
    {
        var result = new List<IRewardItem>();
        var count = Random.Range(minRewardItemsCount, maxRewardItemsCount);
        for(int i = 0; i<count; i++)
        {
            var rewardBlockSO = levelGroup.GetBlock();
            var rewardMultiplicity = 
                Random.Range(rewardBlockSO.minRewardItemMultiplicity, rewardBlockSO.maxRewardItemMultiplicity);
            var rewardItem = new BlockRewardItem(rewardBlockSO, rewardMultiplicity);
            result.Add(rewardItem);
        }
        var diggerCount = Helpers.GetIntFromDistribution(levelGroup.diggerDistribution);
        if(diggerCount > 0)
        {
            var diggerReward = new DiggerRewardItem(diggerCount);
            result.Add(diggerReward);
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
