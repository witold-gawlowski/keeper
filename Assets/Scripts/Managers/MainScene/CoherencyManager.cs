using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InstanceID = System.Int32;
public class CoherencyManager: Singleton<CoherencyManager>
{
    public bool IsCoherent { get { return visited.Count == overlaps.Count; } }
    public Dictionary<InstanceID, InstanceID> Components { get; private set; }

    private Dictionary<Collider2D, List<Collider2D>> overlaps;
    private HashSet<Collider2D> visited;

    public void CalculateComponents()
    {
        Traverse();
    }
    public void CalculateNeighborhood()
    {
        var filter = Helpers.GetSingleLayerMaskContactFilter(Constants.blockLayer);
        var blocks = BlockManager.Instance.Blocks;
        overlaps = new Dictionary<Collider2D, List<Collider2D>>();
        foreach (var b in blocks)
        {
            if (b.gameObject.activeSelf)
            {
                var collidingColliders = new List<Collider2D>();
                var blockCollider = b.GetComponent<Collider2D>();
                Physics2D.OverlapCollider(blockCollider, filter, collidingColliders);
                overlaps.Add(blockCollider, collidingColliders);
            }
        }
    }
    private void Traverse()
    {
        Components = new Dictionary<InstanceID, InstanceID>();
        visited = new HashSet<Collider2D>();
        foreach(var c in overlaps.Keys)
        {
            if (!visited.Contains(c))
            {
                Step(c, c.GetInstanceID());
            }
        }
    }
    private void Step(Collider2D col, int componentIndex)
    {
        visited.Add(col);
        Components.Add(col.GetInstanceID(), componentIndex);
        foreach (var o in overlaps[col])
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
            foreach (Collider2D ee in e.Value)
            {
                s += ee.name;
            }
            Debug.Log(s);
        }
    }
}
