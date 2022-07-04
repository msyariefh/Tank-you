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
    private Vector3 toRotateOriPos; // Original Position object want to rotate
    private Vector3 touch0Pos; // Touch position as comparison to others (0)

    // Angle
    private float angleOff; // Difference between 2 angle

    private float maxAngle;
    private bool isTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        col = GetComponent<Collider2D>();
        toRotateOriPos = col.transform.position; // (0,0) position for rotation
        maxAngle = GameManager.Instance.maximumShootAngle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If there are touch(es)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch object
            Vector3 touchPos = cam.ScreenToWorldPoint(touch.position); // convert touch pos to world point

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

                Vector3 deltaTouch0 = touch0Pos - toRotateOriPos; // Last touch pos from rotating obj
                Vector3 deltaToucht = touchPos - toRotateOriPos; // current touch pos from rotating obj

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
    }

    private bool IsPositive(float num)
    {
        return num >= 0;
    }
}
