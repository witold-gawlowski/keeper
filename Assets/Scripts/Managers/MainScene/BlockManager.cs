using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InstanceID = System.Int32;
public class BlockManager : Singleton<BlockManager>
{
    public System.Action<GameObject> blockSpawnedEvent;
    public List<BlockScript> Blocks { get; private set; }

    [SerializeField] private List<ColorSO> colors;
    [SerializeField] private float spawnTapMaxDuration = 0.1f;

    private Dictionary<InstanceID, Color> blockColors;
    private Dictionary<BlockScript, BlockSO> blockTypes;
    private bool isFreezed;
    public void Init()
    {
        GenerateBlockPool();
        InitBlockColors();
        SetFreezeBlocks(false);
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
        foreach(var bs in Blocks)
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
        foreach(var b in Blocks)
        {
            if (b.gameObject.activeSelf)
            {
                var color = blockColors[components[b.GetInstanceID()]];
                b.SetColor(color);
            }
        }
    }
    void HandleVerdictStartedEvent()
    {
        SetFreezeBlocks(true);
    }
    void HandlerPointerReleasedEvent(Vector2 initialWorldosition, Vector2 worldPos)
    {
        var drag = initialWorldosition - worldPos;
        if (drag.magnitude < spawnTapMaxDuration)
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
    void InitBlockColors()
    {
        blockColors = new Dictionary<InstanceID, Color>();
        int i = 0;
        int n = Blocks.Count;
        foreach(var b in Blocks)
        {
            blockColors.Add(b.GetInstanceID(), colors[i % n].value);
            i++;
        }
    }
    void GenerateBlockPool()
    {
        blockTypes = new Dictionary<BlockScript, BlockSO>();
        var inventory = InventoryManager.Instance.GetInventory();
        Blocks = new List<BlockScript>();
        foreach (var item in inventory)
        {
            for (int i = 0; i < item.Value; i++)
            {
                var block = Instantiate(item.Key.prefab, transform);
                var script = block.GetComponent<BlockScript>();
                blockTypes.Add(script, item.Key);
                block.SetActive(false);
                Blocks.Add(script);
            }
        }
    }
    void Spawn(Vector2 position)
    {
        foreach (var bs in Blocks)
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
        Blocks.Remove(block);
        Blocks.Add(block);
    }
}
