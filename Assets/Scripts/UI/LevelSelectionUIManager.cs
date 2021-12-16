using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUIManager : Singleton<LevelSelectionUIManager>
{
    public System.Action PreviousLevelButtonPressedEvent;
    public System.Action NextLevelButtonPressedEvent;
    public System.Action StartButtonPressedEvent;
    public System.Action BackButtonPressedEvent;

    [SerializeField] Button previousLevelButton;
    [SerializeField] Button nextLevelButton;
    private void OnEnable()
    {
        LevelManager.Instance.selectedLevelUpdatedEvent += HandleSelectedLevelUpdatedEvent;
    }
    private void OnDisable()
    {
        if (LevelManager.Instance)
        {
            LevelManager.Instance.selectedLevelUpdatedEvent -= HandleSelectedLevelUpdatedEvent;
        }
    }
    void HandleSelectedLevelUpdatedEvent(int currentLevel)
    {
        if(currentLevel == 0)
        {
            previousLevelButton.interactable = false;
            nextLevelButton.interactable = true;
        }
        else if(currentLevel == LevelManager.Instance.MaxLevels - 1)
        {
            previousLevelButton.interactable = true;
            nextLevelButton.interactable = false;
        }
        else
        {
            previousLevelButton.interactable = true;
            nextLevelButton.interactable = true;
        }
    }
    public void HandlePreviousLevelButtonPress()
    {
        PreviousLevelButtonPressedEvent?.Invoke();
    }
    public void HandleNextLevelButtonPress()
    {
        NextLevelButtonPressedEvent?.Invoke();
    }
    public void HandleStartPress()
    {
        StartButtonPressedEvent();
    }
    public void HandleBackPress()
    {
        BackButtonPressedEvent();
    }
}
