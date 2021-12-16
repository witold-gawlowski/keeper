using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public System.Action<int, int> scoreUpdatedEvent;
    public System.Action targetFractionHitEvent;
    int hits;
    int tries;
    private void OnEnable()
    {
        SubscribeToBlockUpdates();
        FillManager.Instance.tryHitEvent += HandleFillUpdate;
    }
    void Start()
    {
        hits = 0;
        tries = 0; 
    }
    private void OnDisable()
    {
        UnsubscribeFromBlockUpdates();
    }
    void HandleFillUpdate(bool hit)
    {
        if (hit)
        {
            hits++;
        }
        tries++;
        scoreUpdatedEvent(hits, tries);
        CheckIfLevelCompleted();
    }
    void CheckIfLevelCompleted()
    {
        float completionFraction = 1.0f * hits / tries;
        float targetCompletionFraction = GameManager.Instance.SelectedLevel.targetCompletionFraction;
        if(completionFraction >= targetCompletionFraction)
        {
            targetFractionHitEvent();
        }
    }
    private void SubscribeToBlockUpdates()
    {
        DragManager.Instance.dragFinishedEvent += ResetCounter;
        BlockManager.Instance.blockSpawnedEvent += BlockSpawnedEventHandler;
;
    }
    private void UnsubscribeFromBlockUpdates()
    {
        if (DragManager.Instance)
        {
            DragManager.Instance.dragFinishedEvent -= ResetCounter;
        }
        if (BlockManager.Instance)
        {
            BlockManager.Instance.blockSpawnedEvent -= BlockSpawnedEventHandler;
        }
    }
    void BlockSpawnedEventHandler(GameObject _)
    {
        ResetCounter();
    }
    void ResetCounter()
    {
        hits = 0;
        tries = 0;
    }
}
