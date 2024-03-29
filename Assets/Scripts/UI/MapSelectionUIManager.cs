using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapSelectionUIManager : Singleton<MapSelectionUIManager>
{
    public System.Action PreviousLevelButtonPressedEvent;
    public System.Action NextLevelButtonPressedEvent;
    public System.Action StartButtonPressedEvent;
    public System.Action BackButtonPressedEvent;

    [SerializeField] Button previousMapButton;
    [SerializeField] Button nextMapButton;
    [SerializeField] TMP_Text levelText;
    [SerializeField] LootItemsUIScript rewardUIScript;
    [SerializeField] private TMP_Text areaToComplete;
    [SerializeField] Button startButton;
    [SerializeField] TMP_Text mapIndexText;
    private void OnEnable()
    {
        MapManager.Instance.selectedMapChangedEvent += HandleSelectedMapChangedEvent;
    }
  
    public void Init()
    {
        levelText.text = "Level " + GameStateManager.Instance.Level;
        HandleSelectedMapChangedEvent(0);
    }
    void HandleSelectedMapChangedEvent(int currentMap)
    {
        UpdateSelectionButtonsInteractivity(currentMap);
        UpdateRewards(currentMap);
        UpdatePercetnageToComplete(currentMap);
        UpdateStartButtonInteractibility(currentMap);
        UpdateIndexText(currentMap);
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
    void UpdatePercetnageToComplete(int currentMapIndex)
    {
        var levelData = LevelScheduler.Instance.CurrentLevelData;
        var mapData = levelData[currentMapIndex];
        var fraction = mapData.completionFraction;
        areaToComplete.text = Mathf.RoundToInt(fraction * 100).ToString() + "%";
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
    void UpdateIndexText(int currentMapIndex)
    {
        mapIndexText.text = (currentMapIndex+1) + "/" + LevelScheduler.Instance.CurrentLevelData.Count;
    }
    void UpdateStartButtonInteractibility(int currentMapIndex)
    {
        var levelData = LevelScheduler.Instance.CurrentLevelData;
        var selectedMap = levelData[currentMapIndex];
        if (selectedMap.Surrendered)
        {
            startButton.interactable = false;
        }
        else
        {
            startButton.interactable = true;
        }
    }
    void UpdateRewards(int currentMapIndex)
    {
        rewardUIScript.Clear();
        var levelData = LevelScheduler.Instance.CurrentLevelData;
        var selectedMap = levelData[currentMapIndex];
        foreach (var r in selectedMap.reward)
        {
            if (r is BlockRewardItem)
            {
                var blockReward = r as BlockRewardItem;
                rewardUIScript.AddBlockItem(blockReward.Sprite, blockReward.count);
            }
            else if(r is DiggerRewardItem)
            {
                var diggerReward = r as DiggerRewardItem;
                rewardUIScript.AddDiggerItem(diggerReward.count);
            }
            else if(r is ComponentOrbRewardItem)
            {
                var componentOrbReward = r as ComponentOrbRewardItem;
                rewardUIScript.AddComponentOrbCountItem(componentOrbReward.count);
            }
        }
    }
}
