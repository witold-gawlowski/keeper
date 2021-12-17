using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : Singleton<DragManager>
{
    public System.Action dragFinishedEvent;
    public System.Action dragStartedEvent;

    [SerializeField] float dragForce = 5;
    GameObject draggedBlock;
    Rigidbody2D draggedRB;
    Vector2 pointerOffset;
    [SerializeField] float rotationSpeed = 50f;
    bool isFreezed;
    private void Awake()
    {
        isFreezed = false;
    }
    private void OnEnable()
    {
        MainSceneManager.Instance.verdictStartedEvent += VerdictStartedHandler;
        InputManager.mouse0DownEvent += StartBlockDrag;
        InputManager.mouse0UpEvent += FinishBlockDrag;
        InputManager.rPressedEvent += ContintueBlockRotation;
        InputManager.rUpEvent += FinishBlockRotation;
        InputManager.rDownEvent += StartBlockRotation;
        InputManager.mouse0PressedEvent += ContinueBlockDrag;
    }
    private void OnDisable()
    {
        MainSceneManager.Instance.verdictStartedEvent -= VerdictStartedHandler;
        InputManager.mouse0DownEvent -= StartBlockDrag;
        InputManager.mouse0UpEvent -= FinishBlockDrag;
        InputManager.rPressedEvent -= ContintueBlockRotation;
        InputManager.rUpEvent -= FinishBlockRotation;
        InputManager.rDownEvent -= StartBlockRotation;
        InputManager.mouse0PressedEvent -= ContinueBlockDrag;
    }
    void VerdictStartedHandler()
    {
        isFreezed = true;
    }
    void ContinueBlockDrag(Vector2 worldPos)
    {
        if (!isFreezed && draggedBlock)
        {
            Vector2 direction = pointerOffset + worldPos - (Vector2)draggedBlock.transform.position;
            draggedRB.AddForce(direction * dragForce);
        }
    }
    void StartBlockDrag(Vector2 worldPos, Collider2D block)
    {
        if (!isFreezed && block)
        {
            draggedRB = block.GetComponent<Rigidbody2D>();
            draggedRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            draggedBlock = block.gameObject;
            pointerOffset = (Vector2)draggedBlock.transform.position - worldPos;
            dragStartedEvent?.Invoke();
        }
    }
    void FinishBlockDrag()
    {
        if (!isFreezed && draggedBlock != null)
        {
            draggedRB.constraints = RigidbodyConstraints2D.FreezeAll;
            draggedBlock = null;
            draggedRB = null;
        }
        if (!isFreezed && dragFinishedEvent != null)
        {
            dragFinishedEvent();
        }
    }
    void StartBlockRotation()
    {
        if (!isFreezed && draggedRB)
        {
            draggedRB.constraints = RigidbodyConstraints2D.None;
        }
    }
    void FinishBlockRotation()
    {
        if (!isFreezed && draggedRB)
        {
            draggedRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    void ContintueBlockRotation()
    {
        if (!isFreezed && draggedBlock)
        {
            draggedRB.angularVelocity = rotationSpeed;
        }
    }
}
