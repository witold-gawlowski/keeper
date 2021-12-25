using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public System.Action<int> selectedMapUpdatedEvent;
    public System.Action<MapSO> mapConfirmedEvent;
    public int MaxMaps { get; private set; }
    public MapSO SelectedLevel => mapSOs[selectedMapIndex];

    List<MapSO> mapSOs;
    List<GameObject> mapGOs;
    int selectedMapIndex;
    private void OnEnable()
    {
        LevelSelectionUIManager.Instance.PreviousLevelButtonPressedEvent += HandlePreviousLevelSelectedEvent;
        LevelSelectionUIManager.Instance.NextLevelButtonPressedEvent += HandleNextLevelSelectedEvent;
        LevelSelectionUIManager.Instance.StartButtonPressedEvent += HandleSelectedLevelConfirm;
    }
    public void Init()
    {
        var level = GameManager.Instance.Level;
        mapSOs = LevelScheduler.Instance.CurrentLevelMaps;
        InitializeMapGOs();
    }

    void InitializeMapGOs()
    {
        mapGOs = new List<GameObject>();
        foreach (var map in mapSOs)
        {
            var newMap = Instantiate(map.prefab);
            newMap.SetActive(false);
            mapGOs.Add(newMap);
        }
        MaxMaps = mapGOs.Count;
        SetSelectedMap(0);
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
        foreach (var l in mapGOs)
        {
            l.SetActive(false);
        }
        mapGOs[index].SetActive(true);
        selectedMapIndex = index;
    }
    void HandleSelectedLevelConfirm()
    {
        mapConfirmedEvent(mapSOs[selectedMapIndex]);
    }
}
