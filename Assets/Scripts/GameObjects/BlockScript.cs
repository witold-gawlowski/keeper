using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PolygonCollider2D mCollider;
    [SerializeField] private Rigidbody2D rigidBody;
    private bool isFinalized;
    private void Awake()
    {
        isFinalized = false;
    }
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
        var newColor = isFinalized ? Helpers.GetDarkenedColor(c, 0.75f) : c;
        spriteRenderer.color = newColor;
    }
    public void Finalize()
    {
        isFinalized = true;
    }
}
