using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentIndex = BlockScript;

public class BlockColorManager : Singleton<BlockColorManager>
{
    [SerializeField] private List<ColorSO> colors;
    private Dictionary<BlockScript, ColorSO> blockColorMap;
    public void GenerateBlockColorMap()
    {
        blockColorMap = new Dictionary<ComponentIndex, ColorSO>();
        IEnumerator<ColorSO> currentColor = colors.GetEnumerator();
        var blockScripts = BlockManager.Instance.BlockScripts;
        foreach (var bs in blockScripts)
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
    public void RepaintBlocks()
    {
        var components = CoherencyManager.Instance.Components;
        var blockScripts = BlockManager.Instance.BlockScripts;
        foreach (var b in blockScripts)
        {
            if (b.gameObject.activeSelf)
            {
                var component = components[b];
                var color = blockColorMap[component];
                b.SetColor(color.value);
            }
        }
    }

}
