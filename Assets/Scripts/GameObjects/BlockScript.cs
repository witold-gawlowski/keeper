using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PolygonCollider2D linkingCollider;
    [SerializeField] private PolygonCollider2D blockingCollider;
    public Sprite GetSprite()
    {
        return sr.sprite;
    }
    public PolygonCollider2D GetLinkingCollider()
    {
        return linkingCollider;
    }
    public PolygonCollider2D GetBlockingCollider()
    {
        return blockingCollider;
    }
    public void SetColor(Color c)
    {
        sr.color = c;
    }
}
