using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoherencyManager: Singleton<CoherencyManager>
{
    public bool IsCoherent { get { return visited.Count == overlaps.Count; } }
    
    private Dictionary<Collider2D, List<Collider2D>> overlaps;
    private HashSet<Collider2D> visited;
    private Collider2D startCol;
    public void CalculateCoherency()
    {
        if(startCol == null || !startCol.gameObject.activeSelf)
        {
            var lastBlockTouched = MainSceneManager.Instance.LastBlockTouched;
            startCol = lastBlockTouched.GetComponent<Collider2D>();
        }
        CalculateOverlaps();
        Traverse();
    }
    private void Traverse()
    {
        visited = new HashSet<Collider2D>();
        Step(startCol);
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
