using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BlockColliderShrinkerScript : MonoBehaviour
{
    public BlockSO blockToShrink;
    public void Shrink(BlockSO b)
    {
        var collider = b.PrefabBlockScript.GetComponent<PolygonCollider2D>();
        
        var newPoints = new Vector2[100];
    }
    private List<Vector2> ShrinkPath(int pathIndex, PolygonCollider2D collider)
    {
        var result = new List<Vector2>();
        var pathPoints = collider.GetPath(pathIndex);
        var len = pathPoints.Length;
        for(int i=0; i<len; i++)
        {
            var c = pathPoints[i]; //current
            var p = pathPoints[(i + len - 1) % len]; //previous
            var n = pathPoints[(i + 1) % len]; //next
            var pNorm = (p - c).normalized;
            var nNorm = (p - n).normalized;
            var candidateDisplacement = (pNorm + nNorm) * 0.1f;
            var newCornerCandidate1 = c + candidateDisplacement;
            var newCornerCandidate2 = c - candidateDisplacement;
            bool cand1Inside = IsPointInsideCollider(collider, newCornerCandidate1);
            var candInside = cand1Inside ? newCornerCandidate1 : newCornerCandidate2;
            result.Add(candInside);
        }
        return result;
    }
    private bool IsPointInsideCollider(Collider2D col, Vector2 point)
    {
        if (col.ClosestPoint(point) == point)
        {
            return true;
        }
        return false;
    }
}
