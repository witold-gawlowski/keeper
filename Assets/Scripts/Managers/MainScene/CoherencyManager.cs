using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoherencyManager: Singleton<CoherencyManager>
{
    public bool IsCoherent { get { return visited.Count == overlaps.Count; } }

    Dictionary<Collider2D, List<Collider2D>> overlaps;
    HashSet<Collider2D> visited;
    List<Collider2D> highlightedBlocks;

    public void CalculateCoherency()
    {
        if(highlightedBlocks == null || highlightedBlocks.Count == 0)
        {
            var lastBlockTouched = MainSceneManager.Instance.LastBlockTouched;
            Collider2D collider = lastBlockTouched.GetComponent<Collider2D>();
            highlightedBlocks = new List<Collider2D>() { collider };
        }
        CalculateOverlaps();
        Traverse();
    }
    private void Traverse()
    {
        visited = new HashSet<Collider2D>();
        Collider2D startCollider = highlightedBlocks[0];
        Step(startCollider);
    }
    private void Step(Collider2D col)
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
    private void CalculateOverlaps()
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
