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
        MainMenuUIManager.Instance.startNewGameEvent += LoadInventoryScene;
        MainMenuUIManager.Instance.continueGameEvent += LoadInventoryScene;
    }
    protected override void SubscribeToInventorySceneEvents()
    {
        InventoryUIManager.Instance.continuePressedEvent += LoadMapSelectionScene;
        InventoryUIManager.Instance.backPressedEvent += LoadMenuScene;
    }
    protected override void SubscribeToMapSelectionEvents()
    {
        MapSelectionUIManager.Instance.BackButtonPressedEvent += LoadInventoryScene;
        MapManager.Instance.mapConfirmedEvent += (MapData _) => LoadMainScene();
    }
    protected override void SubscribeToMainSceneEvents()
    {
        MainSceneUIManager.Instance.surrenderPressedEvent += LoadMapSelectionScene;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += LoadMapSelectionScene;
        MainSceneUIManager.Instance.levelFailedConfirmPressedEvent += LoadMenuScene;
    }
    #endregion
    #region Handlers
    void LoadMenuScene() => SceneManager.LoadScene(Constants.MenuSceneName);
    void LoadInventoryScene() => SceneManager.LoadScene(Constants.InventorySceneName);
    void LoadMapSelectionScene() => SceneManager.LoadScene(Constants.MapSelectionSceneName);
    void LoadMainScene() => SceneManager.LoadScene(Constants.MainSceneName);
    #endregion
}