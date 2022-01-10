using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    public Sprite GetSprite()
    {
        return sr.sprite;
    }
    public Collider2D GetCollider()
    {
        return col;
    }
    public void SetColor(Color c)
    {
        sr.color = c;
    }
}
