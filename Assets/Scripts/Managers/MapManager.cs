using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public System.Action<int> selectedMapChangedEvent;
    public System.Action<MapData> mapConfirmedEvent;
    public int MaxMaps { get; private set; }

    Dictionary<MapData, GameObject> mapObjectsByMapData;
    int selectedMapIndex;
    private void OnEnable()
    {
        MapSelectionUIManager.Instance.PreviousLevelButtonPressedEvent += HandlePreviousLevelSelectedEvent;
        MapSelectionUIManager.Instance.NextLevelButtonPressedEvent += HandleNextLevelSelectedEvent;
        MapSelectionUIManager.Instance.StartButtonPressedEvent += HandleMapSelectedConfirm;
    }
    public void Init()
    {
        var level = GameStateManager.Instance.Level;
        InitializeMapGOs();
        SetSelectedMap(0);
    }

    void InitializeMapGOs()
    {
        mapObjectsByMapData = new Dictionary<MapData, GameObject>();
        var mapsData = LevelScheduler.Instance.CurrentLevelData;
        foreach (var data in mapsData)
        {
            var newMap = Instantiate(data.map.prefab);
            mapObjectsByMapData.Add(data, newMap);
        }
        MaxMaps = mapObjectsByMapData.Count;
    }
    void OnDisable()
    {
        if (MapSelectionUIManager.Instance)
        {
            MapSelectionUIManager.Instance.PreviousLevelButtonPressedEvent -= HandlePreviousLevelSelectedEvent;
            MapSelectionUIManager.Instance.NextLevelButtonPressedEvent -= HandleNextLevelSelectedEvent;
            MapSelectionUIManager.Instance.StartButtonPressedEvent -= HandleMapSelectedConfirm;
        }
    }
    void HandlePreviousLevelSelectedEvent()
    {
        SetSelectedMap((selectedMapIndex - 1 + MaxMaps) % MaxMaps);
        selectedMapChangedEvent(selectedMapIndex);
    }
    void HandleNextLevelSelectedEvent()
    {
        SetSelectedMap((selectedMapIndex + 1 + MaxMaps) % MaxMaps);
        selectedMapChangedEvent(selectedMapIndex);
    }
    void SetSelectedMap(int index)
    {
        foreach (var l in mapObjectsByMapData.Values)
        {
            l.SetActive(false);
        }
        var selectedMapData = LevelScheduler.Instance.CurrentLevelData[index];
        var selectedMapObject = mapObjectsByMapData[selectedMapData];
        selectedMapObject.SetActive(true);
        selectedMapIndex = index;
    }
    void HandleMapSelectedConfirm()
    {
        var selectedMapData = LevelScheduler.Instance.CurrentLevelData[selectedMapIndex];
        mapConfirmedEvent?.Invoke(selectedMapData);        
    }
}
