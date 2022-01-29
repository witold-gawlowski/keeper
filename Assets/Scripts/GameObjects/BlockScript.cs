using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PolygonCollider2D collider;
    public Sprite GetSprite()
    {
        return sr.sprite;
    }
    public PolygonCollider2D GetCollider()
    {
        return collider;
    }
    public void SetColor(Color c)
    {
        sr.color = c;
    }
}
