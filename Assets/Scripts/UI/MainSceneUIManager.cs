using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainSceneUIManager : Singleton<MainSceneUIManager>
{
    public System.Action cheatPressedEvent;
    public System.Action surrenderPressedEvent;
    public System.Action<GameObject> blockSelectedForDeletionEvent;
    public System.Action levelCompletedConfirmPressedEvent;
    public System.Action levelFailedConfirmPressedEvent;
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
        MainSceneManager.Instance.verdictConditionsMetEvent += HandleTargetFrationHit;
        DragManager.Instance.dragStartedEvent += DisableVerdict;
        BlockManager.Instance.blockSpawnedEvent += DisableVerdict;
        MainSceneManager.Instance.levelCompletedEvent += HandleLevelCompleted;
        MainSceneManager.Instance.levelFailedEvent += HandleLevelFailed;
    }
    #endregion
    #region Custom public functions
    public void OnLevelCompletedConfimPressed()
    {
        levelCompletedConfirmPressedEvent();
    }
    public void OnLevelFailedConfirmPressed()
    {
        levelFailedConfirmPressedEvent();   
    }
    public void OnCheatPressed()
    {
        cheatPressedEvent();
    }
    public void OnVerdictPressed()
    {
        verdictPressedEvent();
    }
    public void OnSurrenderPressed()
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
}
