using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GlobalManager<GameStateManager>
{
    public MapData SelectedMapData { get; private set; }
    public int Level { get; private set; }
    public bool GameInProgress { get; private set; }
    public void HandleLevelCompleted()
    {
        Level++;
    }
    protected override void SubscribeToMenuSceneEvents()
    {
        MainMenuUIManager.Instance.startNewGameEvent += HandleGameStarted;
    }
    protected override void SubscribeToMapSelectionEvents()
    {
        MapManager.Instance.mapConfirmedEvent += HandleMapConfirmed;
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += HandleGameEnded;
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleGameEnded;
        MainSceneManager.Instance.levelFailedEvent += HandleGameEnded;
    }
    #region Handlers
    void HandleMapConfirmed(MapData data)
    {
        SelectedMapData = data;
    }
    void HandleGameEnded()
    {
        Level = 1;
        GameInProgress = false;
    }
    void HandleGameStarted()
    {
        Level = 1;
        GameInProgress = true;
    }

    #endregion Handlers
}
