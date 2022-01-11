using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ColorSO))]
public class ColorSOEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        ColorSO color = target as ColorSO;
        var newTex = new Texture2D(50, 50);
        var pixData = new Color[50*50];
        for(int i=0; i<50*50; i++)
        {
            pixData[i] = color.value;
        }
        newTex.SetPixels(pixData);
        newTex.Apply();
        return newTex;
    }
}