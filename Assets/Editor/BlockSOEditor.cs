using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BlockSO))]
[CanEditMultipleObjects]
public class BlockSOEditor: Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        BlockSO block = target as BlockSO;
        var sprite = block.prefab.GetComponent<SpriteRenderer>().sprite;
        var rect = sprite.rect;
        var x = (int)rect.x;
        var y = (int)rect.y;
        var rWidth = (int)rect.width;
        var rHeight = (int)rect.height;
        var texture = sprite.texture;
        var pixData = texture.GetPixels(x, y, rWidth, rHeight);
        var newTex = new Texture2D((int)rect.width, (int)rect.height);
        newTex.SetPixels(pixData);
        newTex.Apply();
        return sprite.texture;
    }
}