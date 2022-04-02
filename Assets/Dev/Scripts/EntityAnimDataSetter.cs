using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EntityAnimDataSetter : MonoBehaviour // all new
{
    public Animator anim;

    public bool isFlippedEaten;

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

        await Task.Delay(500);

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
        //SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Blob_Moving_Spawning);

        InputManager.instance.canRecieveInput = true; //new

        //await Task.Delay(1000);

    }
    public void SetRetractData()
    {
        InputManager.instance.canRecieveInput = false; //new
        SoundManager.instance.PlaySound(SoundManager.instance.SFXAudioSource, Sounds.Blob_Moving_Spawning);
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

    public void CheckRotationAngle()
    {
        if (isFlippedEaten)
        {
            Vector3 rotation = new Vector3(0, 180, 0);
            transform.rotation = Quaternion.Euler(rotation);
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


    public void BeetleTeleportFromData()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.70f, transform.localPosition.z);

        Tile current = GetComponent<Beetle>().currentTile;

        current.isBeetleForTutorial = false;

    }

    public void ResetTeleportFromData()
    {
        Tile current = GetComponent<Beetle>().currentTile;

        transform.localPosition = new Vector3(transform.localPosition.x, current.transform.localPosition.y + (LevelEditor.instance.offsetY * 2), transform.localPosition.z);

        current.isBeetleForTutorial = true;
    }


    public void BeetleFlyData()
    {
        SoundManager.instance.PlaySoundFadeOut(SoundManager.instance.SFXAudioSource, Sounds.Beetle_Flight);
    }
    public void BeetleCarryFoodData()
    {
        Vector3 newV = transform.GetComponent<Beetle>().foodCarryDisplay.transform.localPosition;

        transform.GetComponent<Beetle>().foodCarryDisplay.transform.localPosition = new Vector3(newV.x, newV.y + 0.75f, newV.z);
    }
    public void BeetleCarryFoodDataDissappear()
    {
        transform.GetComponent<Beetle>().foodCarryDisplay.SetActive(false);
    }
    public void BeetleCarryFoodDataAppear()
    {
        if (transform.GetComponent<Beetle>().hasPickedFood)
        {
            transform.GetComponent<Beetle>().foodCarryDisplay.SetActive(true);
        }
    }

    public void SetTargetBeetleDisplay()
    {
        GridManager.instance.SetTargetTileBeetleOn(GetComponent<Beetle>().PublicTargetTile);
    }

    public void DeploySmokeLandBeetle()
    {
        transform.GetComponent<Beetle>().smokeVFX.SetActive(true);
    }
}
