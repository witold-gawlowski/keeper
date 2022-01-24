using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BlockSO))]
[CanEditMultipleObjects]
public class BlockSOEditor: Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
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
        var scaleFactor = Mathf.Min(width / rWidth, height / rHeight);
        var newTex = new Texture2D(rWidth , rHeight );
        newTex.SetPixels(pixData);
        newTex.Apply();
        return newTex;

    }
    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                int x = (int)((float)j / (float)result.width * source.width);
                int y = (int)((float)i / (float)result.height * source.height);
                Color newColor = source.GetPixel(x, y);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }
}