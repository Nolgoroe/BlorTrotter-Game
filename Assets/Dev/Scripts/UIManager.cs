using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;
using System.Linq;

public enum UIScreenTypes
{
    MainMenu,
    GameScreen,
    WinLoseScreen,
    LevelSelection,
    WikiBlob,
    Settings,
    Pause,
    Credits,
    LoadingScreen,
    StartGifScreen
}

public class UIManager : MonoBehaviour, IManageable
{
    public static UIManager instance;

    [Header("Type Writer Settings")]
    [SerializeField]  private float typewriterSpeed;

    public TMP_Text tutorialTextObject;

    [Header("UI screen Settings")]
    [SerializeField] private GameObject[] allGameScreens;

    private Dictionary<UIScreenTypes, GameObject> screenTypeToObject;

    [Header("UI screen Effect Settings")]
    [SerializeField] private int screenFadeSpeed;

    public TMP_Text testText; //DELTE THIS AFTER SHOWING NATHAN
    public Image imageTest; //DELTE THIS AFTER SHOWING NATHAN

    [Header("UI Animations")]
    public GameObject lockPrefab;

    [Header("Wiki screen")]
    public GameObject wikiBaseScreen;

    [Header("Level Selection screen")]
    public int currentLevelDisplayedID;
    public TMP_Text levelNameText;
    public Image levelImage;
    public Button levelButton;
    public Sprite unlockedButtonSprite, lockedButtonSprite;
    public GameObject lockImage;
    public GameObject starsParent;
    public GameObject[] starsInLevelSelection;

    [Header("Loading Screen")]
    public GameObject tapToContinueText;
    public GameObject loadingText;

    [Header("Game Screen")]
    public TMP_Text knowledgeText;
    public TMP_Text foodText;
    public TMP_Text moveAmount;
    public GameObject saltPowerSprite;
    public GameObject kininePowerSprite;
    public Slider FoodSlider, KnowledgeSlider;
    public float SliderSpeed;


    [Header("Win Lose Screen")]
    public TMP_Text numberOfMovesText;
    public TMP_Text foodEatenText;
    public TMP_Text blobsCollectedText;
    public TMP_Text blobFragmentsText;

    public GameObject[] stars;
    public int timeWaitBetweenStars;

    [Header("Tutorial")]
    public GameObject exclimationMark;
    public int timeExclimationMarkShown;

    [Header("ETC")]
    public List<GameObject> tempScreens;

    public void initManager()
    {
        instance = this;

        screenTypeToObject = new Dictionary<UIScreenTypes, GameObject>();

        for (int i = 0; i < allGameScreens.Length; i++)
        {
            screenTypeToObject.Add((UIScreenTypes)i, allGameScreens[i]);
        }

        //DisableAllScreens();
        Debug.Log("success UI");
    }


    public async void TypeWriterWrite(string textToType, TMP_Text textObjectToChange)
    {
        textObjectToChange.text = "";

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textObjectToChange.text = textToType.Substring(0, charIndex);

            await Task.Yield();
        }

        textObjectToChange.text = textToType;
    }

    public void DisplaySpecificScreens(UIScreenTypes[] screens)
    {
        foreach (GameObject go in allGameScreens)
        {
            go.SetActive(false);
        }

        foreach (UIScreenTypes type in screens)
        {
            screenTypeToObject[type].SetActive(true);

            if(type == UIScreenTypes.LoadingScreen)
            {
                tapToContinueText.SetActive(false);
                loadingText.SetActive(true);
                LoadingTime();
            }

            if(type == UIScreenTypes.StartGifScreen)
            {
                LoadingTimeGif();
            }
        }        
    }

    private async void LoadingTime()
    {
        int randomTime = UnityEngine.Random.Range(2, 4);

        await Task.Delay(randomTime * 1000);

        tempScreens.Add(screenTypeToObject[UIScreenTypes.LoadingScreen]);
        tapToContinueText.SetActive(true);
        loadingText.SetActive(false);
    }
    private async void LoadingTimeGif()
    {
        int randomTime = UnityEngine.Random.Range(3, 7);

        await Task.Delay(randomTime * 1000);

        DeactivateSpecificScreens(new UIScreenTypes[] { UIScreenTypes.StartGifScreen});
    }
    public void DisplaySpecificScreensNoDeactivate(UIScreenTypes[] screens)
    {
        foreach (UIScreenTypes type in screens)
        {
            screenTypeToObject[type].SetActive(true);
        }
    }

    public void DeactivateSpecificScreens(UIScreenTypes[] screens)
    {
        foreach (UIScreenTypes type in screens)
        {
            screenTypeToObject[type].SetActive(false);
        }
    }

    public void DisableAllScreens()
    {
        foreach (GameObject go in allGameScreens)
        {
            go.SetActive(false);
        }
    }

    public void FadeImage(bool fadeIn, Image toFade)
    {
        if (fadeIn)
        {
            LeanTween.value(toFade.gameObject, 0, 1, screenFadeSpeed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                Image sr = toFade.GetComponent<Image>();
                Color newColor = sr.color;
                newColor.a = val;
                sr.color = newColor;
            });
        }
        else
        {
            LeanTween.value(toFade.gameObject, 1, 0, screenFadeSpeed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                Image sr = toFade.GetComponent<Image>();
                Color newColor = sr.color;
                newColor.a = val;
                sr.color = newColor;
            });
        }
    }

    public void FadeText(bool fadeIn, TMP_Text toFade)
    {
        if (fadeIn)
        {
            LeanTween.value(toFade.gameObject, 0, 1, screenFadeSpeed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                TMP_Text sr = toFade.GetComponent<TMP_Text>();
                Color newColor = sr.color;
                newColor.a = val;
                sr.color = newColor;
            });
        }
        else
        {
            LeanTween.value(toFade.gameObject, 1, 0, screenFadeSpeed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                TMP_Text sr = toFade.GetComponent<TMP_Text>();
                Color newColor = sr.color;
                newColor.a = val;
                sr.color = newColor;
            });
        }
    }

    public void CheckDisableTempScreens()
    {
        foreach (GameObject go in tempScreens)
        {
            go.SetActive(false);
        }

        tempScreens.Clear();
    }






    public void OpenPauseScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.Pause});

        InputManager.instance.canRecieveInput = false;
        CameraController.canControlCamera = false;
    }
    public void OpenSettingsScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.Settings});
    }
    public void OpenCreditsScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.Credits});
    }
    public void OpenWikiScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.WikiBlob});
    }
    public void BackToBaseWiki(GameObject toDeactivate)
    {
        toDeactivate.SetActive(false);
        wikiBaseScreen.SetActive(true);
    }
    public void OpenLevelSelectionScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.LevelSelection});

        currentLevelDisplayedID = 0;

        LevelSelectionScreenLogic();
    }
    public void ReturnToMainMenu()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.MainMenu });
    }
    public void ReturnToMainMenuFromGame()
    {
        LevelManager.instance.DestroyLevel();
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.MainMenu });
    }





    public void LevelSelectionArrowLeft()
    {
        currentLevelDisplayedID--;

        if(currentLevelDisplayedID < 0)
        {
            currentLevelDisplayedID = LevelManager.instance.allLevels.Length - 1;
        }

        LevelSelectionScreenLogic();
    }
    public void LevelSelectionArrowRight()
    {
        currentLevelDisplayedID++;
        
        if(currentLevelDisplayedID == LevelManager.instance.allLevels.Length)
        {
            currentLevelDisplayedID = 0;
        }

        LevelSelectionScreenLogic();
    }
    public void CallStartLevel()
    {
        LevelManager.instance.LaunchLevel(currentLevelDisplayedID);

        currentLevelDisplayedID = 0;
    }

    public void CallContinuePlaying()
    {
        int lastLevelPlayer = 0;

        /// get last played level here...


        /// if there is no last levelplayer, start from level 0.
        
        LevelManager.instance.LaunchLevel(lastLevelPlayer);
    }

    public void SetInGameUIData()
    {
        if(LevelManager.instance.currentLevel.amountOfKnowledge == 0)
        {
            KnowledgeSlider.value = 1;
        }
        else
        {
            UpdateKnowledgeAmount();
        }

        if (LevelManager.instance.currentLevel.amountOfFood == 0)
        {
            FoodSlider.value = 1;

        }
        else
        {
            UpdateFoodAmount();
        }

        moveAmount.text = LevelManager.instance.currentLevel.maxNumberOfMoves.ToString();

        foreach (GameObject go in stars)
        {
            go.SetActive(false);
        }

        numberOfMovesText.text = "0/" + LevelManager.instance.currentLevel.movesNeeded;
        foodEatenText.text = "0/" + LevelManager.instance.currentLevel.amountOfFood;
        blobsCollectedText.text = "0/" + LevelManager.instance.currentLevel.amountOfKnowledge;
    }

    public void UpdateKnowledgeAmount()
    {
        knowledgeText.text = ScoreManager.instance.currentCollectedKnowledge + "/" + LevelManager.instance.currentLevel.amountOfKnowledge.ToString();


        float currentSliderValue = KnowledgeSlider.value;

        float targetValue = (float)ScoreManager.instance.currentCollectedKnowledge / (float)LevelManager.instance.currentLevel.amountOfKnowledge;

        LeanTween.value(KnowledgeSlider.gameObject, currentSliderValue, targetValue, SliderSpeed).setOnUpdate((float val) =>
        {
            float newValue;

            newValue = val;
            KnowledgeSlider.value = newValue;
        });
    }
    public void UpdateFoodAmount()
    {
        foodText.text = ScoreManager.instance.currentCollectedFood + "/" + LevelManager.instance.currentLevel.amountOfFood.ToString();

        float currentSliderValue = FoodSlider.value;

        float targetValue = (float)ScoreManager.instance.currentCollectedFood / (float)LevelManager.instance.currentLevel.amountOfFood;

        LeanTween.value(FoodSlider.gameObject, currentSliderValue, targetValue, SliderSpeed).setOnUpdate((float val) =>
        {
            float newValue;

            newValue = val;
            FoodSlider.value = newValue;
        });

    }

    public void UpdateNumOfMoves()
    {
        moveAmount.text = ScoreManager.instance.currentLevelNumberOfMovesRemaining.ToString();
    }

    private void LevelSelectionScreenLogic()
    {
        levelNameText.text = LevelManager.instance.allLevels[currentLevelDisplayedID].name;
        levelImage.sprite = LevelManager.instance.allLevels[currentLevelDisplayedID].spriteLevelImage;

        LevelSavedData data = LevelManagerSaveData.instance.levelsSaved.Where(p => p.levelID == currentLevelDisplayedID).SingleOrDefault();

        for (int i = 0; i < 3; i++)
        {
            starsInLevelSelection[i].SetActive(false);
        }

        if (data == null)
        {
            levelButton.GetComponent<Image>().sprite = lockedButtonSprite;
            levelButton.interactable = false;
            lockImage.SetActive(true);
            starsParent.SetActive(false);
        }
        else
        {
            levelButton.GetComponent<Image>().sprite = unlockedButtonSprite;
            levelButton.interactable = true;
            lockImage.SetActive(false);
            starsParent.SetActive(true);

            for (int i = 0; i < data.AmountOfStars; i++)
            {
                starsInLevelSelection[i].SetActive(true);
            }
        }
    }



    public async void SetWinLoseScreenData()
    {
        numberOfMovesText.text = ScoreManager.instance.currentLevelNumberOfMovesRemaining + "/" + LevelManager.instance.currentLevel.movesNeeded;
        foodEatenText.text = ScoreManager.instance.currentCollectedFood + "/" + LevelManager.instance.currentLevel.amountOfFood;
        blobsCollectedText.text = ScoreManager.instance.currentCollectedKnowledge + "/" + LevelManager.instance.currentLevel.amountOfKnowledge;


        int score = ScoreManager.instance.calcualteEndLevelScore();

        for (int i = 0; i < score; i++)
        {
            stars[i].SetActive(true);

            await Task.Delay(timeWaitBetweenStars);
        }

        //blobFragmentsText WHAT IS THIS
    }




    public async void DisplayTutorialExclimation()
    {
        exclimationMark.SetActive(true);

        await Task.Delay(timeExclimationMarkShown);

        exclimationMark.SetActive(false);
    }

}

//[ContextMenu("Test Display Screens")]
//public void Display() //DELTE THIS AFTER SHOWING NATHAN
//{
//    DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.screen1, UIScreenTypes.screen2});
//}

//[ContextMenu("Test Display Screens no Deactivate")]
//public void Display2() //DELTE THIS AFTER SHOWING NATHAN
//{
//    DisplaySpecificScreensNoDeactivate(new UIScreenTypes[] { UIScreenTypes.screen3});
//}

//[ContextMenu("Test Deactivate Specific Screens")]
//public void Display3() //DELTE THIS AFTER SHOWING NATHAN
//{
//    DeactivateSpecificScreens(new UIScreenTypes[] { UIScreenTypes.screen3});
//}

//[ContextMenu("Test Deactivate ALL Screens")]
//public void Display4() //DELTE THIS AFTER SHOWING NATHAN
//{
//    DisableAllScreens();
//}

//[ContextMenu("Test Fade In Screen")]
//public void CallFadeIn() //DELTE THIS AFTER SHOWING NATHAN
//{
//    FadeImage(true, imageTest);
//}


//[ContextMenu("Test Fade Out Screen")]
//public void CallFadeOut() //DELTE THIS AFTER SHOWING NATHAN
//{
//    FadeImage(false, imageTest);
//}

//[ContextMenu("Test Fade In TEXT")]
//public void CallFadeInText() //DELTE THIS AFTER SHOWING NATHAN
//{
//    FadeText(true, testText);
//}


//[ContextMenu("Test Fade Out TEXT")]
//public void CallFadeOutText() //DELTE THIS AFTER SHOWING NATHAN
//{
//    FadeText(false, testText);
//}
