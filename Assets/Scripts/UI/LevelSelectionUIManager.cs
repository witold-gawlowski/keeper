using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionUIManager : Singleton<LevelSelectionUIManager>
{
    public System.Action PreviousLevelButtonPressedEvent;
    public System.Action NextLevelButtonPressedEvent;
    public System.Action StartButtonPressedEvent;
    public System.Action BackButtonPressedEvent;

    [SerializeField] GameObject rewardIconPrefab;
    [SerializeField] Button previousMapButton;
    [SerializeField] Button nextMapButton;
    [SerializeField] TMP_Text levelText;
    [SerializeField] Transform rewardImageParent;
    [SerializeField] List<InventoryItemUIScript> rewardIcons;
    private void OnEnable()
    {
        MapManager.Instance.selectedMapUpdatedEvent += HandleSelectedMapUpdatedEvent;
    }
    private void OnDisable()
    {
        if (MapManager.Instance)
        {
            MapManager.Instance.selectedMapUpdatedEvent -= HandleSelectedMapUpdatedEvent;
        }
    }
  
    public void Init()
    {
        levelText.text = "Level " + GameStateManager.Instance.Level;
        HandleSelectedMapUpdatedEvent(0);
    }
    void HandleSelectedMapUpdatedEvent(int currentMap)
    {
        UpdateSelectionButtonsInteractivity(currentMap);
        UpdateRewards(currentMap);
    }
    void UpdateSelectionButtonsInteractivity(int currentMap)
    {
        if (currentMap == 0)
        {
            previousMapButton.interactable = false;
            nextMapButton.interactable = true;
        }
        else if (currentMap == MapManager.Instance.MaxMaps - 1)
        {
            previousMapButton.interactable = true;
            nextMapButton.interactable = false;
        }
        else
        {
            previousMapButton.interactable = true;
            nextMapButton.interactable = true;
        }
    }
    void HandlePreviousLevelButtonPress()
    {
        PreviousLevelButtonPressedEvent?.Invoke();
    }
    void HandleNextLevelButtonPress()
    {
        NextLevelButtonPressedEvent?.Invoke();
    }
    void HandleStartPress()
    {
        StartButtonPressedEvent();
    }
    void HandleBackPress()
    {
        BackButtonPressedEvent();
    }
    void UpdateRewards(int currentMapIndex)
    {
        foreach(Transform icon in rewardImageParent)
        {
            if (icon != rewardImageParent)
            {
                icon.gameObject.SetActive(false);
            }
        }
        var levelData = LevelScheduler.Instance.CurrentLevelData;
        var selectedMapRewards = levelData[currentMapIndex];
        foreach (var r in selectedMapRewards.reward)
        {
            AddReward(r.Sprite, r.count);
        }
    }
    void AddReward(Sprite s, int count)
    {
        foreach(Transform icon in rewardImageParent)
        {
            if(icon != rewardImageParent && icon.gameObject.activeSelf == false)
            {
                icon.gameObject.SetActive(true);
                var rewardIconScript = icon.GetComponent<InventoryItemUIScript>();
                rewardIconScript.SetSprite(s);
                rewardIconScript.SetCount(count);
                return;
            }
        }
    }
}
