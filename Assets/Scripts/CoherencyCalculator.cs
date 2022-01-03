using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoherencyCalculator
{
    static Dictionary<Collider2D, List<Collider2D>> overlaps;
    static HashSet<Collider2D> visited;
    public static bool IsCoherent()
    {
        CalculateOverlaps();
        Traverse();
        if (visited.Count == overlaps.Count)
        {
            return true;
        }
        return false;
    }
    private static void Traverse()
    {
        visited = new HashSet<Collider2D>();
        var startGO = BlockManager.Instance.BlockGOs[0];
        var startCollider = startGO.GetComponent<Collider2D>();
        Step(startCollider);
    }
    private static void Step(Collider2D col)
    {
        visited.Add(col);
        foreach (var o in overlaps[col])
        {
            if (!visited.Contains(o))
            {
                Step(o);
            }
        }
    }
    private static void CalculateOverlaps()
    {
        var filter = Helpers.GetSingleLayerMaskContactFilter(Constants.blockLayer);
        var blocks = BlockManager.Instance.BlockGOs;
        overlaps = new Dictionary<Collider2D, List<Collider2D>>();
        foreach (var b in blocks)
        {
            if (b.activeSelf)
            {
                var collidingColliders = new List<Collider2D>();
                var blockCollider = b.GetComponent<Collider2D>();
                Physics2D.OverlapCollider(blockCollider, filter, collidingColliders);
                overlaps.Add(blockCollider, collidingColliders);
            }
        }
    }
    private static void LogOverlaps()
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
