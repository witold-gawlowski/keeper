using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    public System.Action startNewGameEvent;
    public System.Action continueGameEvent;
    [SerializeField] Button continueGameButton;
    private void Start()
    {
        if (GameStateManager.Instance.GameInProgress)
        {
            continueGameButton.gameObject.SetActive(true);
        }
        else
        {
            continueGameButton.gameObject.SetActive(false);
        }
    }
    public void OnStartPress()
    {
        startNewGameEvent();
    }
    public void OnContinuePress()
    {
        continueGameEvent();
    }
}
