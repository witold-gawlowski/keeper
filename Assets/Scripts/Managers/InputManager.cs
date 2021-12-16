using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static System.Action<Vector2, Collider2D> mouse0DownEvent;
    public static System.Action<Vector2, Collider2D> mouse0DownEventWithDPressed;
    public static System.Action<Vector2> mouse0PressedEvent;
    public static System.Action mouse0UpEvent;
    public static System.Action rPressedEvent;
    public static System.Action rDownEvent;
    public static System.Action rUpEvent;
    public static System.Action dPressed;

    bool rPressed;
    Vector3 mousePositionScreen;
    Camera mainCamera;
    int blockLayerMask;
    private void Start()
    {
        rPressed = false;
        mainCamera = Camera.main;
        blockLayerMask = Helpers.GetSingleLayerMask(Constants.blockLayer);
    }
    void Update()
    {
        mousePositionScreen = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePositionScreen);
            mouse0PressedEvent(mousePositionWorld);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D hit = Physics2D.OverlapPoint(mousePositionWorld, blockLayerMask);
                if (Input.GetKey(KeyCode.D))
                {
                    mouse0DownEventWithDPressed(mousePositionWorld, hit);
                }
                else
                {
                    mouse0DownEvent(mousePositionWorld, hit);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouse0UpEvent();
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
    }
}
