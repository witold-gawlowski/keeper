using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainSceneUIManager : Singleton<MainSceneUIManager>
{
    public System.Action cheatPressedEvent;
    public System.Action surrenderPressedEvent;
    public System.Action<BlockScript> blockSelectedForDeletionEvent;
    public System.Action levelCompletedConfirmPressedEvent;
    public System.Action levelFailedConfirmPressedEvent;
    public System.Action verdictPressedEvent;

    [SerializeField] private GameObject tickIcon;
    [SerializeField] private GameObject crossIcon;
    [SerializeField] private TMP_Text componentCountText;
    [SerializeField] private GameObject levelCompletedBox;
    [SerializeField] private GameObject levelFailedBox;
    [SerializeField] private UnityEngine.UI.Image bin;
    [SerializeField] private UnityEngine.UI.Slider scoreSlider;
    [SerializeField] private UnityEngine.UI.Button verdictButton;
    [SerializeField] private UnityEngine.UI.Button surrenderButton;
    [SerializeField] private UnityEngine.UI.Image timeLeftImage;
    [SerializeField] private UnityEngine.UI.Image nextBLockImage;

    private Vector2 binPositionWorld;

    #region Unity functions
    void Start()
    {
        SetBinPosition();
        SetComponentCount(0);
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
        BlockSupplyManager.Instance.selectedBlockUpdatedEvent += HandleSelectedBlockUpdated;
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
    public void SetComponentCount(int val)
    {
        componentCountText.text = val + " component" + (val == 1 ? "" : "s");
        if(val == 1)
        {
            tickIcon.SetActive(true);
            crossIcon.SetActive(false);
        }
        else
        {
            tickIcon.SetActive(false);
            crossIcon.SetActive(true);
        }
    }
    public void SetTimeLeft(float val)
    {
        timeLeftImage.fillAmount = val;
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
    void HandleSelectedBlockUpdated()
    {
        var script = BlockSupplyManager.Instance.SelectedBlockToSpawn.PrefabBlockScript;
        var sprite = script.GetSprite();
        nextBLockImage.sprite = sprite;
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
                var script = col.GetComponent<BlockScript>();
                blockSelectedForDeletionEvent(script);
            }
        }
    }
    #endregion
}
