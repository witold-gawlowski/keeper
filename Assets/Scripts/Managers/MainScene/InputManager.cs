using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : Singleton<InputManager>
{
    public System.Action<Vector2> pointerPressedEvent;
    public System.Action<Vector2> pointerDownEvent;
    public System.Action<Vector2, Vector2> pointerReleased;

    Vector2 currentDragStartPositionWorld;
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
                pointerReleased(currentDragStartPositionWorld, pointerPositionWorld);
            }
            pointerDownEvent(pointerPositionWorld);
        }
    }
    bool PointerDown()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#elif UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            return true;
        }
        return false;
#endif
    }
    bool PointerPressed()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#elif UNITY_ANDROID
       if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }
        return false;
#endif
    }
    bool PointerReleased()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonUp(0);
#elif UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            var endedPhase = touch.phase == TouchPhase.Ended;
            if(endedPhase){
                Debug.Log("released!!");
                return true;
            }
        }
        return false;
#endif
    }
    Vector2 PointerPosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#elif UNITY_ANDROID
        var result = new Vector2();
        if(Input.touchCount == 1)
        {
            result = Input.GetTouch(0).position;
        }
        return result;
#endif
    }
}
