using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : Singleton<DragManager>
{
    public System.Action dragFinishedEvent;

    [SerializeField] float dragForce = 5;
    GameObject draggedBlock;
    Rigidbody2D draggedRB;
    Vector2 pointerOffset;
    [SerializeField] float rotationSpeed = 50f;
    private void OnEnable()
    {
        InputManager.mouse0DownEvent += Mouse0DownEventHandler;
        InputManager.mouse0UpEvent += Mouse0UpEventHandler;
        InputManager.rPressedEvent += RPressedEventHandler;
        InputManager.rUpEvent += RUpEventHandler;
        InputManager.rDownEvent += RDownEventHandler;
        InputManager.mouse0PressedEvent += Mouse0PressedEventHandler;
    }
    private void OnDisable()
    {
        InputManager.mouse0DownEvent -= Mouse0DownEventHandler;
        InputManager.mouse0UpEvent -= Mouse0UpEventHandler;
        InputManager.rPressedEvent -= RPressedEventHandler;
        InputManager.rUpEvent -= RUpEventHandler;
        InputManager.rDownEvent -= RDownEventHandler;
        InputManager.mouse0PressedEvent -= Mouse0PressedEventHandler;
    }
    void Mouse0PressedEventHandler(Vector2 worldPos)
    {
        if (draggedBlock)
        {
            Vector2 direction = pointerOffset + worldPos - (Vector2)draggedBlock.transform.position;
            draggedRB.AddForce(direction * dragForce);
        }
    }
    void Mouse0DownEventHandler(Vector2 worldPos, Collider2D block)
    {
        if (block)
        {
            draggedRB = block.GetComponent<Rigidbody2D>();
            draggedRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            draggedBlock = block.gameObject;
            pointerOffset = (Vector2)draggedBlock.transform.position - worldPos;
        }
    }

    void Mouse0UpEventHandler()
    {
        if (draggedBlock != null)
        {
            draggedRB.constraints = RigidbodyConstraints2D.FreezeAll;
            draggedBlock = null;
            draggedRB = null;
        }
        if (dragFinishedEvent != null)
        {
            dragFinishedEvent();
        }
    }
    void RDownEventHandler()
    {
        if (draggedRB)
        {
            draggedRB.constraints = RigidbodyConstraints2D.None;
        }
    }
    void RUpEventHandler()
    {
        if (draggedRB)
        {
            draggedRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    void RPressedEventHandler()
    {
        if (draggedBlock)
        {
            draggedRB.angularVelocity = rotationSpeed;
        }
    }
}
