using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour, IManageable
{
    public static LevelManager instance;

    public LevelScriptableObject currentLevel;

    [SerializeField] private int currentLevelNumberOfMoves;

    public int currentCooldownSummonEnemies;

    public bool hasKininePower, hasSaltPower;

    public List<Animator> kinineLocks;
    public List<Animator> saltLocks;

    public void initManager()
    {
        instance = this;
        kinineLocks = new List<Animator>();
        saltLocks = new List<Animator>();

        Debug.Log("success levels");
    }



    public void ChooseLevel(int levelNum) // we instantiate to have a clone of the scriptable object, because we absolutely don't want to change the data in the original one
    {
        currentLevel = Instantiate((LevelScriptableObject)Resources.Load("Scriptable Objects/Levels/Level " + levelNum)); /// NATHA PLEASE COMMENT THIS
    }


    public void LoadLevel() 
    {
        currentLevelNumberOfMoves = currentLevel.maxNumberOfMoves;
        currentCooldownSummonEnemies = currentLevel.summonEnemyCooldown;
        hasSaltPower = false;
        hasKininePower = false;
        kinineLocks.Clear();
        saltLocks.Clear();

        /// level editor generate level here          
        LevelEditor.instance.CallGenerateLevel();

        GridManager.instance.TilesIndexer();
        GridManager.instance.SetMapEdges();
        GridManager.instance.FillallEdgeTileInLevelList();
        GridManager.instance.LeaveGooOnTile(EntityManager.instance.GetPlayer().gooTiles[0]);

        //EntityManager.instance.SetEnemyTargetTiles();

        EntityManager.instance.SetPlayerTurn();

        InputManager.instance.canRecieveInput = true; //New
    }


    public void CheckEndLevel()
    {
        if(currentLevelNumberOfMoves <= 0)
        {
            // level lost logic here
            // send to UI Screen to manage screen display
            Debug.Log("LOST LEVEL, OUT OF MOVES!");
        }
    }

    public void DecreaseNumberOfMoves() 
    {
        currentLevelNumberOfMoves--;
        CheckEndLevel();
    }
    public void DecreaseSummonEnemyCooldown() 
    {
        currentCooldownSummonEnemies--;

        if(currentCooldownSummonEnemies <= 0)
        {
            if (!EntityManager.instance.CheckLimitOfEnemiesReached(currentCooldownSummonEnemies))
            {
                EntityManager.instance.SpawnEnemy();
            }

            currentCooldownSummonEnemies = currentLevel.summonEnemyCooldown;
        }
    }

    public async void ActivateKininePower()
    {
        hasKininePower = true;

        await Task.Delay(1000);

        UnlcokKinine();
    }

    public async void ActivateSaltPower()
    {
        hasSaltPower = true;

        await Task.Delay(1000);

        UnlockSalt();
    }

    private void UnlockSalt()
    {
        foreach (Animator anim in saltLocks)
        {
            anim.SetBool("Unlock", true);
            //anim.GetComponent<ConnecetdElement>().connectedElement.GetComponent<Tile>().isLocked = false;
            anim.GetComponent<ConnecetdElement>().connectedElement.GetComponent<Tile>().isFull = false;
        }

        saltLocks.Clear();

        GridManager.instance.FillallEdgeTileInLevelList();
    }

    private void UnlcokKinine()
    {
        foreach (Animator anim in kinineLocks)
        {
            anim.SetBool("Unlock", true);
            //anim.GetComponent<ConnecetdElement>().connectedElement.GetComponent<Tile>().isLocked = false;
            anim.GetComponent<ConnecetdElement>().connectedElement.GetComponent<Tile>().isFull = false;
        }

        kinineLocks.Clear();

        GridManager.instance.FillallEdgeTileInLevelList();
    }




    [ContextMenu("Choose Level")]
    public void callChooseLevel() //DELTE THIS AFTER SHOWING NATHAN
    {
        ChooseLevel(1);
    }

    [ContextMenu("Load Level")]
    public void CallLoadLevel() //DELTE THIS AFTER SHOWING NATHAN
    {
        LoadLevel();
    }

    [ContextMenu("Decrease Number Of Moves")]
    public void test() //DELTE THIS AFTER SHOWING NATHAN
    {
        currentLevelNumberOfMoves--;
        CheckEndLevel();
    }
}
