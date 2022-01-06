using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectionScriptInitializer : MonoBehaviour
{
    void Start()
    {
        MapManager.Instance.Init();
        MapSelectionUIManager.Instance.Init();
    }
}
