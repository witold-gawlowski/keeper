using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : Singleton<InputManager>
{
    public System.Action<Vector2> pointerPressedEvent;
    public System.Action<Vector2> pointerDownEvent;
    public System.Action<Vector2> pointerReleasedAfterHoldEvent;
    public System.Action<Vector2, Vector2> pointerInstantlyReleasedEvent;

    Vector2 currentDragStartPositionWorld;
    Collider2D currentTouchInitialPositionHit;
    bool rPressed;
    Vector3 pointerPositionScreen;
    Camera mainCamera;
    Touch touch;
    private void Start()
    {
        rPressed = false;
        mainCamera = Camera.main;
    }
    void Update()
    {
#if UNITY_EDITOR
        pointerPositionScreen = PointerPosition();
        if (PointerDown() || PointerReleased())
        {
            Vector2 pointerPositionWorld = mainCamera.ScreenToWorldPoint(pointerPositionScreen);
            if (PointerPressed())
            {
                currentDragStartPositionWorld = pointerPositionWorld;
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    pointerPressedEvent(pointerPositionWorld);
                }
            }
            else if (PointerReleased())
            {
                var drag = currentDragStartPositionWorld - pointerPositionWorld;
                if (drag.magnitude < 0.1f)
                {
                    pointerReleasedAfterHoldEvent(pointerPositionWorld);
                }
                else if (drag.magnitude >= 0.1f)
                {
                    pointerInstantlyReleasedEvent(currentDragStartPositionWorld, pointerPositionWorld);
                }
            }
            pointerDownEvent(pointerPositionWorld);
        }
#endif
//#if UNITY_ANDROID
//        if (Input.touchCount == 1)
//        {
//            var touch = Input.GetTouch(0);
//            Vector2 pointerPositionWorld = mainCamera.ScreenToWorldPoint(touch.position);

//            if (touch.phase == TouchPhase.Began)
//            {
//                currentDragStartPositionWorld = pointerPositionWorld;
//                if (!EventSystem.current.IsPointerOverGameObject())
//                {
//                    pointerPressedEvent(pointerPositionWorld);
//                }
//            }
//            else if (touch.phase == TouchPhase.Ended)
//            {
//                var drag = currentDragStartPositionWorld - pointerPositionWorld;
//                if (drag.magnitude < 0.1f)
//                {
//                    pointerReleasedAfterHoldEvent(pointerPositionWorld);
//                }
//                else if (drag.magnitude >= 0.1f)
//                {
//                    pointerInstantlyReleasedEvent(currentDragStartPositionWorld, pointerPositionWorld);
//                }
//            }
//            pointerDownEvent(pointerPositionWorld);
//        }
//#endif
    }
    bool PointerDown()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }
        return false;
#endif
    }
    bool PointerPressed()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            return true;
        }
        return false;
#endif
    }
    bool PointerReleased()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonUp(0);
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }
        return false;
#endif
    }
    Vector2 PointerPosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#endif
#if UNITY_ANDROID
        var result = new Vector2();
        if(Input.touchCount == 1)
        {
            result = Input.GetTouch(0).position;
        }
        return result;
#endif
    }
}
