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
    public float LastManipulationFinishTime { get; private set; }
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
        DragManager.Instance.newRotationPositionEvent += HandleNewRotationPosition;
        DragManager.Instance.turnFinishedEvent += HandleTurnFinished;
        this.levelCompletedEvent += HandleLevelCompleted;
        MainSceneUIManager.Instance.surrenderPressedEvent += test;
    }
    void test()
    {
        levelFailedEvent();
    }
    void HandleVerdictFinished(int hits)
    {
        InventoryManager.Instance.RemoveDiggers(hits);
        var diggerCount = InventoryManager.Instance.DiggerCount;
        if(diggerCount >= 0)
        {
            levelCompletedEvent?.Invoke();
            return;
        }
        levelFailedEvent?.Invoke();
    }
    //TODO: move theese calls to coresponding managers
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
    void HandleNewRotationPosition()
    {
        OnColliderPositionUpdate();
    }
    void HandleBlockDragContinued()
    {
        OnColliderPositionUpdate();
    }
    void HandleBlockSpawned(GameObject block)
    {
        StartCoroutine(HandleBlockSpawnedCoroutine(block));
    }
    IEnumerator HandleBlockSpawnedCoroutine(GameObject block)
    {
        LastManipulationFinishTime = Time.time;
        LastBlockTouched = block;
        yield return null;
        OnColliderPositionUpdate();
        StartCoroutine(CheckForLevelCompletion());
    }

    void HandleDragFinished(GameObject block)
    {
        LastManipulationFinishTime = Time.time;
        LastBlockTouched = block;
        StartCoroutine(CheckForLevelCompletion());
    }
    void HandleTurnFinished(GameObject _)
    {
        LastManipulationFinishTime = Time.time;
        StartCoroutine(CheckForLevelCompletion());
    }
    void OnColliderPositionUpdate()
    {
        Physics2D.SyncTransforms();
        CoherencyManager.Instance.CalculateNeighborhood();
        CoherencyManager.Instance.CalculateComponents();
        var componentCount = CoherencyManager.Instance.ComponentCount;
        MainSceneUIManager.Instance.SetComponentCount(componentCount);
        BlockColorManager.Instance.RepaintBlocks();
    }
    private IEnumerator CheckForLevelCompletion()
    {
        float targetCompletionFraction = mapData.completionFraction;
        yield return FillManager.Instance.CalculateCoveredAreaFraction();
        bool isAreaCovered = FillManager.Instance.AreaFractionCovered >= targetCompletionFraction;
        bool isSingleComponent = CoherencyManager.Instance.ComponentCount == 1;
        if (isAreaCovered && isSingleComponent)
        {
            if (CoherencyManager.Instance.IsCoherent)
            {
                verdictConditionsMetEvent();
            }
        }
    }
}
