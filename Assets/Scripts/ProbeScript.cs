using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeScript : Poolable
{
    [SerializeField] Color missStartColor;
    [SerializeField] Color missEndColor;
    [SerializeField] Color hitColor;
    [SerializeField] float animationLength;

    SpriteRenderer sr;
    float animationTime;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();    
    }
    public void HandleHit()
    {
        sr.color = hitColor;
    }
    public void HandleMiss()
    {
        StartCoroutine(HitAnimation());
    }
    IEnumerator HitAnimation()
    {
        animationTime = 0;
        while(animationTime < animationLength)
        {
            float completionFraction = animationTime / animationLength;
            sr.color = missStartColor * (1 - completionFraction) + missEndColor * completionFraction;
            animationTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        ProbePool.Instance.Despawn(this);
    }
}
