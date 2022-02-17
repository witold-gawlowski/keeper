using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PolygonCollider2D mCollider;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AnimationCurve flashCurve;
    [SerializeField] private float blockHighlightDelay;

    private float hue;
    private float saturation;
    private float lightness;
    private float transparency;
    private bool isFinalized;
    private float lastTouched;
    private void Awake()
    {
        isFinalized = false;
    }
    private void Update()
    {

        if (!isFinalized)
        {
            var isManipulated = DragManager.Instance.IsBlockManipulated();
            if (!isManipulated)
            {
                var lastTouchFinishedTime = MainSceneManager.Instance.LastManipulationFinishTime;
                if (Time.time - lastTouchFinishedTime > blockHighlightDelay)
                {
                    var v = flashCurve.Evaluate(Time.time);
                    var newColor = Color.HSVToRGB(hue, saturation * 1 / (v), lightness * v);
                    newColor.a = transparency;
                    spriteRenderer.color = newColor;
                }
            }
        }
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
        spriteRenderer.color = c;
        float h; float s; float v;
        Color.RGBToHSV(c, out h, out s, out v);
        hue = h;
        saturation = s;
        lightness = v;
        transparency = c.a;
    }
    public void Finalize()
    {
        isFinalized = true;
    }
}
