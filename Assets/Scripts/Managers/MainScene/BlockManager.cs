using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;
    public List<BlockScript> BlockScripts { get; private set; }
    public GameObject LastBlockSpawned { get; private set; }

    [SerializeField] private AnimationCurve probeFlashTransparencyCurve;
    [SerializeField] private float spawnTapMaxDuration = 0.1f;
    [SerializeField] private float spawnTapMaxLength = 0.1f;
    [SerializeField] private PolygonCollider2D spawiningProbe;
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
            StartCoroutine(TrySpawn(worldPos));
        }
    }
    void HandlerPointerReleasedEvent(Vector2 initialWorldPosition, Vector2 worldPos, float tapTimeSpan)
    {
        var spawnConditionsMet = CheckSpawnConditionsOnPointerReleased(initialWorldPosition, worldPos, tapTimeSpan);
        if (spawnConditionsMet)
        {
            StartCoroutine(TrySpawn(worldPos));
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

    IEnumerator TrySpawn(Vector2 position)
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
                    Helpers.ReplicateColliderToProbe(bs, spawiningProbe);
                    spawiningProbe.transform.position = position;
                    spawiningProbe.transform.rotation = bs.transform.rotation;
                    yield return new WaitForFixedUpdate();
                    var filter = Helpers.GetSingleLayerMaskContactFilter(Constants.blockLayer);
                    var colliders = new List<Collider2D>();
                    Physics2D.OverlapCollider(spawiningProbe, filter, colliders);
                    if (colliders.Count > 0)
                    {
                        StartCoroutine(FlashProbe(bs));
                        yield break;
                    }
                    Spawn(bs, position);
                    yield break;
                }
            }
        }
    }
    private void Spawn(BlockScript bs, Vector2 position)
    {
        var go = bs.gameObject;
        go.SetActive(true);
        bs.transform.position = position;

        BlockSupplyManager.Instance.ConsumeSelected();
        if (blockSpawnedEvent != null)
        {
            blockSpawnedEvent(go);
        }
        if (LastBlockSpawned != null)
        {
            Finalize(LastBlockSpawned);
        }
        LastBlockSpawned = go;
    }
    IEnumerator FlashProbe(BlockScript bs)
    {
        var probeSr = spawiningProbe.GetComponent<SpriteRenderer>();
        probeSr.sprite = bs.GetSprite();
        var startTime = Time.time;
        var animFrameCount = probeFlashTransparencyCurve.length;
        var animationLength = probeFlashTransparencyCurve[animFrameCount - 1].time;
        var newColor = probeSr.color;
        while (Time.time - startTime < animationLength)
        {
            newColor.a = probeFlashTransparencyCurve.Evaluate(Time.time - startTime);
            probeSr.color = newColor;
            yield return null;
        }
    }
    void Finalize(GameObject block)
    {
        var lastBlockSpawnedScript = block.GetComponent<BlockScript>();
        lastBlockSpawnedScript.Finalize();
    }
    void Despawn(BlockScript block)
    {
        block.gameObject.SetActive(false);
        var blockType = blockTypes[block];
        //BlockSupplyManager.Instance.RecoverBlock(blockType);
    }

}
