using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GlobalManager<GameStateManager>
{
    public MapData SelectedMapData { get; private set; }
    public int Level { get; private set; }
    public void HandleLevelCompleted()
    {
        Level++;
    }
    protected override void Awake()
    {
        base.Awake();
        HandleGameStarted();
    }
    protected override void SubscribeToLevelSelectionEvents()
    {
        MapManager.Instance.mapConfirmedEvent += HandleMapConfirmed;
    }
    protected override void SubscribeToMainSceneEvents()
    {
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

    #endregion Handlers
}
