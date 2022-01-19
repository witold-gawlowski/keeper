using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ComponentIndex = BlockScript;
public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;
    public List<BlockScript> BlockScripts { get; private set; }

    [SerializeField] private float spawnTapMaxDuration = 0.1f;
    [SerializeField] private float spawnTapMaxLength = 0.1f;
    [SerializeField] private List<ColorSO> colors;
    private Dictionary<ComponentIndex, ColorSO> componentColors;
    private Dictionary<BlockScript, BlockSO> blockTypes;
    private Dictionary<BlockScript, ColorSO> blockColorMap;
    private bool isFreezed;
    public void Init()
    {
        GenerateBlockPool();
        SetFreezeBlocks(false);
        GenerateBlockColorMap();
    }
    private void OnEnable()
    {
        InputManager.Instance.pointerReleased += HandlerPointerReleasedEvent;
        MainSceneUIManager.Instance.blockSelectedForDeletionEvent += Despawn;
        MainSceneManager.Instance.verdictStartedEvent += HandleVerdictStartedEvent;
    }
    public Dictionary<BlockSO, int> GetUsedBlocks()
    {
        var result = new Dictionary<BlockSO, int>();
        foreach(var bs in BlockScripts)
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
    public void RepaintBlocks()
    {
        var components = CoherencyManager.Instance.Components;
        foreach(var b in BlockScripts)
        {
            if (b.gameObject.activeSelf)
            {
                var component = components[b];
                var color = blockColorMap[component];
                b.SetColor(color.value);
            }
        }
    }
    void HandleVerdictStartedEvent()
    {
        SetFreezeBlocks(true);
    }
    void HandlerPointerReleasedEvent(Vector2 initialWorldosition, Vector2 worldPos, float tapTimeSpan)
    {
        var drag = initialWorldosition - worldPos;
        if (drag.magnitude < spawnTapMaxLength && tapTimeSpan < spawnTapMaxDuration)
        {
            var blockMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
            var hit = Physics2D.OverlapPoint(worldPos, blockMask);
            if (!isFreezed && hit == null)
            {
                Spawn(worldPos);
            }
        }
    }
    void SetFreezeBlocks(bool value)
    {
        isFreezed = value;
    }
    void GenerateBlockPool()
    {
        blockTypes = new Dictionary<BlockScript, BlockSO>();
        var inventory = InventoryManager.Instance.GetBlocks();
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
        foreach (var bs in BlockScripts)
        {
            var go = bs.gameObject;
            if (!go.activeSelf)
            {
                go.SetActive(true);
                bs.transform.position = position;
                if (blockSpawnedEvent != null)
                {
                    blockSpawnedEvent(go);
                }
                return;
            }
        }
    }
    void Despawn(BlockScript block)
    {
        block.gameObject.SetActive(false);
        SetBlockAsLast(block);
    }
    void SetBlockAsLast(BlockScript block)
    {
        BlockScripts.Remove(block);
        BlockScripts.Add(block);
    }
    void GenerateBlockColorMap()
    {
        blockColorMap = new Dictionary<ComponentIndex, ColorSO>();
        IEnumerator<ColorSO> currentColor = colors.GetEnumerator();
        foreach (var bs in BlockScripts)
        {
            var hasElem = currentColor.MoveNext();
            if (!hasElem)
            {
                currentColor.Reset();
                currentColor.MoveNext();
            }
            blockColorMap.Add(bs, currentColor.Current);
        }
    }
}
