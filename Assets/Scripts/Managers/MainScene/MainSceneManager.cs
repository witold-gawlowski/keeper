using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    public System.Action verdictStartedEvent;
    public System.Action verdictConditionsMetEvent;
    public System.Action levelCompletedEvent;
    public System.Action levelFailedEvent;
    public GameObject LevelObject { get; private set; }
    public GameObject LastBlockTouched { get; private set; }
    private MapData mapData; 
    private void Awake()
    {
        mapData = GameStateManager.Instance.SelectedMapData;
        LevelObject = Instantiate(mapData.map.prefab, transform);
    }
    private void OnEnable()
    {
        MainSceneUIManager.Instance.verdictPressedEvent += HandleVerdictPressed;
        MainSceneUIManager.Instance.cheatPressedEvent += HandleCheatPressed; 
        VerdictManager.Instance.resultEvent += HandleVerdictFinished;
        DragManager.Instance.dragFinishedEvent += HandleDragFinished;
        BlockManager.Instance.blockSpawnedEvent += HandleBlockSpawned;
        DragManager.Instance.dragContinuedEvent += HandleBlockDragContinued;
        this.levelCompletedEvent += HandleLevelCompleted;
    }
    void HandleVerdictFinished(int hits)
    {
        InventoryManager.Instance.RemoveDiggers(hits);
        if(hits >= 0)
        {
            levelCompletedEvent?.Invoke();
            return;
        }
        levelFailedEvent?.Invoke();
    }
    void HandleLevelCompleted()
    {
        GameStateManager.Instance.OnLevelCompleted();
        InventoryManager.Instance.OnLevelCompleted();
    }
    void HandleVerdictPressed()
    {
        verdictStartedEvent();
    }
    void HandleCheatPressed()
    {
        levelCompletedEvent?.Invoke();
    }
    void HandleBlockDragContinued()
    {
        CoherencyManager.Instance.CalculateNeighborhood();
        CoherencyManager.Instance.CalculateComponents();
        BlockManager.Instance.RepaintBlocks();
    }
    void HandleBlockSpawned(GameObject block)
    {
        LastBlockTouched = block;
        CoherencyManager.Instance.CalculateNeighborhood();
        CoherencyManager.Instance.CalculateComponents();
        BlockManager.Instance.RepaintBlocks();
        StartCoroutine(CheckForLevelCompletion());
    }
    void HandleDragFinished(GameObject block)
    {
        LastBlockTouched = block;
        CoherencyManager.Instance.CalculateNeighborhood();
        StartCoroutine(CheckForLevelCompletion());
    }
    private IEnumerator CheckForLevelCompletion()
    {
        float targetCompletionFraction = mapData.map.targetCompletionFraction;
        yield return FillManager.Instance.CalculateCoveredAreaFraction();
        if (FillManager.Instance.AreaFractionCovered >= targetCompletionFraction)
        {
            if (CoherencyManager.Instance.IsCoherent)
            {
                verdictConditionsMetEvent();
            }
        }
    }
}
