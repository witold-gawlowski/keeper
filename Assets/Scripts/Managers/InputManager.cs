using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : Singleton<InputManager>
{
    public System.Action<Vector2> pointerDownEvent;
    public System.Action<Vector2> mouse0DownWithDPressedEvent;
    public System.Action<Vector2> pointerPressedEvent;
    public System.Action<Vector2> shortTouchFinishedEvent;
    public System.Action<Vector2, Vector2> pointerUpAfterLongDown;
    public System.Action rPressedEvent;
    public System.Action rDownEvent;
    public System.Action rUpEvent;
    public System.Action dPressedEvent;

    Vector2 currentDragStartPositionWorld;
    Collider2D currentTouchInitialPositionHit;
    bool rPressed;
    Vector3 pointerPositionScreen;
    Camera mainCamera;
    private void Start()
    {
        rPressed = false;
        mainCamera = Camera.main;
    }
    void Update()
    {
#if UNITY_EDITOR
        pointerPositionScreen = Input.mousePosition;
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            Vector2 mousePositionWorld = mainCamera.ScreenToWorldPoint(pointerPositionScreen);
            if (Input.GetMouseButtonDown(0))
            {
                currentDragStartPositionWorld = mousePositionWorld;
                if (Input.GetKey(KeyCode.D))
                {
                    mouse0DownWithDPressedEvent(mousePositionWorld);
                }
                else
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        pointerDownEvent(mousePositionWorld);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var drag = currentDragStartPositionWorld - mousePositionWorld;
                if (drag.magnitude < 0.1f)
                {
                    shortTouchFinishedEvent(mousePositionWorld);
                }
                else if(drag.magnitude >= 0.1f)
                {
                    pointerUpAfterLongDown(currentDragStartPositionWorld, mousePositionWorld);
                }
            }
            pointerPressedEvent(mousePositionWorld);
        }

        if (Input.GetKey(KeyCode.R))
        {
            rPressed = true;
            rPressedEvent();
        }
        else
        {
            if (rPressed)
            {
                rUpEvent();
                rPressed = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rDownEvent();
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            Vector2 pointerPositionWorld = mainCamera.ScreenToWorldPoint(touch.position);
            
            if (touch.phase == TouchPhase.Began)
            {
                currentDragStartPositionWorld = pointerPositionWorld;
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    pointerDownEvent(pointerPositionWorld);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                shortTouchFinishedEvent(pointerPositionWorld);
            }
            pointerPressedEvent(pointerPositionWorld);
        }
#endif
    }
}
