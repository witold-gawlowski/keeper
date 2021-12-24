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
        MapManager.Instance.selectedMapUpdatedEvent += HandleSelectedLevelUpdatedEvent;
    }
    private void OnDisable()
    {
        if (MapManager.Instance)
        {
            MapManager.Instance.selectedMapUpdatedEvent -= HandleSelectedLevelUpdatedEvent;
        }
    }
    void HandleSelectedLevelUpdatedEvent(int currentLevel)
    {
        if(currentLevel == 0)
        {
            previousLevelButton.interactable = false;
            nextLevelButton.interactable = true;
        }
        else if(currentLevel == MapManager.Instance.MaxMaps - 1)
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
