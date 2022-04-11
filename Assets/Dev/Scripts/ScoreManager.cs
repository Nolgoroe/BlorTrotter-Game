using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IManageable   //singleton , only instantiate one time 
{
    public static ScoreManager instance;

    [SerializeField] private int levelScoreINT;
    [SerializeField] private float levelScoreFloat;


    public int currentLevelNumberOfMovesRemaining;
    public int currentCollectedFood;
    public int currentCollectedKnowledge;
    public int nbrBlobFragment;

    public void initManager()
    {
        instance = this;
        Debug.Log("success score");

        resetLevelScore();
    }


    public void UpdateLevelScore(int amout)
    {
        levelScoreINT += amout;
    }

    public void UpdateLevelScore(float amout)
    {
        levelScoreFloat += amout;
    }


    public void resetLevelScore()
    {
        levelScoreINT = 0;
        levelScoreFloat = 0;
    }



    public int calcualteEndLevelScore()/// can only be 1 - 3
    {
        int score = 0;

        if(currentLevelNumberOfMovesRemaining >= LevelManager.instance.currentLevel.movesNeeded)
        {
            score++;
        }

        if(currentCollectedFood >= LevelManager.instance.currentLevel.amountOfFood)
        {
            score++;
        }

        if(currentCollectedKnowledge >= LevelManager.instance.currentLevel.amountOfKnowledge)
        {
            score++;
        }

        return score;
    }
}
