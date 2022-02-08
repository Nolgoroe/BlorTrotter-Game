using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IManageable
{
    public static ScoreManager instance;

    [SerializeField] private int levelScoreINT;
    [SerializeField] private float levelScoreFloat;

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








    [ContextMenu("Test add int")]
    public void callUpdateLevelScoreInt() //DELTE THIS AFTER SHOWING NATHAN
    {
        UpdateLevelScore(5);
    }

    [ContextMenu("Test add Float")]
    public void callUpdateLevelScoreFloat() //DELTE THIS AFTER SHOWING NATHAN
    {
        UpdateLevelScore(5.5f);
    }

    [ContextMenu("Test decrease int")]
    public void callUpdateLevelScoreIntDecrease() //DELTE THIS AFTER SHOWING NATHAN
    {
        UpdateLevelScore(-1);
    }

    [ContextMenu("Test decrease Float")]
    public void callUpdateLevelScoreDecrease() //DELTE THIS AFTER SHOWING NATHAN
    {
        UpdateLevelScore(-2.34f);
    }

}
