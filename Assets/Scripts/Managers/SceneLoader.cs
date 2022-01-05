using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : GlobalManager<SceneLoader>
{
    #region Unity Functions
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    #endregion
    #region Custom Private Functions
    protected override void SubscribeToMenuSceneEvents()
    {
        MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent += HandleLevelSelectionOpen;
    }
    protected override void UnsubscribeFromMenuSceneEvents()
    {
        if (MainMenuUIManager.Instance)
        {
            MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent -= HandleLevelSelectionOpen;
        }
    }
    protected override void SubscribeToLevelSelectionEvents()
    {
        LevelSelectionUIManager.Instance.BackButtonPressedEvent += HandleBackToMainMenu;
        MapManager.Instance.mapConfirmedEvent += HandleLevelConfirmed;
    }
    protected override void UnsubscribeFromLevelSelectionEvents()
    {
        if (LevelSelectionUIManager.Instance)
        {
            LevelSelectionUIManager.Instance.BackButtonPressedEvent -= HandleBackToMainMenu;
            MapManager.Instance.mapConfirmedEvent -= HandleLevelConfirmed;
        }
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleSurrenderEvent;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += HandleLevelCompletedConfirmPressed;
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += HandledLevelFailedConfirmedEvent;
    }

    protected override void UnsubscribeFromMainSceneEvents()
    {
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.surrenderPressedEvent -= HandleSurrenderEvent;
            MainSceneUIManager.Instance.levelFailedConfirmPressedEvent -= HandledLevelFailedConfirmedEvent;
            MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent -= HandleLevelCompletedConfirmPressed;
        }
    }
    #endregion
    #region Handlers
    void HandleSurrenderEvent() => SceneManager.LoadScene(MenuScene.name);
    void HandleBackToMainMenu() => SceneManager.LoadScene(MenuScene.name);
    void HandleLevelSelectionOpen() => SceneManager.LoadScene(LevelSelectionScene.name);
    void HandleLevelConfirmed(MapData _) => SceneManager.LoadScene(MainScene.name);
    void HandleLevelCompletedConfirmPressed() => SceneManager.LoadScene(MenuScene.name);
    void HandledLevelFailedConfirmedEvent() => SceneManager.LoadScene(MenuScene.name);
    #endregion
}