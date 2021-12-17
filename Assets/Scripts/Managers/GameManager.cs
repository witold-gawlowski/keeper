using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public System.Action mainSceneStartedEvent;
    public System.Action surrenderEvent;
    public LevelSO SelectedLevel { get; private set; }

    [SerializeField] private UnityEditor.SceneAsset mainScene;
    [SerializeField] private UnityEditor.SceneAsset menuScene;
    [SerializeField] private UnityEditor.SceneAsset levelSelectionScene;

    private Dictionary<string, System.Action> sceneLoadedHandlers;
    private Dictionary<string, System.Action> sceneUnloadedHandlers;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        InitializeSceneLoadedHandlers();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        MenuSceneLoadedHandler();
    }
    void InitializeSceneLoadedHandlers()
    {
        sceneLoadedHandlers = new Dictionary<string, System.Action>()
        {
            { menuScene.name, MenuSceneLoadedHandler},
            { levelSelectionScene.name,  SubscribeToLevelSelectionEvents},
            { mainScene.name, MainSceneLoadedHandler },
        };
        sceneUnloadedHandlers = new Dictionary<string, System.Action>()
        {
            { menuScene.name, UnsubscribeFromMainMenuEvents },
            { levelSelectionScene.name,  UnsubscribeFromLevelSelectionEvents},
            { mainScene.name,  MainSceneUnloadedHandler },
        };
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        sceneLoadedHandlers[scene.name]?.Invoke();
    }
    void OnSceneUnloaded(Scene scene)
    {
        sceneUnloadedHandlers[scene.name]?.Invoke();
    }
    void SubscribeToLevelSelectionEvents()
    {
        LevelSelectionUIManager.Instance.BackButtonPressedEvent += HandleBackToMainMenu;
        LevelManager.Instance.levelConfirmedEvent += HandleLevelConfirmed;
    }
    void UnsubscribeFromLevelSelectionEvents()
    {
        if (LevelSelectionUIManager.Instance) {
            LevelSelectionUIManager.Instance.BackButtonPressedEvent -= HandleBackToMainMenu;
            LevelManager.Instance.levelConfirmedEvent -= HandleLevelConfirmed;
        }
    }
    void MenuSceneLoadedHandler()
    {
        MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent += HandleLevelSelectionOpen;
    }
    void UnsubscribeFromMainMenuEvents()
    {
        if (MainMenuUIManager.Instance)
        {
            MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent -= HandleLevelSelectionOpen;
        }
    }
    void MainSceneLoadedHandler()
    {
        SubscribeToMainSceneEvents();
        mainSceneStartedEvent();
    }
    void SubscribeToMainSceneEvents()
    {
        BlockManager.Instance.SubscribeToMainSceneEvents();
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleSurrenderEvent;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += HandleLevelCompletedEvent;
    }
    void MainSceneUnloadedHandler()
    {   
        UnsubscribeFromMainSceneEvents();
    }
    void UnsubscribeFromMainSceneEvents()
    {
        BlockManager.Instance.UnsubscribeFromMainSceneEvents();
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.surrenderPressedEvent -= HandleSurrenderEvent;
        }
    }
    void HandleSurrenderEvent()
    {
        surrenderEvent?.Invoke();
        SceneManager.LoadScene(menuScene.name);
    }
    void HandleLevelSelectionOpen() => SceneManager.LoadScene(levelSelectionScene.name);
    void HandleBackToMainMenu() => SceneManager.LoadScene(menuScene.name);
    void HandleLevelConfirmed(LevelSO level)
    {
        SelectedLevel = level;
        SceneManager.LoadScene(mainScene.name);
    }
    void HandleLevelCompletedEvent()
    {
        SceneManager.LoadScene(menuScene.name);
    }
}
