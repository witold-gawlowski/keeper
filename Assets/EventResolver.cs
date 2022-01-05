using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventResolver : GlobalManager<EventResolver>
{
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelCompleted;
    }
    void HandleLevelCompleted()
    {
        GameStateManager.Instance.HandleLevelCompleted();
        InventoryManager.Instance.HandleLevelCompleted();
    }
}
