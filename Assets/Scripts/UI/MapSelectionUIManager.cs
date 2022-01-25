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
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] LootItemsUIScript rewardUIScript;
    [SerializeField] private LootItemsUIScript inventoryItemsUIScript;
    [SerializeField] private TMP_Text areaToComplete;
    private void OnEnable()
    {
        MapManager.Instance.selectedMapChangedEvent += HandleSelectedMapChangedEvent;
    }
  
    public void Init()
    {
        levelText.text = "Level " + GameStateManager.Instance.Level;
        HandleSelectedMapChangedEvent(0);
        UpdateInventory();
    }
    public void HandleInventoryTogglePress()
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
        }
    }
    void HandleSelectedMapChangedEvent(int currentMap)
    {
        UpdateSelectionButtonsInteractivity(currentMap);
        UpdateRewards(currentMap);
        UpdatePercetnageToComplete(currentMap);
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
        var fraction = mapData.map.targetCompletionFraction;
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
    void UpdateRewards(int currentMapIndex)
    {
        rewardUIScript.Clear();
        var levelData = LevelScheduler.Instance.CurrentLevelData;
        var selectedMapRewards = levelData[currentMapIndex];
        foreach (var r in selectedMapRewards.reward)
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
        }
    }
    private void UpdateInventory()
    {
        inventoryItemsUIScript.Clear();
        var inventory = InventoryManager.Instance;
        foreach (var i in inventory.GetBlocks())
        {
            if (i.Value > 0)
            {
                var blockPrefab = i.Key.PrefabBlockScript;
                var sprite = blockPrefab.GetSprite();
                inventoryItemsUIScript.AddBlockItem(sprite, i.Value);
            }
        }
        var diggerCount = inventory.DiggerCount;
        inventoryItemsUIScript.AddDiggerItem(diggerCount);
    }

}
