using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;

    private List<GameObject> blocks;
    bool isFreezed;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        isFreezed = false;
    }
    private void Start()
    {
        GenerateBlockPool();
    }
    private void OnEnable()
    {
        GameManager.Instance.mainSceneStartedEvent += HandleMainSceneStartedEvent;
    }
    public void SubscribeToMainSceneEvents()
    {
        InputManager.mouse0DownEventWithDPressed += Mouse0DownWithDPressedEventHandler;
        InputManager.mouse0DownEvent += Mouse0DownEventHandler;
        MainSceneUIManager.Instance.blockSelectedForDeletionEvent += Despawn;
        GameManager.Instance.surrenderEvent += DespawnAll;
        MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent += DespawnAll;
        MainSceneManager.Instance.verdictStartedEvent += HandleVerdictStardedEvent;
    }
    public void UnsubscribeFromMainSceneEvents()
    {
        InputManager.mouse0DownEventWithDPressed -= Mouse0DownWithDPressedEventHandler;
        InputManager.mouse0DownEvent -= Mouse0DownEventHandler;
        if (MainSceneUIManager.Instance)
        {
            MainSceneUIManager.Instance.levelCompletedConfirmPressedEvent -= DespawnAll;
        }
        GameManager.Instance.surrenderEvent -= DespawnAll;
        if (MainSceneManager.Instance)
        {
            MainSceneManager.Instance.verdictStartedEvent -= HandleVerdictStardedEvent;
        }
    }
    void HandleVerdictStardedEvent()
    {
        SetFreezeBlocks(true);
    }
    void HandleMainSceneStartedEvent()
    {
        SetFreezeBlocks(false);
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
        var inventory = InventoryManager.Instance.GetInventory();
        blocks = new List<GameObject>();
        foreach (var item in inventory)
        {
            for (int i = 0; i < item.countInInventory; i++)
            {
                var block = Instantiate(item.prefab, transform);
                block.SetActive(false);
                blocks.Add(block);
            }
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
    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.mainSceneStartedEvent -= HandleMainSceneStartedEvent;
        }
    }
}
