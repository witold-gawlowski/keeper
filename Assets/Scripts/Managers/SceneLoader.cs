using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : GlobalManager<SceneLoader>
{
    public System.Action mainSceneStartedEvent;
    public System.Action mainSceneEndedEvent;
    public System.Action gameStartedEvent;

    #region Unity Functions
    protected override void OnEnable()
    {
        base.OnEnable();
        gameStartedEvent += HandleGameStartedEvent;
    }
    void Start()
    {
        gameStartedEvent();
    }
    #endregion
    #region Custom Private Functions
    public override void SubscribeToMenuSceneEvents()
    {
        Debug.Log("scene loader subscribe to main menu events");
        MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent += HandleLevelSelectionOpen;
    }
    public override void UnsubscribeFromMenuSceneEvents()
    {
        if (MainMenuUIManager.Instance)
        {
            MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent -= HandleLevelSelectionOpen;
        }
    }
    public override void SubscribeToLevelSelectionEvents()
    {
        LevelSelectionUIManager.Instance.BackButtonPressedEvent += HandleBackToMainMenu;
        MapManager.Instance.mapConfirmedEvent += HandleLevelConfirmed;
    }
    public override void UnsubscribeFromLevelSelectionEvents()
    {
        if (LevelSelectionUIManager.Instance)
        {
            LevelSelectionUIManager.Instance.BackButtonPressedEvent -= HandleBackToMainMenu;
            MapManager.Instance.mapConfirmedEvent -= HandleLevelConfirmed;
        }
    }
    public override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleSurrenderEvent;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += HandleLevelCompletedEvent;
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += HandledLevelFailedConfirmedEvent;
    }

    public override void UnsubscribeFromMainSceneEvents()
    {
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.surrenderPressedEvent -= HandleSurrenderEvent;
            MainSceneUIManager.Instance.levelFailedConfirmPressedEvent -= HandledLevelFailedConfirmedEvent;
            MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent -= HandleLevelCompletedEvent;
        }
    }
    #endregion
    #region Handlers
    void HandleSurrenderEvent() => SceneManager.LoadScene(MenuScene.name);
    void HandleGameStartedEvent() => SceneManager.LoadScene(MenuScene.name);
    void HandleBackToMainMenu() => SceneManager.LoadScene(MenuScene.name);
    void HandleLevelSelectionOpen() => SceneManager.LoadScene(LevelSelectionScene.name);
    void HandleLevelConfirmed(MapData _) => SceneManager.LoadScene(MainScene.name);
    void HandleLevelCompletedEvent() => SceneManager.LoadScene(MenuScene.name);
    void HandledLevelFailedConfirmedEvent() => SceneManager.LoadScene(MenuScene.name);
    #endregion
}