using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IManageable
{
    public static InputManager instance;

    private Touch touch;

    public void initManager()
    {
        instance = this;
        Debug.Log("success Input");
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                hit = Physics2D.GetRayIntersection(ray);

                if (hit)
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        Debug.Log("Detected Click on Tile object! " + hit.transform.name);
                        GridManager.instance.SetCurrentSelectedTile(hit.transform.GetComponent<Tile>());
                    }
                }
                else
                {
                    Debug.Log("NOTHING");
                }

            }
        }
    }
}
