using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    public System.Action verdictStartedEvent;
    public System.Action targetFractionHitEvent;
    public System.Action levelCompleted;
    public System.Action levelFailed;
    public GameObject LevelObject { get; private set; }
    private void Awake()
    {
        LevelObject = Instantiate(GameManager.Instance.SelectedMap.prefab, transform);
    }
    private void OnEnable()
    {
        MainSceneUIManager.Instance.verdictPressedEvent += HandleVerdictPressedEvent;
        FillManager.Instance.finishedCalulatingAreaFractionEvent += HandleFinishedAreaCalculation;
        VerdictManager.Instance.resultEvent += HandleVerdictFinished; 
    }
    void HandleFinishedAreaCalculation(float fraction)
    {
        float targetCompletionFraction = GameManager.Instance.SelectedMap.targetCompletionFraction;
        if (fraction >= targetCompletionFraction)
        {
            targetFractionHitEvent();
        }
    }
    void HandleVerdictFinished(int hits)
    {
        var maxHits = GameManager.Instance.SelectedMap.maxNumberOfHits;
        if(hits <= maxHits)
        {
            levelCompleted?.Invoke();
            return;
        }
        levelFailed?.Invoke();
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
