using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class FillManager: Singleton<FillManager>
{
    public System.Action<float> finishedCalulatingAreaFractionEvent;
    public System.Action<float> finishedAreaCalculationFrame;
    public float AreaFractionCovered { get; private set; }

    [SerializeField] private int numberOfEvalutionTries;
    private ObjectPool hitPool;
    private Vector2 minBoundCoordinatesCorner;
    private Vector2 maxBoundCoordinatesCorner;
    private int levelMask;
    private int blockMask;
    private int hits;
    private int tries;
    void Start()
    {
        hitPool = GetComponentInChildren<ObjectPool>();
        SetupBounds();
        levelMask = Helpers.GetSingleLayerMask(Constants.levelLayer);
        blockMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    public IEnumerator CalculateCoveredAreaFraction()
    {
        hits = 0;
        tries = 0;
        var sw = new Stopwatch();
        sw.Start();
        bool done = false;
        while (!done)
        {
            sw.Restart();
            while (sw.Elapsed.TotalSeconds < Time.fixedDeltaTime)
            {
                Vector2 randomPoint = GetRandomPointInBounds();
                var hitLevel = Physics2D.OverlapPoint(randomPoint, levelMask);
                if (hitLevel != null)
                {
                    tries++;
                    var hitBlock = Physics2D.OverlapPoint(randomPoint, blockMask);
                    if (hitBlock != null)
                    {
                        hits++;
                    }
                }
                if (tries >= numberOfEvalutionTries)
                {
                    done = true;
                    break;
                }
            }
            finishedAreaCalculationFrame(1.0f * hits / tries);
            yield return new WaitForFixedUpdate();
        }
        var resultFraction = 1.0f * hits / tries;
        AreaFractionCovered = resultFraction;
        finishedCalulatingAreaFractionEvent?.Invoke(resultFraction);
    }
    void SetupBounds()
    {
        GameObject levelGO = GameObject.FindWithTag(Constants.levelLayer);
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
            UnityEngine.Debug.LogWarning("Missing Level Object!");
        }
    }
    Vector2 GetRandomPointInBounds()
    {
        float randomX = Random.Range(minBoundCoordinatesCorner.x, maxBoundCoordinatesCorner.x);
        float randomY = Random.Range(minBoundCoordinatesCorner.y, maxBoundCoordinatesCorner.y);
        return new Vector2(randomX, randomY);
    }
}
