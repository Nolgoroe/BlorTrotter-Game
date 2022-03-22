using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour, IManageable
{
    public static LevelManager instance;

    public LevelScriptableObject currentLevel;
    public LevelScriptableObject[] allLevels;

    public int currentCooldownSummonEnemies;
    //public int currentCollectedKnowledge, currentCollectedFood;

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


    public void LaunchLevel(int index)
    {

        ChooseLevel(index);
        ResetDataStartLevel();
        LoadLevel();
        CameraController.instance.CenterOnBlob();

        UIManager.instance.DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.LoadingScreen, UIScreenTypes.GameScreen });
    }

    public void ResetDataStartLevel()
    {
        CameraController.canControlCamera = true;

        ScoreManager.instance.currentLevelNumberOfMovesRemaining = currentLevel.maxNumberOfMoves;
        currentCooldownSummonEnemies = currentLevel.summonEnemyCooldown;
        hasSaltPower = false;
        hasKininePower = false;
        kinineLocks.Clear();
        saltLocks.Clear();

        EntityManager.instance.SetPlayer(null);

        ScoreManager.instance.currentCollectedKnowledge = 0;
        ScoreManager.instance.currentCollectedFood = 0;

        UIManager.instance.saltPowerSprite.SetActive(false);
        UIManager.instance.kininePowerSprite.SetActive(false);
        UIManager.instance.SetInGameUIData();

        for (int i = 0; i < ObjectRefrencer.instance.levelMap.transform.childCount; i++)
        {
            Destroy(ObjectRefrencer.instance.levelMap.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ObjectRefrencer.instance.enemies.transform.childCount; i++)
        {
            Destroy(ObjectRefrencer.instance.enemies.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ObjectRefrencer.instance.blobs.transform.childCount; i++)
        {
            Destroy(ObjectRefrencer.instance.blobs.transform.GetChild(i).gameObject);
        }


        GridManager.instance.allTilesInLevel.Clear();
        GridManager.instance.allEdgeTileInLevel.Clear();
        GridManager.instance.allEnemyGooTiles.Clear();
    }

    public void DestroyLevel()
    {
        InputManager.instance.canRecieveInput = false;
        CameraController.canControlCamera = false;

        ScoreManager.instance.currentLevelNumberOfMovesRemaining = 0;
        currentCooldownSummonEnemies = 0;
        hasSaltPower = false;
        hasKininePower = false;
        kinineLocks.Clear();
        saltLocks.Clear();

        GridManager.instance.allTilesInLevel.Clear();
        GridManager.instance.allEdgeTileInLevel.Clear();

        EntityManager.instance.SetPlayer(null);

        ScoreManager.instance.currentCollectedKnowledge = 0;
        ScoreManager.instance.currentCollectedFood = 0;

        UIManager.instance.saltPowerSprite.SetActive(false);
        UIManager.instance.kininePowerSprite.SetActive(false);

        for (int i = 0; i < ObjectRefrencer.instance.levelMap.transform.childCount; i++)
        {
            Destroy(ObjectRefrencer.instance.levelMap.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ObjectRefrencer.instance.enemies.transform.childCount; i++)
        {
            Destroy(ObjectRefrencer.instance.enemies.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ObjectRefrencer.instance.blobs.transform.childCount; i++)
        {
            Destroy(ObjectRefrencer.instance.blobs.transform.GetChild(i).gameObject);
        }
    }

    public void ChooseLevel(int levelIndex) // we instantiate to have a clone of the scriptable object, because we absolutely don't want to change the data in the original one
    {
        //currentLevel = Instantiate((LevelScriptableObject)Resources.Load("Scriptable Objects/Levels/Level " + levelNum)); /// NATHA PLEASE COMMENT THIS
        currentLevel = allLevels[levelIndex];
    }


    public void LoadLevel() 
    {

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


    public void CheckLoseLevel()
    {
        if(ScoreManager.instance.currentLevelNumberOfMovesRemaining <= 0)
        {
            // level lost logic here
            // send to UI Screen to manage screen display
            Debug.Log("LOST LEVEL, OUT OF MOVES!");
        }
    }
    public void CheckWinLevel()
    {
        if(ScoreManager.instance.currentCollectedFood == currentLevel.amountOfFood)
        {
            InputManager.instance.canRecieveInput = false;
            CameraController.canControlCamera = false;

            UIManager.instance.DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.WinLoseScreen});

            int score = ScoreManager.instance.calcualteEndLevelScore();

            LevelManagerSaveData.instance.SaveLevel(score);

            UIManager.instance.SetWinLoseScreenData();
            Debug.Log("WON LEVEL");
        }
    }

    public void DecreaseNumberOfMoves() 
    {
        ScoreManager.instance.currentLevelNumberOfMovesRemaining--;
        UIManager.instance.UpdateNumOfMoves();
    }
    public void AddMovesEat(int amount) 
    {
        ScoreManager.instance.currentLevelNumberOfMovesRemaining += amount;
        UIManager.instance.UpdateNumOfMoves();
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
        UIManager.instance.kininePowerSprite.SetActive(true);

        await Task.Delay(1000);

        UnlcokKinine();
    }

    public async void ActivateSaltPower()
    {
        hasSaltPower = true;
        UIManager.instance.saltPowerSprite.SetActive(true);

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




    public void MoveToNextLevel()
    {
        LaunchLevel(currentLevel.levelID + 1);
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
        ScoreManager.instance.currentLevelNumberOfMovesRemaining--;
        CheckLoseLevel();
    }
}
