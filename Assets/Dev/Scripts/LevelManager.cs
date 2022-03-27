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

    public bool levelEnded;

    public List<Animator> kinineLocks;
    public List<Animator> saltLocks;

    public void initManager()
    {
        instance = this;
        kinineLocks = new List<Animator>();
        saltLocks = new List<Animator>();

        Debug.Log("success levels");
    }


    public async void LaunchLevel(int index)
    {
        levelEnded = false;

        ChooseLevel(index);
        ResetDataStartLevel();
        LoadLevel();

        UIManager.instance.DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.LoadingScreen, UIScreenTypes.GameScreen,UIScreenTypes.GameBG, UIScreenTypes.AsthericWoodsGameScreen });
        
        await Task.Delay(1000);
        CameraController.instance.CenterOnBlob();

    }

    public void ResetDataStartLevel()
    {
        CameraController.canControlCamera = true;

        ScoreManager.instance.currentLevelNumberOfMovesRemaining = currentLevel.maxNumberOfMoves;
        currentCooldownSummonEnemies = currentLevel.summonEnemyCooldown;
        kinineLocks.Clear();
        saltLocks.Clear();

        EntityManager.instance.SetPlayer(null);
        EntityManager.instance.slugSpawnTiles.Clear();
        EntityManager.instance.beetleSpawnTiles.Clear();
        EntityManager.instance.beetleTargetAndSpawnTiles.Clear();
        EntityManager.instance.beetleTargetTiles.Clear();

        EntityManager.instance.allEnemies.Clear();

        ScoreManager.instance.currentCollectedKnowledge = 0;
        ScoreManager.instance.currentCollectedFood = 0;

        UIManager.instance.saltPowerSprite.SetActive(false);
        UIManager.instance.kininePowerSprite.SetActive(false);
        UIManager.instance.SetInGameUIData();
        UIManager.instance.ResetWinScreenPositions();

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
        kinineLocks.Clear();
        saltLocks.Clear();

        GridManager.instance.allTilesInLevel.Clear();
        GridManager.instance.allEdgeTileInLevel.Clear();

        EntityManager.instance.SetPlayer(null);
        EntityManager.instance.slugSpawnTiles.Clear();
        EntityManager.instance.beetleSpawnTiles.Clear();
        EntityManager.instance.beetleTargetAndSpawnTiles.Clear();
        EntityManager.instance.beetleTargetTiles.Clear();

        EntityManager.instance.allEnemies.Clear();

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
    }


    public void CheckLoseLevel()
    {
        if(ScoreManager.instance.currentLevelNumberOfMovesRemaining <= 0 || EntityManager.instance.GetPlayer().entityAdjacentTiles.Count == 0)
        {
            InputManager.instance.canRecieveInput = false;
            CameraController.canControlCamera = false;
            levelEnded = true;

            UIManager.instance.LoseLevelAnimationSequence();
            Debug.Log("LOST LEVEL, OUT OF MOVES!");
        }
    }
    public void CheckWinLevel()
    {
        if(ScoreManager.instance.currentCollectedFood == currentLevel.amountOfFood)
        {
            InputManager.instance.canRecieveInput = false;
            CameraController.canControlCamera = false;
            levelEnded = true;

            UIManager.instance.WinLevelAnimationSequence();

            Debug.Log("WON LEVEL");
        }
    }

    public void DecreaseNumberOfMoves(int amount) 
    {
        UIManager.instance.decreaseMoveText.text = "-" + amount;
        UIManager.instance.decreaseMoveText.gameObject.SetActive(true);

        ScoreManager.instance.currentLevelNumberOfMovesRemaining -= amount;

        if (ScoreManager.instance.currentLevelNumberOfMovesRemaining < 0)
        {
            ScoreManager.instance.currentLevelNumberOfMovesRemaining = 0;
        }

        UIManager.instance.UpdateNumOfMoves(false);
    }
    public void AddMovesEat(int amount) 
    {
        UIManager.instance.addMoveText.text = "+" + amount;
        UIManager.instance.addMoveText.gameObject.SetActive(true);

        ScoreManager.instance.currentLevelNumberOfMovesRemaining += amount;
        UIManager.instance.UpdateNumOfMoves(true);
    }

    public async void ActivateKininePower()
    {
        UIManager.instance.kininePowerSprite.SetActive(true);

        await Task.Delay(1000);

        UnlcokKinine();
    }

    public async void ActivateSaltPower()
    {
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



    public void RestartLevel()
    {
        LaunchLevel(currentLevel.levelID);
    }
}
