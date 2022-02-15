using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScheduler : GlobalManager<LevelScheduler>
{
    public List<MapData> CurrentLevelData { get; private set; }

    [SerializeField] private List<LevelGroupSO> levelGroups;

    private List<MapSO> allMapsSorted;

    protected override void SubscribeToMenuSceneEvents()
    {
        MainMenuUIManager.Instance.startNewGameEvent += HandleNewGameStartedEvent;
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelFinished;
        MainSceneManager.Instance.levelFailedEvent += HandleLevelFailed;
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleLevelFailed;
    }
    private void HandleLevelFailed()
    {
        GameStateManager.Instance.SelectedMapData.Surrendered = true;
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
        var mapsPerLevel = Helpers.GetIntFromDistribution(levelGroup.mapsPerLevelDistribution);
        var maps = Helpers.GetRandomSubset(currentLevelPool, mapsPerLevel);
        for (int i = 0; i < mapsPerLevel; i++)
        {
            var mapData = maps[i];
            var rewards = CreateReward(levelGroup);
            var completionFraction = 
                Random.Range(mapData.minTargetCompletionFraction, mapData.maxTargetCompletionFraction);
            var data = new MapData(mapData, rewards, completionFraction);
            CurrentLevelData.Add(data);
        }
    }
    private List<IRewardItem> CreateReward(LevelGroupSO levelGroup)
    {
        var result = new List<IRewardItem>();
        var count = Helpers.GetIntFromDistribution(levelGroup.rewardCountDistribution);
        for(int i = 0; i<count; i++)
        {
            var rewardBlockSO = BlockDictionary.Instance.GetBlockWithRarity();
            var rewardMultiplicity = Helpers.GetIntFromDistribution(rewardBlockSO.rewardMultiplicityDistribution);
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
