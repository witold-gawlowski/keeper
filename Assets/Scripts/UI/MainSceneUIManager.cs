using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainSceneUIManager : Singleton<MainSceneUIManager>
{
    public System.Action surrenderPressedEvent;
    public System.Action<GameObject> blockSelectedForDeletionEvent;
    public System.Action levelCompletedConfirmPressedEvent;
    public System.Action verdictPressedEvent;

    [SerializeField] GameObject levelCompletedBox;
    [SerializeField] GameObject levelFailedBox;
    [SerializeField] UnityEngine.UI.Image bin;
    [SerializeField] UnityEngine.UI.Slider scoreSlider;
    [SerializeField] UnityEngine.UI.Button verdictButton;
    [SerializeField] UnityEngine.UI.Button surrenderButton;

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
        MainSceneManager.Instance.verdictStartedEvent += HandleVerdictStarted;
        FillManager.Instance.finishedAreaCalculationFrame += UpdateScore;
        MainSceneManager.Instance.targetFractionHitEvent += HandleTargetFrationHit;
        DragManager.Instance.dragStartedEvent += DisableVerdict;
        BlockManager.Instance.blockSpawnedEvent += DisableVerdict;
        MainSceneManager.Instance.levelCompleted += HandleLevelCompleted;
        MainSceneManager.Instance.levelFailed += HandleLevelFailed;
    }
    #endregion
    #region Custom public functions
    public void LevelCompletedConfimPress()
    {
        levelCompletedConfirmPressedEvent();
    }
    public void OnVerdictPress()
    {
        verdictPressedEvent();
    }
    public void OnSurrender()
    {
        surrenderPressedEvent();
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
    void HandleLevelCompleted()
    {
        levelCompletedBox.SetActive(true);
    }
    void HandleLevelFailed()
    {
        levelFailedBox.SetActive(true);
    }
    void HandleTargetFrationHit()
    {
        verdictButton.interactable = true;
    }
    void HandleVerdictStarted()
    {
        surrenderButton.gameObject.SetActive(false);
        verdictButton.gameObject.SetActive(false);
        bin.gameObject.SetActive(false);
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
                blockSelectedForDeletionEvent(col.gameObject);
            }
        }
    }
    #endregion
    private void OnDisable()
    {
        if (MainSceneManager.Instance)
        {
            MainSceneManager.Instance.verdictStartedEvent -= HandleVerdictStarted;
            MainSceneManager.Instance.targetFractionHitEvent -= HandleTargetFrationHit;
            MainSceneManager.Instance.levelCompleted -= HandleLevelCompleted;
            MainSceneManager.Instance.levelFailed -= HandleLevelFailed;
        }
        if (FillManager.Instance)
        {
            FillManager.Instance.finishedAreaCalculationFrame -= UpdateScore;
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
