using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(GameManager.Instance.SelectedLevel.prefab, transform);
    }
}
