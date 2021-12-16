using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager: Singleton<FillManager>
{
    public int probeCountPerSecond;
    [HideInInspector] public System.Action<bool> tryHitEvent;

    private ObjectPool hitPool;

    Vector2 minBoundCoordinatesCorner;
    Vector2 maxBoundCoordinatesCorner;

    int skippedFixedUpdateCounter;
    int numberOfFixedUpdatesToSkip;
    void Start()
    {
        CalculateSkipCount();
        skippedFixedUpdateCounter = 0;
        hitPool = GetComponentInChildren<ObjectPool>();
        SetupBounds();
    }
    void FixedUpdate()
    {
        if (skippedFixedUpdateCounter % numberOfFixedUpdatesToSkip == 0)
        {
            TryHit();
        }
        skippedFixedUpdateCounter++;
    }
    void SetupBounds()
    {
        GameObject levelGO = GameObject.FindWithTag(Constants.levelTag);
        if (levelGO)
        {
            Collider2D levelCol = levelGO.GetComponent<Collider2D>();
            if (levelCol)
            {
                Bounds levelBounds = levelCol.bounds;
                minBoundCoordinatesCorner = levelBounds.min;
                maxBoundCoordinatesCorner = levelBounds.max;
            }
        }
        else
        {
            Debug.LogWarning("Missing Level Object!");
        }
    }
    void TryHit()
    {
        float randomX = Random.Range(minBoundCoordinatesCorner.x, maxBoundCoordinatesCorner.x);
        float randomY = Random.Range(minBoundCoordinatesCorner.y, maxBoundCoordinatesCorner.y);
        Vector2 randomPoint = new Vector2(randomX, randomY);
        Collider2D col = Physics2D.OverlapPoint(randomPoint);
        tryHitEvent(col);
    }
    void CalculateSkipCount()
    {
        float defaultProbeRate = 1.0f / Time.fixedDeltaTime;
        numberOfFixedUpdatesToSkip = Mathf.RoundToInt(defaultProbeRate / probeCountPerSecond);
    }

}
