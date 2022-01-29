using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BlockColliderShrinkerScript))]
public class BlockColliderShrinkerEditor : Editor
{
    //unifinished, see BlockColliderShrinkerScript
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BlockColliderShrinkerScript myTarget = (BlockColliderShrinkerScript)target;
        if(GUILayout.Button("Add shrinked collider data"))
        {
            myTarget.Shrink();
        }
    }
}
