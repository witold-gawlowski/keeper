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
    [SerializeField] private PolygonCollider2D probeCollider;

    bool turnStarted;
    bool isFreezed;
    GameObject draggedBlock;
    Rigidbody2D draggedRigidbody;
    Collider2D draggedCollider;
    Quaternion initialBlockRotation;
    Vector2 pointerOffset;
    Vector2 turnStartPosition;
    int blockLayerMask;
    GameObject lastBlockTouched;
    int lastRotationIndex;

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
    void HandlePointerReleased(Vector2 initialDragPosition, Vector2 finalDragPosition, float timeSpan)
    {
        if (draggedBlock)
        {
            FinishBlockDrag(initialDragPosition, finalDragPosition, timeSpan);
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
        var rotationIndex = (int)( -turnDrag.y * turnSpeed) % 4;
        lastBlockTouched.transform.rotation = initialBlockRotation * Quaternion.Euler(0, 0, rotationIndex * 90);
        if(rotationIndex != lastRotationIndex)
        {
            newRotationPositionEvent();
            lastRotationIndex = rotationIndex;
        }
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
            draggedRigidbody = block.GetComponent<Rigidbody2D>();
            draggedRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            draggedCollider = block.GetComponent<PolygonCollider2D>();
            draggedBlock = block.gameObject;
            ReplicateColliderToProbe(block);
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
        if (!isFreezed && draggedBlock)
        {
            probeCollider.transform.position = worldPos + pointerOffset;
            yield return new WaitForFixedUpdate();
            var colliding = CheckProbeColliding();
            if (colliding)
            {
                draggedRigidbody.AddForce((worldPos + pointerOffset - (Vector2)draggedBlock.transform.position) * 100.0f);
                //draggedRigidbody.MovePosition(worldPos + pointerOffset);
            }
            else
            {
                Debug.Log("not colliding");
                draggedBlock.transform.position = worldPos + pointerOffset;
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
            draggedBlock = null;
        }
    }
    void ReplicateColliderToProbe(GameObject block)
    {
        var collider = block.GetComponent<PolygonCollider2D>();
        probeCollider.pathCount = collider.pathCount;
        for(int i=0; i<probeCollider.pathCount; i++)
        {
            probeCollider.SetPath(i, collider.GetPath(i));
        }
    }
    bool CheckProbeColliding()
    {
        var filter = Helpers.GetSingleLayerMaskContactFilter(Constants.blockLayer);
        var colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(probeCollider, filter, colliders);
        if(colliders.Count > 1 || (colliders.Count == 1 && colliders[0] != draggedCollider))
        {
            return true;
        }
        foreach(var c in colliders)
        {
            Debug.Log(c.name);
        }
        return false;
    }

}
