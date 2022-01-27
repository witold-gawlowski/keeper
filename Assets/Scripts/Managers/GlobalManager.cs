using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager<T> : Singleton<T> where T: MonoBehaviour
{
    private Dictionary<string, System.Action> sceneLoadedHandlers;
    private Dictionary<string, System.Action> sceneUnloadedHandlers;

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
            { Constants.MenuSceneName, SubscribeToMenuSceneEvents },
            { Constants.InventorySceneName, SubscribeToInventorySceneEvents },
            { Constants.MapSelectionSceneName,  SubscribeToMapSelectionEvents },
            { Constants.MainSceneName, SubscribeToMainSceneEvents },
        };
        sceneUnloadedHandlers = new Dictionary<string, System.Action>()
        {
            { Constants.MenuSceneName, UnsubscribeFromMenuSceneEvents },
            { Constants.InventorySceneName, UnsubscribeFromInventorySceneEvents },
            { Constants.MapSelectionSceneName,  UnsubscribeFromLevelSelectionEvents },
            { Constants.MainSceneName,  UnsubscribeFromMainSceneEvents },
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
    protected virtual void SubscribeToInventorySceneEvents()
    {
    }
    protected virtual void SubscribeToMapSelectionEvents()
    {
    }
    protected virtual void SubscribeToMainSceneEvents()
    {
    }
    protected virtual void UnsubscribeFromMenuSceneEvents()
    {
    }
    protected virtual void UnsubscribeFromInventorySceneEvents()
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
