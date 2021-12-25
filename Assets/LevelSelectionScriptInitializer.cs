using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScriptInitializer : MonoBehaviour
{
    void Start()
    {
        LevelScheduler.Instance.CreateCurrentLevelData();
        MapManager.Instance.Init();
        LevelSelectionUIManager.Instance.Init();
    }
}
