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
    MapData mapData;
    private void Awake()
    {
        mapData = GameStateManager.Instance.SelectedMapData;
        LevelObject = Instantiate(mapData.map.prefab, transform);
    }
    private void OnEnable()
    {
        MainSceneUIManager.Instance.verdictPressedEvent += HandleVerdictPressedEvent;
        FillManager.Instance.finishedCalulatingAreaFractionEvent += HandleFinishedAreaCalculation;
        VerdictManager.Instance.resultEvent += HandleVerdictFinished; 
    }
    void HandleFinishedAreaCalculation(float fraction)
    {
        float targetCompletionFraction = mapData.map.targetCompletionFraction;
        if (fraction >= targetCompletionFraction)
        {
            if (CoherencyCalculator.IsCoherent())
            {
                verdictConditionsMetEvent();
            }
        }
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
    void OnDisable()
    {
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.verdictPressedEvent -= HandleVerdictPressedEvent;
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
