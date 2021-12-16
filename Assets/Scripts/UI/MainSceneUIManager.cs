using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainSceneUIManager : Singleton<MainSceneUIManager>
{
    public System.Action SurrenderPressedEvent;
    public System.Action<GameObject> BlockSelectedForDeletionEvent;
    public System.Action LevelCompletedConfirmPressedEvent;

    [SerializeField] GameObject levelCompletedBox;
    [SerializeField] UnityEngine.UI.Image bin;
    [SerializeField] UnityEngine.UI.Slider scoreSlider;

    private Vector2 binPositionWorld;
    void Start()
    {
        SetBinPosition();
    }
    void Update()
    {
        CheckForBlockToRemove();
    }
    private void OnEnable()
    {
        FillManager.Instance.finishedAreaCalculationFrame += UpdateScore;
        ScoreManager.Instance.targetFractionHitEvent += HandleTargetFrationHit;
    }
    private void OnDisable()
    {
        if (FillManager.Instance)
        {
            FillManager.Instance.finishedAreaCalculationFrame += UpdateScore;
        }
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.targetFractionHitEvent -= HandleTargetFrationHit;
        }
    }
    void SetBinPosition()
    {
        binPositionWorld = Camera.main.ScreenToWorldPoint(bin.transform.position);
    }
    public void OnSurrender()
    {
        SurrenderPressedEvent();
    }
    void CheckForBlockToRemove()
    {
        Collider2D col = Physics2D.OverlapPoint(binPositionWorld);
        if (col)
        {
            if (col.tag == Constants.blockTag)
            {
                BlockSelectedForDeletionEvent(col.gameObject);
            }
        }
    }
    void UpdateScore(float value)
    {
        scoreSlider.value = value;
    }
    void HandleTargetFrationHit()
    {
        levelCompletedBox.SetActive(true);
    }
    public void LevelCompletedConfimPress()
    {
        LevelCompletedConfirmPressedEvent();
    }
}
