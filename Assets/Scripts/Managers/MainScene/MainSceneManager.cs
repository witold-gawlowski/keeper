using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    public System.Action verdictStartedEvent;
    public System.Action verdictConditionsMetEvent;
    public System.Action levelCompletedEvent;
    public System.Action levelFailedEvent;
    public GameObject LevelObject { get; private set; }
    public GameObject LastBlockTouched { get; private set; }
    private MapData mapData; 
    private void Awake()
    {
        mapData = GameStateManager.Instance.SelectedMapData;
        LevelObject = Instantiate(mapData.map.prefab, transform);
    }
    private void OnEnable()
    {
        BlockManager.Instance.blockSpawnedEvent += HandleBlockSpawedEvent;
        MainSceneUIManager.Instance.verdictPressedEvent += HandleVerdictPressedEvent;
        MainSceneUIManager.Instance.cheatPressedEvent += HandleCheatPressed; 
        FillManager.Instance.finishedCalulatingAreaFractionEvent += HandleFinishedAreaCalculation;
        VerdictManager.Instance.resultEvent += HandleVerdictFinished;
        DragManager.Instance.dragFinishedEvent += HandleDragFinished;
    }
    void HandleFinishedAreaCalculation(float fraction)
    {
        float targetCompletionFraction = mapData.map.targetCompletionFraction;
        if (fraction >= targetCompletionFraction)
        {
            CoherencyManager.Instance.CalculateCoherency();
            if (CoherencyManager.Instance.IsCoherent)
            {
                verdictConditionsMetEvent();
            }
        }
    }
    void HandleBlockSpawedEvent(GameObject block)
    {
        LastBlockTouched = block;
    }
    void HandleVerdictFinished(int hits)
    {
        var maxHits = mapData.map.maxNumberOfHits;
        if(hits <= maxHits)
        {
            levelCompletedEvent?.Invoke();
            return;
        }
        levelFailedEvent?.Invoke();
    }
    void HandleVerdictPressedEvent()
    {
        verdictStartedEvent();
    }
    void HandleCheatPressed()
    {
        levelCompletedEvent?.Invoke();
    }
    void HandleDragFinished(GameObject block)
    {
        LastBlockTouched = block;
    }
    void OnDisable()
    {
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.verdictPressedEvent -= HandleVerdictPressedEvent;
            MainSceneUIManager.Instance.cheatPressedEvent -= HandleCheatPressed;
        }
        if (FillManager.Instance)
        {
            FillManager.Instance.finishedCalulatingAreaFractionEvent -= HandleFinishedAreaCalculation;
        }
        if (VerdictManager.Instance)
        {
            VerdictManager.Instance.resultEvent -= HandleVerdictFinished;
        }
    }
}
