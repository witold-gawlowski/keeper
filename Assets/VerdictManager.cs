using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerdictManager : Singleton<VerdictManager>
{
    public System.Action<bool> verdictEvent;

    [SerializeField] private float numberOfVerdictPointPerUnit = 5.0f;
    [SerializeField] private float dispersionRadius = 0.4f;

    private ObjectPool hitPool;
    private PolygonCollider2D levelCollider;
    int pathCount;
    int levelLayerMask;
    int blockLayerMask;
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
        MainSceneUIManager.Instance.verdictPressedEvent += StartVerdict;
    }
    private void OnDisable()
    {
        if (MainMenuUIManager.Instance)
        {
            MainSceneUIManager.Instance.verdictPressedEvent -= StartVerdict;
        }
    }
    void InitLevelCollider()
    {
        GameObject levelObject = MainSceneManager.Instance.LevelObject;
        levelCollider = levelObject.GetComponent<PolygonCollider2D>();
        pathCount = levelCollider.pathCount;
    }
    void StartVerdict()
    {
        Debug.Log("Start Verdict");
        for(int i=0; i<pathCount; i++)
        {
            var path = levelCollider.GetPath(i);
            Debug.Log("pathcount " + path.Length);
            for (int j=0; j<path.Length; j++)
            {
                var pointA = path[j];
                var pointB = path[(j + 1) % path.Length];
                var length = (pointB - pointA).magnitude;
                var numberOfVerdictPoints = length * numberOfVerdictPointPerUnit;
                var unitVector = (pointB - pointA).normalized;
                for(int k=0; k<numberOfVerdictPoints; k++)
                {
                    var basePosition = unitVector * k + pointA;
                    var randomX = Random.RandomRange(-dispersionRadius, dispersionRadius);
                    var randomY = Random.RandomRange(-dispersionRadius, dispersionRadius);
                    var dispersionVector = new Vector2(randomX, randomY);
                    var dispersedPosition = basePosition + dispersionVector;
                    var levelColliderHit = Physics2D.OverlapPoint(dispersedPosition, levelLayerMask);
                    if (levelColliderHit==null)
                    {
                        var blockColliderHit = Physics2D.OverlapPoint(dispersedPosition, blockLayerMask);
                        if(blockColliderHit != null)
                        {
                            hitPool.Spawn(dispersedPosition);
                        }
                    }
                }
            }
        }
    }
}
