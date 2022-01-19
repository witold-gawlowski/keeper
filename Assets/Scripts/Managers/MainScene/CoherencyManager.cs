using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentIndex = BlockScript;
public class CoherencyManager: Singleton<CoherencyManager>
{
    public bool IsCoherent { get { return visited.Count == overlaps.Count; } }
    public int ComponentCount { get; private set; }
    public Dictionary<BlockScript, ComponentIndex> Components { get; private set; }

    private Dictionary<BlockScript, List<BlockScript>> overlaps;
    private HashSet<BlockScript> visited;

    public void CalculateComponents()
    {
        Traverse();
    }
    public void CalculateNeighborhood()
    {
        var filter = Helpers.GetSingleLayerMaskContactFilter(Constants.blockLayer);
        var blocks = BlockManager.Instance.Blocks;
        overlaps = new Dictionary<BlockScript, List<BlockScript>>();
        foreach (var b in blocks)
        {
            if (b.gameObject.activeSelf)
            {
                var collidingColliders = new List<Collider2D>();
                var blockCollider = b.GetComponent<Collider2D>();
                Physics2D.OverlapCollider(blockCollider, filter, collidingColliders);
                var collidingScripts = new List<BlockScript>();
                foreach(var c in collidingColliders)
                {
                    var cScript = c.GetComponent<BlockScript>();
                    collidingScripts.Add(cScript);   
                }
                overlaps.Add(b, collidingScripts);
            }
        }
    }
    private void Traverse()
    {
        ComponentCount = 0;
        Components = new Dictionary<BlockScript, BlockScript>();
        visited = new HashSet<BlockScript>();
        foreach(var bs in overlaps.Keys)
        {
            if (!visited.Contains(bs))
            {
                Step(bs, bs);
                ComponentCount++;
            }
        }
    }
    private void Step(BlockScript bs, ComponentIndex componentIndex)
    {
        visited.Add(bs);
        Components.Add(bs, componentIndex);
        foreach (var o in overlaps[bs])
        {
            if (!visited.Contains(o))
            {
                Step(o, componentIndex);
            }
        }
    }  
    private void LogOverlaps()
    {
        foreach (var e in overlaps)
        {
            var s = "";
            foreach (BlockScript ee in e.Value)
            {
                s += ee.name;
            }
            Debug.Log(s);
        }
    }
}
