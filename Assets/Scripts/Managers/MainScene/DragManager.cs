using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : Singleton<DragManager>
{
    public System.Action dragStartedEvent;
    public System.Action dragContinuedEvent;
    public System.Action<GameObject> dragFinishedEvent;
    public System.Action<GameObject> turnFinishedEvent;

    [SerializeField] float dragForce = 15;
    [SerializeField] private float maxForceDistance = 1;
    [SerializeField] private float turnSpeed = 10;

    bool turnStarted;
    bool isFreezed;
    GameObject draggedBlock;
    Quaternion initialBlockRotation;
    Vector2 pointerOffset;
    Vector2 turnStartPosition;
    int blockLayerMask;
    GameObject lastBlockTouched;

    private void Awake()
    {
        turnStarted = false;
        isFreezed = false;
        blockLayerMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
    }
    private void OnEnable()
    {
        MainSceneManager.Instance.verdictStartedEvent += VerdictStartedHandler;
        InputManager.Instance.pointerPressedEvent += HandlePointerPressed;
        InputManager.Instance.pointerReleased += HandlePointerReleased;
        InputManager.Instance.pointerDownEvent += HandlePointerDown;
    }
    void VerdictStartedHandler()
    {
        isFreezed = true;
    }
    void HandlePointerPressed(Vector2 worldPos)
    {
        lastBlockTouched = MainSceneManager.Instance.LastBlockTouched;
        Collider2D hit = Physics2D.OverlapPoint(worldPos, blockLayerMask);
        if (hit)
        {
            StartBlockDrag(worldPos, hit.gameObject);
        }
        else if (hit == null && lastBlockTouched != null)
        {
            StartBlockTurn(worldPos);
        }
    }
    void HandlePointerDown(Vector2 worldPos)
    {
        if (draggedBlock)
        {
            ContinueBlockDrag(worldPos);
        }
        else if (draggedBlock == null && lastBlockTouched != null)
        {
            if (turnStarted)
            {
                ContinueBlockTurn(worldPos);
            }
        }
    }
    void HandlePointerReleased(Vector2 initialDragPosition, Vector2 finalDragPosition)
    {
        if (draggedBlock)
        {
            FinishBlockDrag(initialDragPosition, finalDragPosition);
        }
        else if (turnStarted)
        {
            EndBlockTurn();
        }
    }
    void StartBlockTurn(Vector2 worldPos)
    {
        lastBlockTouched = MainSceneManager.Instance.LastBlockTouched;
        turnStarted = true;
        turnStartPosition = worldPos;
        initialBlockRotation = lastBlockTouched.transform.rotation;
    }
    void ContinueBlockTurn(Vector2 worldPos)
    {
        var turnDrag = (turnStartPosition - worldPos);
        lastBlockTouched.transform.rotation = initialBlockRotation * Quaternion.Euler(0, 0, -turnDrag.y * turnSpeed);
    }
    void EndBlockTurn()
    {
        turnStarted = false;
        turnFinishedEvent?.Invoke(lastBlockTouched);
    }
    void StartBlockDrag(Vector2 worldPos, GameObject block)
    {
        if (!isFreezed && block)
        {
            draggedBlock = block.gameObject;
            pointerOffset = (Vector2)draggedBlock.transform.position - worldPos;
            dragStartedEvent?.Invoke();
        }
    }
    void ContinueBlockDrag(Vector2 worldPos)
    {
        if (!isFreezed && draggedBlock)
        {
            draggedBlock.transform.position = worldPos + pointerOffset;
            dragContinuedEvent();
        }
    }

    void FinishBlockDrag(Vector2 _, Vector2 _2)
    {
        if (!isFreezed)
        {
            if (dragFinishedEvent != null)
            {
                dragFinishedEvent(draggedBlock);
            }
            draggedBlock = null;
        }
    }

}
