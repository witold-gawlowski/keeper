using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoaderScript : MonoBehaviour
{
    [SerializeField] private UnityEditor.SceneAsset mainMenu;
    [SerializeField] private UnityEditor.SceneAsset managerScene;
    private void Start()
    {
        SceneManager.LoadScene(managerScene.name, LoadSceneMode.Additive);
        SceneManager.LoadScene(mainMenu.name);
    }
}
