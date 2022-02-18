using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapScript))]
public class MapScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("EnlargeColliderXSCALE"))
        {
            Enlarge();
        }

        DrawDefaultInspector();
    }
    private void Enlarge()
    {
        MapScript script = target as MapScript;
        var collider = script.GetComponent<PolygonCollider2D>();
        var pathCount = collider.pathCount;
        for (int i = 0; i < pathCount; i++)
        {
            var path = collider.GetPath(i);
            var newPath = new List<Vector2>();
            foreach (var p in path)
            {
                var newPoint = new Vector2(p.x * script.scale, p.y * script.scale);
                newPath.Add(newPoint);
            }
            collider.SetPath(i, newPath);
        }
    }
}
