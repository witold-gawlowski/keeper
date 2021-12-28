using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;

    private List<GameObject> blockGOs;
    private Dictionary<GameObject, BlockSO> blockTypes;
    bool isFreezed;
    private void Awake()
    {
        GenerateBlockPool();
        SetFreezeBlocks(false);
    }
    private void OnEnable()
    {
        InputManager.mouse0DownEventWithDPressed += Mouse0DownWithDPressedEventHandler;
        InputManager.mouse0DownEvent += Mouse0DownEventHandler;
        MainSceneUIManager.Instance.blockSelectedForDeletionEvent += Despawn;
        MainSceneManager.Instance.verdictStartedEvent += HandleVerdictStardedEvent;
    }
    public Dictionary<BlockSO, int> GetUsedBlocks()
    {
        var result = new Dictionary<BlockSO, int>();
        foreach(var blockGO in blockGOs)
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
    void Mouse0DownWithDPressedEventHandler(Vector2 position, Collider2D block)
    {
        if (!isFreezed && block != null)
        {
            Despawn(block.gameObject);
        }
    }
    void Mouse0DownEventHandler(Vector2 position, Collider2D block)
    {
        if (!isFreezed && block == null)
        {
            Spawn(position);
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
        blockGOs = new List<GameObject>();
        foreach (var item in inventory)
        {
            for (int i = 0; i < item.Value; i++)
            {
                var block = Instantiate(item.Key.prefab, transform);
                blockTypes.Add(block, item.Key);
                block.SetActive(false);
                blockGOs.Add(block);
            }
        }
    }
    void Spawn(Vector2 position)
    {
        foreach (var go in blockGOs)
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
        blockGOs.Remove(block);
        blockGOs.Add(block);
    }
}
