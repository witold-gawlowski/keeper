using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GlobalManager<GameStateManager>
{
    public MapData SelectedMapData { get; private set; }
    public int Level { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        HandleGameStarted();
    }
    public override void SubscribeToLevelSelectionEvents()
    {
        MapManager.Instance.mapConfirmedEvent += HandleMapConfirmed;
    }
    public override void SubscribeToMainSceneEvents()
    {
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelCompleted;
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += HandleGameStarted;
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleGameStarted;
    }
    #region Hanlders
    void HandleMapConfirmed(MapData data)
    {
        SelectedMapData = data;
    }
    void HandleGameStarted()
    {
        Level = 1;
    }
    void HandleLevelCompleted()
    {
        Level++;
    }
    #endregion Handlers
}
