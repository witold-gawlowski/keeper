using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public System.Action SurrenderEvent;
    public LevelSO SelectedLevel { get; private set; }

    [SerializeField] private UnityEditor.SceneAsset mainScene;
    [SerializeField] private UnityEditor.SceneAsset menuScene;
    [SerializeField] private UnityEditor.SceneAsset levelSelectionScene;

    private Dictionary<string, System.Action> sceneSubsriptions;
    private Dictionary<string, System.Action> sceneUnsubscriptions;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        CreateSceneSubscriptionsDictionary();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SubscribeToMainMenuEvents();
    }
    void CreateSceneSubscriptionsDictionary()
    {
        sceneSubsriptions = new Dictionary<string, System.Action>()
        {
            { menuScene.name, SubscribeToMainMenuEvents},
            { levelSelectionScene.name,  SubscribeToLevelSelectionEvents},
            { mainScene.name, SubscribeToMainSceneEvents },
        };
        sceneUnsubscriptions = new Dictionary<string, System.Action>()
        {
            { menuScene.name, UnsubscribeFromMainMenuEvents },
            { levelSelectionScene.name,  UnsubscribeFromLevelSelectionEvents},
            { mainScene.name,  UnsubscribeFromMainSceneEvents },
        };
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        sceneSubsriptions[scene.name]?.Invoke();
    }
    void OnSceneUnloaded(Scene scene)
    {
        sceneUnsubscriptions[scene.name]?.Invoke();
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
    void SubscribeToMainMenuEvents()
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
    void SubscribeToMainSceneEvents()
    {
        BlockManager.Instance.SubscribeToMainSceneEvents();
        MainSceneUIManager.Instance.SurrenderPressedEvent += HandleSurrenderEvent;
        MainSceneUIManager.Instance.LevelCompletedConfirmPressedEvent += HandleLevelCompletedEvent;
    }
    void UnsubscribeFromMainSceneEvents()
    {
        BlockManager.Instance.UnsubscribeFromMainSceneEvents();
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.SurrenderPressedEvent -= HandleSurrenderEvent;
        }
    }
    void HandleSurrenderEvent()
    {
        SurrenderEvent?.Invoke();
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
