using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;
    private List<GameObject> blocks;
    public void SubscribeToMainSceneEvents()
    {
        InputManager.mouse0DownEventWithDPressed += Mouse0DownWithDPressedEventHandler;
        InputManager.mouse0DownEvent += Mouse0DownEventHandler;
        MainSceneUIManager.Instance.BlockSelectedForDeletionEvent += Despawn;
        GameManager.Instance.SurrenderEvent += DespawnAll;
        MainSceneUIManager.Instance.LevelCompletedConfirmPressedEvent += DespawnAll;
    }
    public void UnsubscribeFromMainSceneEvents()
    {
        InputManager.mouse0DownEventWithDPressed -= Mouse0DownWithDPressedEventHandler;
        InputManager.mouse0DownEvent -= Mouse0DownEventHandler;
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.LevelCompletedConfirmPressedEvent -= DespawnAll;
        }
        GameManager.Instance.SurrenderEvent -= DespawnAll;
        
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        GenerateBlockPool();
    }
    void GenerateBlockPool()
    {
        var inventory = InventoryManager.Instance.GetInventory();
        blocks = new List<GameObject>();
        foreach (var item in inventory)
        {
            for (int i = 0; i < item.count; i++)
            {
                var block = Instantiate(item.prefab, transform);
                block.SetActive(false);
                blocks.Add(block);
            }
        }
    }
    void Mouse0DownWithDPressedEventHandler(Vector2 position, Collider2D block)
    {
        if (block != null)
        {
            Despawn(block.gameObject);
        }
    }
    void Mouse0DownEventHandler(Vector2 position, Collider2D block)
    {
        if (block == null)
        {
            Spawn(position);
        }
    }
    void DespawnAll()
    {
        foreach(var go in blocks)
        {
            Despawn(go);
        }
    }
    void Spawn(Vector2 position)
    {
        foreach (var go in blocks)
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
    }
}
