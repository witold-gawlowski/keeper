using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public System.Action<int> selectedLevelUpdatedEvent;
    public System.Action<LevelSO> levelConfirmedEvent;
    public int MaxLevels { get; private set; }
    public LevelSO SelectedLevel => levelSOs[selectedLevelIndex];

    [SerializeField] List<LevelSO> levelSOs;
    List<GameObject> levelGOs;
    int selectedLevelIndex;
    private void OnEnable()
    {
        LevelSelectionUIManager.Instance.PreviousLevelButtonPressedEvent += HandlePreviousLevelSelectedEvent;
        LevelSelectionUIManager.Instance.NextLevelButtonPressedEvent += HandleNextLevelSelectedEvent;
        LevelSelectionUIManager.Instance.StartButtonPressedEvent += HandleSelectedLevelConfirm;
    }
    void Start()
    {
        InitializeLevels();
    }
    void InitializeLevels()
    {
        levelGOs = new List<GameObject>();
        foreach (var level in levelSOs)
        {
            var newLevel = Instantiate(level.prefab);
            newLevel.SetActive(false);
            levelGOs.Add(newLevel);
        }
        MaxLevels = levelGOs.Count;
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
        SetSelectedLevel((selectedLevelIndex - 1 + MaxLevels) % MaxLevels);
        selectedLevelUpdatedEvent(selectedLevelIndex);
    }
    void HandleNextLevelSelectedEvent()
    {
        SetSelectedLevel((selectedLevelIndex + 1 + MaxLevels) % MaxLevels);
        selectedLevelUpdatedEvent(selectedLevelIndex);
    }
    void SetSelectedLevel(int index)
    {
        foreach (var l in levelGOs)
        {
            l.SetActive(false);
        }
        levelGOs[index].SetActive(true);
        selectedLevelIndex = index;
    }
    void HandleSelectedLevelConfirm()
    {
        levelConfirmedEvent(levelSOs[selectedLevelIndex]);
    }
}
