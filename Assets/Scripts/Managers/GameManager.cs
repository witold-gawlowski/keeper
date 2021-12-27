using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public System.Action mainSceneStartedEvent;
    public System.Action mainSceneEndedEvent;
    public System.Action surrenderEvent;
    public MapData SelectedMapData { get; private set; }
    public int Level { get; private set; }

    [SerializeField] private UnityEditor.SceneAsset mainScene;
    [SerializeField] private UnityEditor.SceneAsset menuScene;
    [SerializeField] private UnityEditor.SceneAsset levelSelectionScene;

    private Dictionary<string, System.Action> sceneLoadedHandlers;
    private Dictionary<string, System.Action> sceneUnloadedHandlers;

    #region Unity Functions
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneLoadedHandler;
        SceneManager.sceneUnloaded += SceneUnloadedHandler;
        InitializeSceneLoadedHandlers();
        Init();
    }
    void Start()
    {
        MenuSceneLoadedHandler();
    }
    #endregion
    #region Custom Private Functions
    void Init()
    {
        Level = 1;
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
    void MainSceneUnloadedHandler() => UnsubscribeFromMainSceneEvents();
    void SubscribeToLevelSelectionEvents()
    {
        LevelSelectionUIManager.Instance.BackButtonPressedEvent += HandleBackToMainMenu;
        MapManager.Instance.mapConfirmedEvent += HandleLevelConfirmed;
    }
    void UnsubscribeFromLevelSelectionEvents()
    {
        if (LevelSelectionUIManager.Instance)
        {
            LevelSelectionUIManager.Instance.BackButtonPressedEvent -= HandleBackToMainMenu;
            MapManager.Instance.mapConfirmedEvent -= HandleLevelConfirmed;
        }
    }
    void UnsubscribeFromMainMenuEvents()
    {
        if (MainMenuUIManager.Instance)
        {
            MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent -= HandleLevelSelectionOpen;
        }
    }
    void SubscribeToMainSceneEvents()
    {
        BlockManager.Instance.SubscribeToMainSceneEvents();
        MainSceneUIManager.Instance.surrenderPressedEvent += HandleSurrenderEvent;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += HandleLevelCompletedEvent;
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelCompleted;
    }

    void UnsubscribeFromMainSceneEvents()
    {
        BlockManager.Instance.UnsubscribeFromMainSceneEvents();
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.surrenderPressedEvent -= HandleSurrenderEvent;
        }
        if (MainSceneManager.Instance)
        {
            MainSceneManager.Instance.levelCompletedEvent -= HandleLevelCompleted;
        }
    }
    #endregion
    #region Handlers
    void SceneLoadedHandler(Scene scene, LoadSceneMode _)
    {
        if (sceneLoadedHandlers.ContainsKey(scene.name))
        {
            sceneLoadedHandlers[scene.name]?.Invoke();
        }
    }
    void SceneUnloadedHandler(Scene scene)
    {
        if (sceneUnloadedHandlers.ContainsKey(scene.name))
        {
            sceneUnloadedHandlers[scene.name]?.Invoke();
        }
    }
    void HandleLevelCompleted() => Level++;
    void MenuSceneLoadedHandler()
    {
        MainMenuUIManager.Instance.onLevelSelectionButtonPressEvent += HandleLevelSelectionOpen;
    }
    void MainSceneLoadedHandler()
    {
        SubscribeToMainSceneEvents();
        mainSceneStartedEvent();
    }   
    void HandleSurrenderEvent()
    {
        surrenderEvent?.Invoke();
        mainSceneEndedEvent?.Invoke();
        SceneManager.LoadScene(menuScene.name);
    }
    void HandleLevelSelectionOpen() => SceneManager.LoadScene(levelSelectionScene.name);
    void HandleBackToMainMenu() => SceneManager.LoadScene(menuScene.name);
    void HandleLevelConfirmed(MapData data)
    {
        SelectedMapData = data;
        SceneManager.LoadScene(mainScene.name);
    }
    void HandleLevelCompletedEvent()
    {
        mainSceneEndedEvent?.Invoke();
        SceneManager.LoadScene(menuScene.name);
    }
}
#endregion