using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    public Sprite GetSprite()
    {
        return sr.sprite;
    }
}
