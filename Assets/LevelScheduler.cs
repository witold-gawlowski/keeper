using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScheduler : Singleton<LevelScheduler>
{
    [SerializeField] private List<LevelSO> availableMaps;
    [SerializeField] private int mapsPerLevel = 2;

    private List<LevelSO> sortedMaps;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    public List<LevelSO> GetMaps(int level)
    {
        var result = new List<LevelSO>();
        var rangeStartIndex = (level - 1) * mapsPerLevel;
        var mapCount = availableMaps.Count;
        try
        {
            result = availableMaps.GetRange(rangeStartIndex, mapsPerLevel);
        }
        catch (System.ArgumentException ex) {
            Debug.Log(ex.Message);
        }
        return result;
    }
    void Init()
    {
        sortedMaps = new List<LevelSO>(availableMaps);
        System.Comparison<LevelSO> comparer =
            (level1, level2) => level1.progressIndex.CompareTo(level2.progressIndex);
        sortedMaps.Sort(comparer);
    }

}
