using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager<T> : Singleton<T> where T: MonoBehaviour
{
    private Dictionary<string, System.Action> sceneLoadedHandlers;
    private Dictionary<string, System.Action> sceneUnloadedHandlers;

    public UnityEditor.SceneAsset MenuScene;
    public UnityEditor.SceneAsset LevelSelectionScene;
    public UnityEditor.SceneAsset MainScene;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeSceneLoadedHandlers();
    }
    protected virtual void OnEnable()
    {
        var obj = this.name;
        SceneManager.sceneLoaded += SceneLoadedHandler;
        SceneManager.sceneUnloaded += SceneUnloadedHandler;
    }
    void InitializeSceneLoadedHandlers()
    {
        var obj = this.name;
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
        var obj = this.name;
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
    protected virtual void SubscribeToMenuSceneEvents()
    {
    }
    protected virtual void SubscribeToLevelSelectionEvents()
    {
    }
    protected virtual void SubscribeToMainSceneEvents()
    {
    }
    protected virtual void UnsubscribeFromMenuSceneEvents()
    {
    }
    protected virtual void UnsubscribeFromLevelSelectionEvents()
    {
    }
    protected virtual void UnsubscribeFromMainSceneEvents()
    {
    }
    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoadedHandler;
        SceneManager.sceneUnloaded -= SceneUnloadedHandler;
    }
}
