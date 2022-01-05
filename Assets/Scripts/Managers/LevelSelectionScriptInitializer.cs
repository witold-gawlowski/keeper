using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScriptInitializer : MonoBehaviour
{
    void Start()
    {
        MapManager.Instance.Init();
        LevelSelectionUIManager.Instance.Init();
    }
}
