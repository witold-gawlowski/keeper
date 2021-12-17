using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainSceneUIManager : Singleton<MainSceneUIManager>
{
    public System.Action SurrenderPressedEvent;
    public System.Action<GameObject> BlockSelectedForDeletionEvent;
    public System.Action LevelCompletedConfirmPressedEvent;
    public System.Action verdictPressedEvent;

    [SerializeField] GameObject levelCompletedBox;
    [SerializeField] UnityEngine.UI.Image bin;
    [SerializeField] UnityEngine.UI.Slider scoreSlider;
    [SerializeField] UnityEngine.UI.Button verdictButton;

    private Vector2 binPositionWorld;

    #region Unity functions
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
        DragManager.Instance.dragStartedEvent += DisableVerdict;
        BlockManager.Instance.blockSpawnedEvent += DisableVerdict;
    }
    #endregion
    #region Custom public functions
    public void LevelCompletedConfimPress()
    {
        LevelCompletedConfirmPressedEvent();
    }
    public void OnVerdictPress()
    {
        verdictPressedEvent();
    }
    public void OnSurrender()
    {
        SurrenderPressedEvent();
    }
    #endregion
    #region Custom private functions
    void SetBinPosition()
    {
        binPositionWorld = Camera.main.ScreenToWorldPoint(bin.transform.position);
    }
    void UpdateScore(float value)
    {
        scoreSlider.value = value;
    }
    void HandleTargetFrationHit()
    {
        verdictButton.interactable = true;
    }
    void DisableVerdict(GameObject _)
    {
        DisableVerdict();
    }
    void DisableVerdict()
    {
        verdictButton.interactable = false;
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
    #endregion
    private void OnDisable()
    {
        if (FillManager.Instance)
        {
            FillManager.Instance.finishedAreaCalculationFrame -= UpdateScore;
        }
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.targetFractionHitEvent -= HandleTargetFrationHit;
        }
        if (DragManager.Instance)
        {
            DragManager.Instance.dragStartedEvent -= DisableVerdict;
        }
        if (BlockManager.Instance)
        {
            BlockManager.Instance.blockSpawnedEvent -= DisableVerdict;
        }
    }
}
