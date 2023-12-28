using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotate : MonoBehaviour
{
    public GameObject objectToRotate; // Object we want to rotate
    private Camera cam; // Main Camera
    private Collider2D col; // Object's collider 2D

    // Position
    // All positions should be in world points not screen
    private Vector2 toRotateOriPos; // Original Position object want to rotate
    private Vector2 touch0Pos; // Touch position as comparison to others (0)

    // Angle
    private float angleOff; // Difference between 2 angle

    private float maxAngle;
    private bool isTouching = false;
    private Vector2 _mouseRef = Vector2.zero;
    private Vector3 _rotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
        toRotateOriPos = col.transform.position; // (0,0) position for rotation
        maxAngle = GameManager.Instance.maximumShootAngle;
    }

    private void OnMouseDown()
    {
#if UNITY_EDITOR
        _mouseRef = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
#endif
    }
    private void OnMouseDrag()
    {
#if UNITY_EDITOR
        if (MenuManager.Instance.state != MenuManager.State.Gameplay) return;
        var curr = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        if (curr == _mouseRef) return;
        if (curr.y < toRotateOriPos.y + .5f) { return; }
        var deltaTouch0 = _mouseRef - toRotateOriPos; // Last touch pos from rotating obj
        var deltaToucht = curr - toRotateOriPos; // current touch pos from rotating obj

        // Calculate the angle's diference
        angleOff = (Mathf.Atan2(deltaTouch0.y, deltaTouch0.x) -
            Mathf.Atan2(deltaToucht.y, deltaToucht.x)) * Mathf.Rad2Deg;
        angleOff = Mathf.Abs(angleOff); // Makes it always positive

        switch (curr.x - _mouseRef.x >= 0)
        {
            // touch from left to right
            case true:
                objectToRotate.transform.Rotate(new Vector3(0, 0, -angleOff));
                break;
            // touch from rigt to left
            case false:
                objectToRotate.transform.Rotate(new Vector3(0, 0, angleOff));
                break;
        }
        _mouseRef = curr;
#endif

    }
    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // If there are touch(es)
        if (Input.touchCount > 0)
        {
            if (MenuManager.Instance.state != MenuManager.State.Gameplay) return;
            var touch = Input.GetTouch(0); // Get the first touch object
            var touchPos = (Vector2)cam.ScreenToWorldPoint(touch.position); // convert touch pos to world point

            // User start to touch
            if ( touch.phase == TouchPhase.Began)
            {
                if (!(col == Physics2D.OverlapPoint(touchPos))) { return; }
                isTouching = true;
                touch0Pos = touchPos;
            }

            // If user move the touch
            if (touch.phase == TouchPhase.Moved)
            {
                if (!isTouching) { return; }
                if ( touchPos.y < toRotateOriPos.y + .5f) { return; }

                var deltaTouch0 = touch0Pos - toRotateOriPos; // Last touch pos from rotating obj
                var deltaToucht = touchPos - toRotateOriPos; // current touch pos from rotating obj

                // Calculate the angle's diference
                angleOff = (Mathf.Atan2(deltaTouch0.y, deltaTouch0.x) -
                    Mathf.Atan2(deltaToucht.y, deltaToucht.x)) * Mathf.Rad2Deg;
                angleOff = Mathf.Abs(angleOff); // Makes it always positive

                // Restirction to move on maximu (x) degrees
                //if (object_to_rotate.transform.rotation.z >= max_angle &&
                //    !IsPositive(touch_pos.x - touch0_pos.x)) { return; }
                //if (object_to_rotate.transform.rotation.z <= max_angle &&
                //    IsPositive(touch_pos.x - touch0_pos.x)) { return; }

                switch (touchPos.x - touch0Pos.x >= 0)
                {
                    // touch from left to right
                    case true:
                        objectToRotate.transform.Rotate(new Vector3(0, 0, -angleOff));
                        touch0Pos = touchPos;
                        break;
                    // touch from rigt to left
                    case false:
                        objectToRotate.transform.Rotate(new Vector3(0, 0, angleOff));
                        touch0Pos = touchPos;
                        break;

                }

            }
            if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }

        }
#endif
    }

    private bool IsPositive(float num)
    {
        return num >= 0;
    }
}
