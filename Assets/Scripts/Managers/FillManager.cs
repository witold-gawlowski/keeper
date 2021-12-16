using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class FillManager: Singleton<FillManager>
{
    public System.Action<float> finishedCalulatingAreaFractionEvent;
    public System.Action<float> finishedAreaCalculationFrame;

    [SerializeField] int numberOfEvalutionTries;
    private ObjectPool hitPool;

    Vector2 minBoundCoordinatesCorner;
    Vector2 maxBoundCoordinatesCorner;

    int hits;
    int tries;
    void Start()
    {
        hitPool = GetComponentInChildren<ObjectPool>();
        SetupBounds();
    }
    private void OnEnable()
    {
        DragManager.Instance.dragFinishedEvent += StartCalculateCoveredAreaFractionCoroutine;
        BlockManager.Instance.blockSpawnedEvent += (GameObject g) => StartCalculateCoveredAreaFractionCoroutine();
    }
    private void OnDisable()
    {
        if (DragManager.Instance)
        {
            DragManager.Instance.dragFinishedEvent -= StartCalculateCoveredAreaFractionCoroutine;
        }
        if (BlockManager.Instance)
        {
            BlockManager.Instance.blockSpawnedEvent -= (GameObject g) => StartCalculateCoveredAreaFractionCoroutine();
        }
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
            UnityEngine.Debug.LogWarning("Missing Level Object!");
        }
    }
    void StartCalculateCoveredAreaFractionCoroutine()
    {
        StartCoroutine(CalculateCoveredAreaFraction());
    }
    IEnumerator CalculateCoveredAreaFraction()
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
                bool hit = TryHit();
                if (hit)
                {
                    hits++;
                }
                tries++;
                if(tries >= numberOfEvalutionTries)
                {
                    done = true;
                    break;
                }
            }
            finishedAreaCalculationFrame(1.0f * hits / tries);
            UnityEngine.Debug.Log(hits + " " + tries + " " + sw.Elapsed.TotalSeconds + " "+ Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        finishedCalulatingAreaFractionEvent(1.0f * hits / tries);
    }
    bool TryHit()
    {
        float randomX = Random.Range(minBoundCoordinatesCorner.x, maxBoundCoordinatesCorner.x);
        float randomY = Random.Range(minBoundCoordinatesCorner.y, maxBoundCoordinatesCorner.y);
        Vector2 randomPoint = new Vector2(randomX, randomY);
        Collider2D col = Physics2D.OverlapPoint(randomPoint);
        return col != null;
    }

}
