using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    public GameObject LevelObject { get; private set; }
    private void Awake()
    {
        LevelObject = Instantiate(GameManager.Instance.SelectedLevel.prefab, transform);
    }
}
