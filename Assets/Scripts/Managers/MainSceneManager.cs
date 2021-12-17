using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    public System.Action verdictStartedEvent;
    public GameObject LevelObject { get; private set; }
    private void Awake()
    {
        LevelObject = Instantiate(GameManager.Instance.SelectedLevel.prefab, transform);
    }
    private void OnEnable()
    {
        MainSceneUIManager.Instance.verdictPressedEvent += HandleVerdictPressedEvent;
    }
    void HandleVerdictPressedEvent()
    {
        verdictStartedEvent();
    }
    void OnDisable()
    {
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.verdictPressedEvent -= HandleVerdictPressedEvent;
        }
    }
}
