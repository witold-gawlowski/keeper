using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(BlockScript))]
public class BlockScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Shrink collider"))
        {
            Shrink();
        }
        DrawDefaultInspector();
    }
    public void Shrink()
    {
        BlockScript script = target as BlockScript;
        var collider = script.GetCollider();
        var pathCount = collider.pathCount;
        for (int i = 0; i < pathCount; i++)
        {
            var newPath = ShrinkPath(collider, i).ToArray();
            collider.SetPath(i, newPath);
        }
    }
    private List<Vector2> ShrinkPath(PolygonCollider2D collider, int pathIndex)
    {
        BlockScript script = target as BlockScript;
        var result = new List<Vector2>();
        var pathPoints = collider.GetPath(pathIndex);
        var len = pathPoints.Length;
        for (int i = 0; i < len; i++)
        {
            var p = pathPoints[(i + len - 1) % len]; //previous
            var c = pathPoints[i]; //current
            var n = pathPoints[(i + 1) % len]; //next
            var pNorm = (c - p).normalized;
            var nNorm = (n - c).normalized;
            var testDisplacement = (pNorm - nNorm) * 0.01f;
            var refPos = (Vector2)script.transform.position;
            var cornerDirectionTest = c + testDisplacement;
            Debug.DrawLine(refPos + c, refPos + cornerDirectionTest, Color.red, 0.5f);
            bool testInside = IsPointInsideCollider(collider, refPos + cornerDirectionTest);
            var newPoint = c + (pNorm - nNorm) * 0.0015f * (testInside ? 1 : -1);
            result.Add(newPoint);
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
