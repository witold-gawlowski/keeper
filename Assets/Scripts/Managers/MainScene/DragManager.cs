using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : Singleton<DragManager>
{
    public System.Action dragStartedEvent;
    public System.Action dragContinuedEvent;
    public System.Action<GameObject> dragFinishedEvent;
    public System.Action<GameObject> turnFinishedEvent;
    public System.Action newRotationPositionEvent;

    [SerializeField] private float dragForce = 15;
    [SerializeField] private float maxForceDistance = 1;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private float overlapForce = 40.0f;
    [SerializeField] private GameObject probeParent;
    [SerializeField] private PolygonCollider2D probeCollider;
    [SerializeField] private SpriteRenderer probeSprite;

    bool isTurning;
    bool isFreezed;
    public GameObject draggedBlock;
    Rigidbody2D draggedRigidbody;
    Quaternion initialBlockRotation;
    Vector2 pointerOffset;
    Vector2 turnStartPosition;
    int blockLayerMask;
    GameObject lastBlockTouched;
    int lastRotationIndex;
    bool detached;
    Vector2 positionPreviousFrame;
    float timeOfLastManipulation;

    private void Awake()
    {
        isTurning = false;
        isFreezed = false;
        blockLayerMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
    }
    private void OnEnable()
    {
        MainSceneManager.Instance.verdictStartedEvent += VerdictStartedHandler;
        InputManager.Instance.pointerPressedEvent += HandlePointerPressed;
        InputManager.Instance.pointerReleasedEvent += HandlePointerReleased;
        InputManager.Instance.pointerDownEvent += HandlePointerDown;
        BlockManager.Instance.blockSpawnedEvent += HandleBlockSpawned;
    }
    public bool IsBlockManipulated()
    {
        return draggedBlock || isTurning;
    }
    void VerdictStartedHandler()
    {
        isFreezed = true;
    }
    void HandleBlockSpawned(GameObject block)
    {
        var blockScript = block.GetComponent<BlockScript>();
    }
    void HandlePointerPressed(Vector2 worldPos)
    {
        lastBlockTouched = MainSceneManager.Instance.LastBlockTouched;
        Collider2D hit = Physics2D.OverlapPoint(worldPos, blockLayerMask);
        if (hit)
        {
            if (BlockManager.Instance.LastBlockSpawned == hit.gameObject)
            {
                var blockScript = hit.GetComponent<BlockScript>();
                StartBlockDrag(worldPos, blockScript);
            }
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
            if (isTurning)
            {
                ContinueBlockTurn(worldPos);
            }
        }
    }
    void HandlePointerReleased(Vector2 initialDragPosition, Vector2 finalDragPosition, float timeSpan)
    {
        if (draggedBlock)
        {
            FinishBlockDrag(initialDragPosition, finalDragPosition, timeSpan);
        }
        else if (isTurning)
        {
            EndBlockTurn();
        }
    }
    void StartBlockTurn(Vector2 worldPos)
    {
        lastBlockTouched = MainSceneManager.Instance.LastBlockTouched;
        isTurning = true;
        turnStartPosition = worldPos;
        initialBlockRotation = lastBlockTouched.transform.rotation;
    }
    void ContinueBlockTurn(Vector2 worldPos)
    {
        var turnDrag = (turnStartPosition - worldPos);
        var rotationIndex = (int)(-turnDrag.y * turnSpeed) % 4;
        var newRotation = initialBlockRotation * Quaternion.Euler(0, 0, rotationIndex * 90);
        lastBlockTouched.transform.rotation = newRotation;
        probeParent.transform.rotation = newRotation;
        if (rotationIndex != lastRotationIndex)
        {
            newRotationPositionEvent();
            lastRotationIndex = rotationIndex;
        }
    }
    void EndBlockTurn()
    {
        isTurning = false;
        turnFinishedEvent?.Invoke(lastBlockTouched);
    }
    void StartBlockDrag(Vector2 worldPos, BlockScript block)
    {
        if (!isFreezed && block)
        {
            detached = false;
            draggedRigidbody = block.GetRigidbody();
            draggedRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            draggedBlock = block.gameObject;
            Helpers.ReplicateColliderToProbe(block, probeCollider);
            probeSprite.sprite = block.GetSprite();
            probeParent.transform.rotation = block.transform.rotation;
            pointerOffset = (Vector2)draggedBlock.transform.position - worldPos;
            dragStartedEvent?.Invoke();
        }
    }
    void ContinueBlockDrag(Vector2 worldPos)
    {
        StartCoroutine(ContinueBlockDragCoroutine(worldPos));
    }
    IEnumerator ContinueBlockDragCoroutine(Vector2 worldPos)
    {
        probeParent.transform.position = worldPos + pointerOffset;
        yield return new WaitForFixedUpdate();
        if (!isFreezed && draggedBlock)
        {
            var colliding = CheckProbeColliding();
            if (colliding)
            {
                if (detached == false)
                {
                    detached = true;
                    probeSprite.gameObject.SetActive(true);
                    draggedRigidbody.velocity = (worldPos - positionPreviousFrame) / Time.deltaTime;
                }
                var target = worldPos + pointerOffset;
                var currentPosition = (Vector2)draggedBlock.transform.position;
                var pointingVector = (target - currentPosition);
                draggedRigidbody.AddForce(pointingVector * overlapForce);
            }
            else
            {
                if (detached == true)
                {
                    detached = false;
                    probeSprite.gameObject.SetActive(false);
                }
                draggedBlock.transform.position = worldPos + pointerOffset;
                positionPreviousFrame = worldPos;
            }
            dragContinuedEvent();
        }
    }

    void FinishBlockDrag(Vector2 _, Vector2 _2, float _3)
    {
        if (!isFreezed)
        {
            if (dragFinishedEvent != null)
            {
                dragFinishedEvent(draggedBlock);
            }
            draggedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            probeSprite.gameObject.SetActive(false);
            draggedBlock = null;
        }
    }
    bool CheckProbeColliding()
    {
        var filter = Helpers.GetSingleLayerMaskContactFilter(Constants.blockLayer);
        var colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(probeCollider, filter, colliders);
        foreach (var c in colliders)
        {
            if (c.gameObject != draggedBlock)
            {
                return true;
            }
        }
        return false;
    }

}
