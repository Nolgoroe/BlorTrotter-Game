using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool canControlCamera;

    bool isZoom;
    Touch touch;

    private Camera cam;

    public float panSpeed;
    public float rightBound;
    public float leftBound;
    public float topBound;
    public float bottomBound;

    private void Start()
    {
        cam = Camera.main;
        isZoom = false;
        canControlCamera = false;
    }

    private void LateUpdate()
    {
        if (Input.touchCount > 0 && canControlCamera)
        {
            touch = Input.GetTouch(0);

            if (Input.touchCount < 2)
            {
                if (!isZoom)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {

                        Vector2 touchDeltaPos = touch.deltaPosition;

                        cam.transform.Translate(-touchDeltaPos.x * panSpeed * Time.deltaTime, -touchDeltaPos.y * panSpeed * Time.deltaTime, 0);

                        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x, -leftBound, rightBound), Mathf.Clamp(cam.transform.position.y, -bottomBound, topBound), -7.56f);
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isZoom = false;
            }

        }



















        //DELTE THIS AFTER SHOWING NATHAN


        //public Transform target;
        //public float distance = 10f;

        //public float xSpeed = 250;
        //public float ySpeed = 120;

        ////public int yMinLimit = -20;
        ////public int yMaxLimit = 80;

        //private float x = 0;
        //private float y = 0;

        //float xsign = 1;

        //private void Start()
        //{
        //    Vector3 angles = transform.eulerAngles;
        //    x = angles.y;
        //    y = angles.x;

        //    Quaternion rotation = Quaternion.Euler(y, x, 0);
        //    //Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        //    transform.rotation = rotation;
        //    //transform.position = position;

        //}

        //private void LateUpdate()
        //{
        //    //get the rotationsigns

        //    Vector3 forward = transform.TransformDirection(Vector3.up);
        //    Vector3 forward2 = target.transform.TransformDirection(Vector3.up);

        //    if (Vector3.Dot(forward, forward2) < 0)
        //    {
        //        xsign = -1;
        //    }
        //    else
        //    {
        //        xsign = 1;
        //    }

        //    Debug.Log(xsign);

        //    foreach (Touch touch in Input.touches)
        //    {
        //        if (touch.phase == TouchPhase.Moved)
        //        {
        //            x += xsign * touch.deltaPosition.x * xSpeed * 0.02f;
        //            y -= touch.deltaPosition.y * ySpeed * 0.02f;



        //            Quaternion rotation = Quaternion.Euler(y, x, 0);
        //            Vector3 position = rotation * new Vector3(0, 1.17f, -distance) + target.position;

        //            transform.rotation = rotation;
        //            transform.position = position;
        //        }
        //    }
        //}
    }
}