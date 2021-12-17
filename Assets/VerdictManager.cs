using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerdictManager : Singleton<VerdictManager>
{
    public System.Action<bool> verdictEvent;

    private PolygonCollider2D levelCollider;
    private void Start()
    {
        GameObject levelObject = MainSceneManager.Instance.LevelObject;
        levelCollider = levelObject.GetComponent<PolygonCollider2D>();
        foreach(var p in levelCollider.points)
        {
            Debug.Log(p);
        }
    }
    private void OnEnable()
    {
        MainSceneUIManager.Instance.verdictPressedEvent += StartVerdict;
    }
    void StartVerdict()
    {

    }
}
