using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInitializationManager : MonoBehaviour
{
    void Start()
    {
        BlockManager.Instance.Init();
    }
}
