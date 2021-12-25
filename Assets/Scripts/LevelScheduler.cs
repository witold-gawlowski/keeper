using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScheduler : Singleton<LevelScheduler>
{
    public class RewardItem
    {
        public BlockSO block;
        public int count;
        [HideInInspector]public Sprite Sprite { get; private set; }
        public RewardItem(BlockSO block, int count)
        {
            this.block = block;
            this.count = count;
            var blockScript = block.prefab.GetComponent<BlockScript>();
            Sprite = blockScript.GetSprite();
        }
    }
    public List<MapSO> CurrentLevelMaps { get; private set; }
    public List<List<LevelScheduler.RewardItem>> CurrentLevelRewards { get; private set; }

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
        Init();
    }
    public void Init()
    {
        allMapsSorted = new List<MapSO>(availableMaps);
        System.Comparison<MapSO> comparer =
            (level1, level2) => level1.progressIndex.CompareTo(level2.progressIndex);
        allMapsSorted.Sort(comparer);
    }
    public void CreateCurrentLevelData()
    {
        CreateMaps();
        CreateRewards();
    }
    public void CreateMaps()
    {
        var level = GameManager.Instance.Level;
        var rangeStartIndex = (level - 1) * mapsPerLevel;
        var mapCount = availableMaps.Count;
        try
        {
            CurrentLevelMaps = availableMaps.GetRange(rangeStartIndex, mapsPerLevel);
        }
        catch (System.ArgumentException ex)
        {
            Debug.Log(ex.Message);
        }
    }
    void CreateRewards()
    {
        CurrentLevelRewards = new List<List<LevelScheduler.RewardItem>>();
        foreach (var map in CurrentLevelMaps)
        {
            var reward = CreateReward();
            CurrentLevelRewards.Add(reward);
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
