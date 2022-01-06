using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoaderScript : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(Constants.ManagerSceneName, LoadSceneMode.Additive);
        SceneManager.LoadScene(Constants.MenuSceneName);
    }
}
