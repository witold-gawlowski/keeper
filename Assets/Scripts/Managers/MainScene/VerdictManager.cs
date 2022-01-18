using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerdictManager : Singleton<VerdictManager>
{
    public System.Action<int> resultEvent;

    [SerializeField] private float numberOfVerdictPointPerUnit = 5.0f;
    [SerializeField] private float dispersionRadius = 0.4f;
    [SerializeField] private float probeInterval = 0.1f;

    private ObjectPool hitPool;
    private PolygonCollider2D levelCollider;
    int pathCount;
    int levelLayerMask;
    int blockLayerMask;
    int hitCounter;
    private void Awake()
    {
        hitPool = GetComponentInChildren<ObjectPool>();
        levelLayerMask = Helpers.GetSingleLayerMask(Constants.levelLayer);
        blockLayerMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
    }
    private void Start()
    {
        InitLevelCollider();
    }
    private void OnEnable()
    {
        MainSceneManager.Instance.verdictStartedEvent += StartRunVerdictCoroutine;
    }
    private void OnDisable()
    {
        if (MainMenuUIManager.Instance)
        {
            MainSceneManager.Instance.verdictStartedEvent -= StartRunVerdictCoroutine;
        }
    }
    void InitLevelCollider()
    {
        GameObject levelObject = MainSceneManager.Instance.LevelObject;
        levelCollider = levelObject.GetComponent<PolygonCollider2D>();
        pathCount = levelCollider.pathCount;
    }
    void StartRunVerdictCoroutine()
    {
        StartCoroutine(RunVerdict());
    }
    IEnumerator RunVerdict()
    {
        hitCounter = 0;
        for (int i=0; i<pathCount; i++)
        {
            var path = levelCollider.GetPath(i);
            for (int j=0; j<path.Length; j++)
            {
                var pointA = path[j];
                var pointB = path[(j + 1) % path.Length];
                var length = (pointB - pointA).magnitude;
                var numberOfVerdictPoints = length * numberOfVerdictPointPerUnit;
                var unitVector = (pointB - pointA).normalized;
                for(int k=0; k<numberOfVerdictPoints; k++)
                {
                    var basePosition = unitVector * 1.0f / numberOfVerdictPointPerUnit * k + pointA;
                    var randomX = Random.Range(-dispersionRadius, dispersionRadius);
                    var randomY = Random.Range(-dispersionRadius, dispersionRadius);
                    var dispersionVector = new Vector2(randomX, randomY);
                    var dispersedPosition = basePosition + dispersionVector;
                    var levelColliderHit = Physics2D.OverlapPoint(dispersedPosition, levelLayerMask);
                    if (levelColliderHit == null)
                    {
                        var blockColliderHit = Physics2D.OverlapPoint(dispersedPosition, blockLayerMask);
                        var hit = hitPool.Spawn(dispersedPosition) as ProbeScript;
                        if (blockColliderHit != null)
                        {
                            hit.HandleHit();
                            hitCounter++;
                        }
                        else
                        {
                            hit.HandleMiss();
                        }
                        Debug.Log("verdict step");
                        yield return new WaitForSeconds(probeInterval);
                    }
                }
            }
        }
        resultEvent(hitCounter);
    }
}
