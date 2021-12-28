using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public System.Action<int> selectedMapUpdatedEvent;
    public System.Action<MapData> mapConfirmedEvent;
    public int MaxMaps { get; private set; }

    Dictionary<MapData, GameObject> mapObjectsByMapData;
    int selectedMapIndex;
    private void OnEnable()
    {
        LevelSelectionUIManager.Instance.PreviousLevelButtonPressedEvent += HandlePreviousLevelSelectedEvent;
        LevelSelectionUIManager.Instance.NextLevelButtonPressedEvent += HandleNextLevelSelectedEvent;
        LevelSelectionUIManager.Instance.StartButtonPressedEvent += HandleSelectedLevelConfirm;
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
        if (LevelSelectionUIManager.Instance)
        {
            LevelSelectionUIManager.Instance.PreviousLevelButtonPressedEvent -= HandlePreviousLevelSelectedEvent;
            LevelSelectionUIManager.Instance.NextLevelButtonPressedEvent -= HandleNextLevelSelectedEvent;
            LevelSelectionUIManager.Instance.StartButtonPressedEvent -= HandleSelectedLevelConfirm;
        }
    }
    void HandlePreviousLevelSelectedEvent()
    {
        SetSelectedMap((selectedMapIndex - 1 + MaxMaps) % MaxMaps);
        selectedMapUpdatedEvent(selectedMapIndex);
    }
    void HandleNextLevelSelectedEvent()
    {
        SetSelectedMap((selectedMapIndex + 1 + MaxMaps) % MaxMaps);
        selectedMapUpdatedEvent(selectedMapIndex);
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
    void HandleSelectedLevelConfirm()
    {
        var selectedMapData = LevelScheduler.Instance.CurrentLevelData[selectedMapIndex];
        mapConfirmedEvent(selectedMapData);
        var mainScene = SceneLoader.Instance.MainScene;
        
    }
}
