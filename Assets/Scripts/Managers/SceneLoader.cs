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
        MainMenuUIManager.Instance.startNewGameEvent += LoadLevelSelectionScene;
        MainMenuUIManager.Instance.continueGameEvent += LoadLevelSelectionScene;
    }
    protected override void SubscribeToLevelSelectionEvents()
    {
        LevelSelectionUIManager.Instance.BackButtonPressedEvent += LoadMenuScene;
        MapManager.Instance.mapConfirmedEvent += (MapData _) => LoadMainScene();
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.surrenderPressedEvent += LoadMenuScene;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += LoadLevelSelectionScene;
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += LoadMenuScene;
    }
    #endregion
    #region Handlers
    void LoadMenuScene() => SceneManager.LoadScene(MenuScene.name);
    void LoadLevelSelectionScene() => SceneManager.LoadScene(LevelSelectionScene.name);
    void LoadMainScene() => SceneManager.LoadScene(MainScene.name);
    #endregion
}