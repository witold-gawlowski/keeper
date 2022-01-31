using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PolygonCollider2D mCollider;
    [SerializeField] private Rigidbody2D rigidBody;
    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }
    public PolygonCollider2D GetCollider()
    {
        return mCollider;
    }
    public Rigidbody2D GetRigidbody()
    {
        return rigidBody;
    }
    public void SetColor(Color c)
    {
        spriteRenderer.color = c;
    }
}
