using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, IManageable
{
    public static CameraController instance;

    public static bool canControlCamera;

    private bool isZoom;
    private Touch touch;

    private Camera cam;

    Transform camPivot;

    public float panSpeed;   // speed to move the cam pivot
    public float rightBound;
    public float leftBound;
    public float topBound;
    public float bottomBound;

    public float startingDistanceFormBoard;

    public void initManager()
    {
        instance = this;

        cam = Camera.main;
        isZoom = false;
        canControlCamera = false;
        camPivot = transform;
    }

    private void LateUpdate()
    {
        if (Input.touchCount > 0 && canControlCamera)
        {
            touch = Input.GetTouch(0);

            if (Input.touchCount < 2) // if we touch the screen with only one finger
            {
                if (!isZoom)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {

                        Vector2 touchDeltaPos = touch.deltaPosition;
                        // we move the cam and if the cam translate to the rigt  it will looks like the game translate to the left
                        // to avoid this we use minus 
                        camPivot.Translate(-touchDeltaPos.x * panSpeed * Time.deltaTime, -touchDeltaPos.y * panSpeed * Time.deltaTime, 0);

                        camPivot.position = new Vector3(Mathf.Clamp(camPivot.position.x, -leftBound, rightBound), Mathf.Clamp(camPivot.position.y, -bottomBound, topBound), 45f);
                    }
                }
            }

            if (Input.touchCount == 2) //zooming and dezooming by using 2 fingers 
            {
                // zoom logic here
            }
        }
        else
        {
            isZoom = false;  // Fail safe - we want to make absoloutly sure that zoom is false after we lifted all fingers from the screen
        }
    }


    [ContextMenu("Center On Blob")]
    public void CenterOnBlob()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - startingDistanceFormBoard);

    }
}