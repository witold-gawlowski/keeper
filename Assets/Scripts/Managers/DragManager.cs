using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : Singleton<DragManager>
{
    public System.Action dragFinishedEvent;
    public System.Action dragStartedEvent;

    [SerializeField] float dragForce = 15;
    [SerializeField] private float maxForceDistance = 1;
    [SerializeField] private float turnSpeed = 10;

    bool turnStarted;
    bool isFreezed;
    GameObject draggedBlock;
    Quaternion initialBlockRotation;
    Vector2 pointerOffset;
    GameObject lastBlockTouched;
    Vector2 turnStartPosition;
    int blockLayerMask;

    private void Awake()
    {
        turnStarted = false;
        isFreezed = false;
        blockLayerMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
    }
    private void OnEnable()
    {
        BlockManager.Instance.blockSpawnedEvent += HandleBlockSpawedEvent;
        MainSceneManager.Instance.verdictStartedEvent += VerdictStartedHandler;
        InputManager.Instance.pointerDownEvent += HandlePointerDown;
        InputManager.Instance.pointerUpAfterLongDown += HandleFinishDrag;
        InputManager.Instance.pointerPressedEvent += HandlePointerPressedEvent;
    }
    void VerdictStartedHandler()
    {
        isFreezed = true;
    }
    void HandlePointerDown(Vector2 worldPos)
    {
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
    void HandlePointerPressedEvent(Vector2 worldPos)
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
    void HandleFinishDrag(Vector2 initialDragPosition, Vector2 finalDragPosition)
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

    void HandleBlockSpawedEvent(GameObject block)
    {
        lastBlockTouched = block;
    }
    void StartBlockTurn(Vector2 worldPos)
    {
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
            Vector2 pointerBlockVector = pointerOffset + worldPos - (Vector2)draggedBlock.transform.position;
            draggedBlock.transform.position = worldPos;
        }
    }

    void FinishBlockDrag(Vector2 _, Vector2 _2)
    {
        if (!isFreezed)
        {
            lastBlockTouched = draggedBlock;
            draggedBlock = null;
            if (dragFinishedEvent != null)
            {
                dragFinishedEvent();
            }
        }
    }

}
