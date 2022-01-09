using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;

    public List<GameObject> BlockGOs { get; private set; }
    private Dictionary<GameObject, BlockSO> blockTypes;
    bool isFreezed;
    public void Init()
    {
        GenerateBlockPool();
        SetFreezeBlocks(false);
    }
    private void OnEnable()
    {
        InputManager.Instance.pointerReleasedAfterHoldEvent += ShortTouchFinishedHandler;
        MainSceneUIManager.Instance.blockSelectedForDeletionEvent += Despawn;
        MainSceneManager.Instance.verdictStartedEvent += HandleVerdictStardedEvent;
    }
    public Dictionary<BlockSO, int> GetUsedBlocks()
    {
        var result = new Dictionary<BlockSO, int>();
        foreach(var blockGO in BlockGOs)
        {
            if (blockGO.activeSelf)
            {
                var blockType = blockTypes[blockGO];
                if (!result.ContainsKey(blockType))
                {
                    result.Add(blockType, 0);
                }
                result[blockType]++;
            }
        }
        return result;
    }
    void HandleVerdictStardedEvent()
    {
        SetFreezeBlocks(true);
    }
    void ShortTouchFinishedHandler(Vector2 worldPos)
    {
        var blockMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
        var hit = Physics2D.OverlapPoint(worldPos, blockMask);
        if (!isFreezed && hit == null)
        {
            Spawn(worldPos);
        }
    }
    void SetFreezeBlocks(bool value)
    {
        isFreezed = value;
    }
    void GenerateBlockPool()
    {
        blockTypes = new Dictionary<GameObject, BlockSO>();
        var inventory = InventoryManager.Instance.GetInventory();
        BlockGOs = new List<GameObject>();
        foreach (var item in inventory)
        {
            for (int i = 0; i < item.Value; i++)
            {
                var block = Instantiate(item.Key.prefab, transform);
                blockTypes.Add(block, item.Key);
                block.SetActive(false);
                BlockGOs.Add(block);
            }
        }
    }
    void Spawn(Vector2 position)
    {
        foreach (var go in BlockGOs)
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                go.transform.position = position;
                if (blockSpawnedEvent != null)
                {
                    blockSpawnedEvent(go);
                }
                return;
            }
        }
    }
    void Despawn(GameObject block)
    {
        block.gameObject.SetActive(false);
        SetBlockAsLast(block);
    }
    void SetBlockAsLast(GameObject block)
    {
        BlockGOs.Remove(block);
        BlockGOs.Add(block);
    }
}
