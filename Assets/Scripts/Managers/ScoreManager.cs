using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public System.Action targetFractionHitEvent;

    private void OnEnable()
    {
        FillManager.Instance.finishedCalulatingAreaFractionEvent += HandleFinishedAreaCalculation;
    }
    private void OnDisable()
    {
        FillManager.Instance.finishedCalulatingAreaFractionEvent -= HandleFinishedAreaCalculation;
    }
    void HandleFinishedAreaCalculation(float fraction)
    {
        float targetCompletionFraction = GameManager.Instance.SelectedLevel.targetCompletionFraction;
        if (fraction >= targetCompletionFraction)
        {
            targetFractionHitEvent();
        }
    }
}
