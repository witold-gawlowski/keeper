using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataWagon : Singleton<DataWagon>
{
    public MapData SelectedMapData { get; private set; }
    public int Level { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #region Handlers
    void HandleLevelConfirmed(MapData data)
    {
        SelectedMapData = data;
    }
    void HandleGameStarted()
    {
        Level = 1;
    }
    void HandleLevelCompleted()
    {
        Level++;
    }
    #endregion
}
