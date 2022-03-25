using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EntityAnimDataSetter : MonoBehaviour // all new
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public async void ResetMoveDataPlayer()
    {
        anim.SetBool("Crawl_Anim_Down_Left", false);
        anim.SetBool("CrawlBack_Anim_Up_Right", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isEatingBack", false);

        await Task.Delay(100);

        InputManager.instance.canRecieveInput = true; //new
    }

    public void ResetDataRotatingSlug()
    {
        anim.SetBool("isFliping_To_D", false);
        anim.SetBool("isFliping_To_U", false);
        anim.SetBool("isFliping_To_L", false);
        anim.SetBool("isFliping_To_R", false);
    }
    public void ResetMoveDataSlug()
    {
        anim.SetBool("isMoving", false);
        anim.SetBool("isMovingBack", false);
        anim.SetBool("isEating", false);
    }

    public void ResetPlayerHurt()
    {
        anim.SetBool("isHurting", false);
    }

    public void SetMoveData()
    {
        InputManager.instance.canRecieveInput = false; //new
    }
    public async void ResetRetractData()
    {
        anim.SetBool("isRetracting", false);

        await Task.Delay(1000);

        InputManager.instance.canRecieveInput = true; //new
    }
    public void SetRetractData()
    {
        InputManager.instance.canRecieveInput = false; //new
    }

    public void SetSlugSpawnActions()
    {
        Slug slug = GetComponent<Slug>();

        EntityManager.instance.SetEnemyTargetTiles(slug);

        if(slug.enemyPath != null)
        {
            if (slug.enemyPath.Count > 0)
            {
                slug.CalculateDirectionNextTile(slug.currentTile, slug.enemyPath[0], slug);
            }
        }
    }

    public void AfterKinineSaltSpawn()
    {
        anim.SetBool("Spawn", false);
    }
    public void DestroyAfterAnim()
    {
        Destroy(gameObject, 0.5f);
    }
}
