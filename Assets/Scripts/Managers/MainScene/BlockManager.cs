using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;
    public List<BlockScript> BlockScripts { get; private set; }
    public BlockScript LastBlockSpawned { get; private set; }

    [SerializeField] private float spawnTapMaxDuration = 0.1f;
    [SerializeField] private float spawnTapMaxLength = 0.1f;
    private Dictionary<BlockScript, BlockSO> blockTypes;
    private bool isFreezed;
    public void Start()
    {
        GenerateBlockPool();
        SetFreezeBlocks(false);
        BlockSupplyManager.Instance.Init();
        BlockColorManager.Instance.GenerateBlockColorMap();
    }
    private void OnEnable()
    {
        //InputManager.Instance.pointerPressedEvent += HandlePointerPressedEvent;
        InputManager.Instance.pointerReleasedEvent += HandlerPointerReleasedEvent;
        MainSceneUIManager.Instance.blockSelectedForDeletionEvent += Despawn;
        MainSceneManager.Instance.verdictStartedEvent += HandleVerdictStartedEvent;
    }
    public Dictionary<BlockSO, int> GetUsedBlocks()
    {
        var result = new Dictionary<BlockSO, int>();
        foreach (var bs in BlockScripts)
        {
            if (bs.gameObject.activeSelf)
            {
                var blockType = blockTypes[bs];
                if (!result.ContainsKey(blockType))
                {
                    result.Add(blockType, 0);
                }
                result[blockType]++;
            }
        }
        return result;
    }
    void HandleVerdictStartedEvent()
    {
        SetFreezeBlocks(true);
    }
    void HandlePointerPressedEvent(Vector2 worldPos)
    {
        var spawnConditionsMet = CheckSpawnConditionsOnPointerPressed(worldPos);
        if (spawnConditionsMet)
        {
            Spawn(worldPos);
        }
    }
    void HandlerPointerReleasedEvent(Vector2 initialWorldPosition, Vector2 worldPos, float tapTimeSpan)
    {
        var spawnConditionsMet = CheckSpawnConditionsOnPointerReleased(initialWorldPosition, worldPos, tapTimeSpan);
        if (spawnConditionsMet)
        {
            Spawn(worldPos);
        }
    }
    bool CheckSpawnConditionsOnPointerPressed(Vector2 worldPos)
    {
        var blockMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
        var hit = Physics2D.OverlapPoint(worldPos, blockMask);
        if (!isFreezed && hit == null)
        {
            return true;
        }
        return false;
    }
    bool CheckSpawnConditionsOnPointerReleased(Vector2 initialWorldPosition, Vector2 worldPos, float tapTimeSpan)
    {
        var drag = initialWorldPosition - worldPos;
        if (drag.magnitude < spawnTapMaxLength && tapTimeSpan < spawnTapMaxDuration)
        {
            var blockMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
            var hit = Physics2D.OverlapPoint(worldPos, blockMask);
            if (!isFreezed && hit == null)
            {
                return true;
            }
        }
        return false;
    }

    void SetFreezeBlocks(bool value)
    {
        isFreezed = value;
    }
    void GenerateBlockPool()
    {
        blockTypes = new Dictionary<BlockScript, BlockSO>();
        var inventory = InventoryManager.Instance.BlockCounts;
        BlockScripts = new List<BlockScript>();
        foreach (var item in inventory)
        {
            for (int i = 0; i < item.Value; i++)
            {
                var block = Instantiate(item.Key.prefab, transform);
                var script = block.GetComponent<BlockScript>();
                blockTypes.Add(script, item.Key);
                block.SetActive(false);
                BlockScripts.Add(script);
            }
        }
    }

    void Spawn(Vector2 position)
    {
        var selectedToSpawn = BlockSupplyManager.Instance.SelectedBlockToSpawn;
        foreach (var bs in BlockScripts)
        {
            var go = bs.gameObject;
            if (!go.activeSelf)
            {
                var blockType = blockTypes[bs];
                if (blockType == selectedToSpawn)
                {
                    go.SetActive(true);
                    bs.transform.position = position;
                    BlockSupplyManager.Instance.ConsumeSelected();
                    if (blockSpawnedEvent != null)
                    {
                        blockSpawnedEvent(go);
                        LastBlockSpawned?.Finalize();
                        var lastBlockSpawnedScript = go.GetComponent<BlockScript>();
                        LastBlockSpawned = lastBlockSpawnedScript;
                    }
                    return;
                }
            }
        }
    }
    void Despawn(BlockScript block)
    {
        block.gameObject.SetActive(false);
        var blockType = blockTypes[block];
        BlockSupplyManager.Instance.RecoverBlock(blockType);
    }

}
