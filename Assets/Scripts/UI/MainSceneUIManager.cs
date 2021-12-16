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
        ScoreManager.Instance.scoreUpdatedEvent += UpdateScore;
        ScoreManager.Instance.targetFractionHitEvent += HandleTargetFrationHit;
    }
    private void OnDisable()
    {
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.scoreUpdatedEvent -= UpdateScore;
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
    void UpdateScore(int value, int total)
    {
        scoreSlider.value = value * 1.0f / total;
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
