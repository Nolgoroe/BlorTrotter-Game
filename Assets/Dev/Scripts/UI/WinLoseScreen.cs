using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinLoseScreen : MonoBehaviour
{
    public TMP_Text winOrLoseText;
    public TMP_Text levelText;
    public TMP_Text nbrMovesRemainingText;
    public TMP_Text nbrFoodCollectedText;
    public TMP_Text nbrBlobCollectedText;
    public TMP_Text nbrBlobFragmentText;
    

    public void SetUpGameOver()
    {
       // winOrLoseText.text = " or "Defaite"
        levelText.text = "NIVEAU " + LevelManager.instance.currentLevel.ToString();
        

       

        // add in the score manager the moves, food, blob... then i can use ScoreManager.instance.foodCollected.ToString(); and same for the others

    }
   /* private void Start()
    {
        winOrLoseText.text = "Skuuuu";
    }
   */
}
