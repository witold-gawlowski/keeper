using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BlockColliderShrinkerScript : MonoBehaviour
{
    //unifinished, needs PrefabUtility functions
    public BlockSO blockToShrink;
    public void Shrink()
    {
        var b = blockToShrink;
        var originalCollider = b.PrefabBlockScript.GetCollider();
        var pathCount = originalCollider.pathCount;
        //var newCollider = b.PrefabBlockScript.GetBlockingCollider();
        //for(int i=0; i<pathCount; i++)
        //{
        //    var newPath = ShrinkPath(originalCollider, i).ToArray();
        //    newCollider.SetPath(i, newPath);
        //}
    }
    private List<Vector2> ShrinkPath(PolygonCollider2D collider, int pathIndex)
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
            var candidateDisplacement = (pNorm + nNorm) * 0.005f;
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
