using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public System.Action<int> selectedMapUpdatedEvent;
    public System.Action<LevelSO> mapConfirmedEvent;
    public int MaxMaps { get; private set; }
    public LevelSO SelectedLevel => mapSOs[selectedMapIndex];

    List<LevelSO> mapSOs;
    List<GameObject> mapGOs;
    int selectedMapIndex;
    private void OnEnable()
    {
        LevelSelectionUIManager.Instance.PreviousLevelButtonPressedEvent += HandlePreviousLevelSelectedEvent;
        LevelSelectionUIManager.Instance.NextLevelButtonPressedEvent += HandleNextLevelSelectedEvent;
        LevelSelectionUIManager.Instance.StartButtonPressedEvent += HandleSelectedLevelConfirm;
    }
    void Awake()
    {
        var level = GameManager.Instance.Level;
        mapSOs = LevelScheduler.Instance.GetMaps(level);
        InitializeLevels();
    }
    void InitializeLevels()
    {
        mapGOs = new List<GameObject>();
        foreach (var level in mapSOs)
        {
            var newLevel = Instantiate(level.prefab);
            newLevel.SetActive(false);
            mapGOs.Add(newLevel);
        }
        MaxMaps = mapGOs.Count;
        SetSelectedLevel(0);
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
        SetSelectedLevel((selectedMapIndex - 1 + MaxMaps) % MaxMaps);
        selectedMapUpdatedEvent(selectedMapIndex);
    }
    void HandleNextLevelSelectedEvent()
    {
        SetSelectedLevel((selectedMapIndex + 1 + MaxMaps) % MaxMaps);
        selectedMapUpdatedEvent(selectedMapIndex);
    }
    void SetSelectedLevel(int index)
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
