using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbstractGlobalManager : Singleton<AbstractGlobalManager>
{
    private Dictionary<string, System.Action> sceneLoadedHandlers;
    private Dictionary<string, System.Action> sceneUnloadedHandlers;

    [SerializeField] protected UnityEditor.SceneAsset MenuScene { get; private set; }
    [SerializeField] protected UnityEditor.SceneAsset LevelSelectionScene { get; private set; }
    [SerializeField] protected UnityEditor.SceneAsset MainScene { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeSceneLoadedHandlers();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoadedHandler;
        SceneManager.sceneUnloaded += SceneUnloadedHandler;
    }
    void InitializeSceneLoadedHandlers()
    {
        sceneLoadedHandlers = new Dictionary<string, System.Action>()
        {
            { MenuScene.name, SubscribeToMenuSceneEvents},
            { LevelSelectionScene.name,  SubscribeToLevelSelectionEvents},
            { MainScene.name, SubscribeToMainSceneEvents },
        };
        sceneUnloadedHandlers = new Dictionary<string, System.Action>()
        {
            { MenuScene.name, UnsubscribeFromMenuSceneEvents},
            { LevelSelectionScene.name,  UnsubscribeFromLevelSelectionEvents},
            { MainScene.name,  UnsubscribeFromMainSceneEvents },
        };
    }
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
    public virtual void SubscribeToMenuSceneEvents()
    {

    }
    public virtual void SubscribeToLevelSelectionEvents()
    {

    }
    public virtual void SubscribeToMainSceneEvents()
    {

    }
    public virtual void UnsubscribeFromMenuSceneEvents()
    {

    }
    public virtual void UnsubscribeFromLevelSelectionEvents()
    {

    }
    public virtual void UnsubscribeFromMainSceneEvents()
    {

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoadedHandler;
        SceneManager.sceneUnloaded -= SceneUnloadedHandler;
    }
}
