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
        MainSceneUIManager.Instance.verdictPressedEvent += HandleVerdictPressedEvent;
        MainSceneUIManager.Instance.cheatPressedEvent += HandleCheatPressed;
        VerdictManager.Instance.resultEvent += HandleVerdictFinished;
        DragManager.Instance.dragFinishedEvent += HandleBlockPositionUpdated;
        BlockManager.Instance.blockSpawnedEvent += HandleBlockPositionUpdated;
    }
    void HandleVerdictFinished(int hits)
    {
        var maxHits = mapData.map.maxNumberOfHits;
        if (hits <= maxHits)
        {
            levelCompletedEvent?.Invoke();
            return;
        }
        levelFailedEvent?.Invoke();
    }
    void HandleVerdictPressedEvent()
    {
        verdictStartedEvent();
    }
    void HandleCheatPressed()
    {
        levelCompletedEvent?.Invoke();
    }
    void HandleBlockPositionUpdated(GameObject block)
    {
        StartCoroutine(HandlePositionUpdateCoroutine(block));
    }
    IEnumerator HandlePositionUpdateCoroutine(GameObject block)
    {
        float targetCompletionFraction = mapData.map.targetCompletionFraction;
        LastBlockTouched = block;
        yield return FillManager.Instance.CalculateCoveredAreaFraction();
        if (FillManager.Instance.AreaFractionCovered >= targetCompletionFraction)
        {
            verdictConditionsMetEvent();
        }
    }
}
