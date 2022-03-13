using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IManageable  //singleton , only instantiate one time 
{
    public static InputManager instance;

    private Touch touch;

    public bool canRecieveInput; //new

    public void initManager()
    {
        instance = this;
        Debug.Log("success Input");
    }

    void Update()
    {
        if (Player.isPlayerTurn && canRecieveInput) 
        {
            if (Input.touchCount > 0)
            {
                touch = Input.touches[0];

                if (touch.phase == TouchPhase.Ended && !CameraController.isDragging)
                {
                    //if a finger touch the screen, cast a ray from the finger point, if the ray intersect with a tile, set the tile as currently selected
                    RaycastHit2D hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    hit = Physics2D.GetRayIntersection(ray);

                    if (hit)
                    {
                        if (hit.collider.CompareTag("Tile"))
                        {
                            Debug.Log("Detected Click on Tile object! " + hit.transform.name);

                            Tile t = hit.transform.GetComponent<Tile>();

                            if (!t.isMainPlayerBody && (!t.isFull || t.isFood || t.isKinine || t.isSalt)) //new
                            {
                                if (EntityManager.instance.GetPlayer().entityAdjacentTiles.Contains(t) || EntityManager.instance.GetPlayer().gooTiles.Contains(t))
                                {
                                    GridManager.instance.SetCurrentSelectedTile(hit.transform.GetComponent<Tile>());
                                }
                            }
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
}
