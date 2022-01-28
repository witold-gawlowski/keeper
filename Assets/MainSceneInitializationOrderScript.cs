using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInitializationOrderScript : MonoBehaviour
{
    private void Start()
    {
        MainSceneInventoryUIScript.Instance.Init();
        BlockSupplyManager.Instance.ListenToInventoryButtonEvents();
    }
}
