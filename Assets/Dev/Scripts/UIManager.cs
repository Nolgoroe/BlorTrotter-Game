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
    Options,
    Pause,
    Credits,
    LoadingScreen,
    StartGifScreen,
    NarratorBlobScreen,
    AsthericWoodsGameScreen,
    GameBG,
    MenuBG
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
    public GameObject[] loadingAnimations;

    [Header("Game Screen")]
    public TMP_Text knowledgeText;
    public TMP_Text foodText;
    public TMP_Text moveAmount;
    public GameObject saltPowerSprite;
    public GameObject kininePowerSprite;
    public Slider FoodSlider, KnowledgeSlider;
    public float SliderSpeed;
    public Image blobNarratorImage;

    [Header("Win Lose Screen")]
    public TMP_Text numberOfMovesText;
    public TMP_Text foodEatenText;
    public TMP_Text blobsCollectedText;
    public TMP_Text blobFragmentsText;
    public TMP_Text SuccessText;
    public TMP_Text defeatText;
    public TMP_Text successTextMainPanel;
    public TMP_Text defeatTextMainPanel;
    public TMP_Text levelNumText;
    public float screenFadeSpeedSuccessText;
    public RectTransform mainPanel;
    public Vector3 mainPanelTargetPos;
    public Vector3 mainPanelOriginalPos;
    public float mainPanelMoveTime;
    public RectTransform Logo;
    public Vector3 LogoTargetPos;
    public Vector3 LogoOriginalPos;
    public float LogoMoveTime;
    public int timeBetweenLogoAndMain;
    public Button restartButton, continueButton;
    public RectTransform topZone;
    public Vector3 topZoneOriginPos;
    public Vector3 topZoneTargetPos;
    public RectTransform bottomZone;
    public Vector3 bototmZoneOriginPos;
    public Vector3 bottomZoneTargetPos;
    public float zonesMoveTime;
    public RectTransform topLeftFoliage;
    public Vector3 topLeftFoliageOriginPos;
    public Vector3 topLeftFoliageTargetPos;
    public RectTransform topRightFoliage;
    public Vector3 topRightFoliageOriginPos;
    public Vector3 topRightFoliageTargetPos;
    public RectTransform bottomLeftoliage;
    public Vector3 bottomLeftFoliageOriginPos;
    public Vector3 bottomLeftFoliageTargetPos;
    public RectTransform bottomRightFoliage;
    public Vector3 bottomRightFoliageOriginPos;
    public Vector3 bottomRightFoliageTargetPos;
    public float foliageMoveTime;

    public GameObject[] stars;
    public int timeWaitBetweenStars;

    [Header("Tutorial")]
    public GameObject exclimationMark;
    public int timeExclimationMarkShown;

    [Header("Settings Screen")]
    public Image toggleOn;
    public Image toggleOff;
    public Image musicImage, soundImage;

    [Header("in game options screen")]
    public Image toggleOnOptions;
    public Image toggleOffOptions;
    public Image musicImageOptions, soundImageOptions;

    [Header("Settings Screen AND in game options screen")]
    public Sprite toggleOnSpriteOrange, toggleOnSpriteGrey, toggleOffSpriteOrange, toggleOffSpriteGrey;
    public Sprite musicSpriteOrange, musicSpriteGrey, soundSpriteOrange, soundSpriteGrey;

    [Header("Narrator Blob")]
    public Sprite[] blobSprites;
    public Image blobImage;
    public int currentBlobImageIndex;
    public Vector3 toScaleToBlobImage;
    public Vector3 toScaleMidBlobImage;
    public Vector3 originalScaleBlobImage;
    public float timeToScaleBlobImage;

    [Header("Feedbacks")]
    public Sprite[] slugMucus;
    //public GameObject RightArrowPrefab;
    //public GameObject LeftArrowPrefab;
    //public GameObject UpArrowPrefab;
    //public GameObject DownArrowPrefab;

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

        originalScaleBlobImage = blobImage.rectTransform.localScale;

        mainPanelOriginalPos = mainPanel.anchoredPosition;
        LogoOriginalPos = Logo.anchoredPosition;

        topZoneOriginPos = topZone.anchoredPosition;
        bototmZoneOriginPos = bottomZone.anchoredPosition;

        topLeftFoliageOriginPos = topLeftFoliage.anchoredPosition;
        topRightFoliageOriginPos = topRightFoliage.anchoredPosition;
        bottomLeftFoliageOriginPos = bottomLeftoliage.anchoredPosition;
        bottomRightFoliageOriginPos = bottomRightFoliage.anchoredPosition;

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
                ChooseAnimationToShowLoading();
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
    private void ChooseAnimationToShowLoading()
    {
        foreach (GameObject go in loadingAnimations)
        {
            go.SetActive(false);
        }

        int rand = UnityEngine.Random.Range(0, loadingAnimations.Length);

        loadingAnimations[rand].SetActive(true);

    }
    private async void LoadingTimeGif()
    {
        int randomTime = UnityEngine.Random.Range(3, 7);

        await Task.Delay(randomTime * 1000);

        DeactivateSpecificScreens(new UIScreenTypes[] { UIScreenTypes.StartGifScreen});

        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.MainMenu, UIScreenTypes.MenuBG });

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

    public void FadeImage(bool fadeIn, Image toFade, float speed)
    {
        if (fadeIn)
        {
            LeanTween.value(toFade.gameObject, 0, 1, speed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                Image sr = toFade.GetComponent<Image>();
                Color newColor = sr.color;
                newColor.a = val;
                sr.color = newColor;
            });
        }
        else
        {
            LeanTween.value(toFade.gameObject, 1, 0, speed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                Image sr = toFade.GetComponent<Image>();
                Color newColor = sr.color;
                newColor.a = val;
                sr.color = newColor;
            });
        }
    }

    public void FadeText(bool fadeIn, TMP_Text toFade, float speed)
    {
        if (fadeIn)
        {
            LeanTween.value(toFade.gameObject, 0, 1, speed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                TMP_Text text = toFade.GetComponent<TMP_Text>();
                Color newColor = text.color;
                newColor.a = val;
                text.color = newColor;
            });
        }
        else
        {
            LeanTween.value(toFade.gameObject, 1, 0, speed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                TMP_Text text = toFade.GetComponent<TMP_Text>();
                Color newColor = text.color;
                newColor.a = val;
                text.color = newColor;
            });
        }
    }
    public void FadeTextCanvasGroup(bool fadeIn, CanvasGroup toFade, float speed)
    {
        if (fadeIn)
        {
            LeanTween.value(toFade.gameObject, 0, 1, speed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                toFade.alpha = val;
            });
        }
        else
        {
            LeanTween.value(toFade.gameObject, 1, 0, speed).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                toFade.alpha = val;
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

        CheckEnableTouchInputs();
    }

    private void CheckEnableTouchInputs()
    {
        if (screenTypeToObject[UIScreenTypes.GameScreen].activeInHierarchy)
        {
            InputManager.instance.canRecieveInput = true;
        }
    }

    public void OpenPauseScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.Pause, UIScreenTypes.GameBG, UIScreenTypes.AsthericWoodsGameScreen, UIScreenTypes.GameBG });

        InputManager.instance.canRecieveInput = false;
        CameraController.canControlCamera = false;
    }
    public void ClosePauseScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.GameScreen, UIScreenTypes.GameBG, UIScreenTypes.AsthericWoodsGameScreen });

        InputManager.instance.canRecieveInput = true;
        CameraController.canControlCamera = true;
    }
    public void OpenSettingsScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.Options, UIScreenTypes.MenuBG });
    }
    public void OpenCreditsScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.Credits, UIScreenTypes.MenuBG });
    }
    public void OpenWikiScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.WikiBlob, UIScreenTypes.MenuBG});
    }
    public void BackToBaseWiki(GameObject toDeactivate)
    {
        toDeactivate.SetActive(false);
        wikiBaseScreen.SetActive(true);
    }
    public void OpenLevelSelectionScreen()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.LevelSelection, UIScreenTypes.MenuBG});

        currentLevelDisplayedID = 0;

        LevelSelectionScreenLogic();
    }
    public void ReturnToMainMenu()
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.MainMenu, UIScreenTypes.MenuBG });
    }
    public void ReturnToMainMenuFromGame()
    {
        LevelManager.instance.DestroyLevel();
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.MainMenu, UIScreenTypes.MenuBG });
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
        int lastLevelPlayed = SaveLoadManager.instance.saveLoadDataObject.maxLevelReached;

        /// get last played level here...


        /// if there is no last levelplayed, start from level 0.
        
        LevelManager.instance.LaunchLevel(lastLevelPlayed + 1);
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

        restartButton.interactable = false;
        continueButton.interactable = false;

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

    public void UpdateNumOfMoves(bool add)
    {
        moveAmount.text = ScoreManager.instance.currentLevelNumberOfMovesRemaining.ToString();

        if (add)
        {
            moveAmount.GetComponent<Animator>().SetBool("Effect Now", true);
        }
    }

    private void LevelSelectionScreenLogic()
    {
        levelNameText.text = LevelManager.instance.allLevels[currentLevelDisplayedID].name;

        LevelSavedData data = SaveLoadManager.instance.saveLoadDataObject.levelsSaved.Where(p => p.levelID == currentLevelDisplayedID).SingleOrDefault();

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
            levelImage.sprite = LevelManager.instance.allLevels[currentLevelDisplayedID].spriteLevelImageLocked;

            CustomChildFitter CCF = levelButton.GetComponent<CustomChildFitter>();
            CCF.fitChild = false;
        }
        else
        {
            levelButton.GetComponent<Image>().sprite = unlockedButtonSprite;
            levelButton.interactable = true;
            lockImage.SetActive(false);
            starsParent.SetActive(true);
            levelImage.sprite = LevelManager.instance.allLevels[currentLevelDisplayedID].spriteLevelImage;

            CustomChildFitter CCF = levelButton.GetComponent<CustomChildFitter>();
            CCF.fitChild = true;

            for (int i = 0; i < data.AmountOfStars; i++)
            {
                starsInLevelSelection[i].SetActive(true);
            }
        }
    }


    public async void DisplayTutorialExclimation()
    {
        exclimationMark.SetActive(true);

        await Task.Delay(timeExclimationMarkShown);

        exclimationMark.SetActive(false);
    }



    public void OpenBlobNarratorScreen()
    {
        DisplaySpecificScreensNoDeactivate(new UIScreenTypes[] { UIScreenTypes.NarratorBlobScreen });

        //currentBlobImageIndex = 0;

        //ChangeBlobNarratorDisplay();

    }
    public void CloseBlobNarratorScreen()
    {
        DeactivateSpecificScreens(new UIScreenTypes[] { UIScreenTypes.NarratorBlobScreen });

        //currentBlobImageIndex = 0;

    }
    public void BlobNarratorArrowLeft()
    {
        currentBlobImageIndex--;

        if (currentBlobImageIndex < 0)
        {
            currentBlobImageIndex = blobSprites.Length - 1;
        }

        ChangeBlobNarratorDisplay();
    }
    public void BlobNarratorArrowRight()
    {
        currentBlobImageIndex++;

        if (currentBlobImageIndex == blobSprites.Length)
        {
            currentBlobImageIndex = 0;
        }

        ChangeBlobNarratorDisplay();
    }
    public void SetBlobNarratorSprite()
    {
        blobNarratorImage.sprite = blobSprites[currentBlobImageIndex];

        CloseBlobNarratorScreen();
    }

    private void ChangeBlobNarratorDisplay()
    {
        blobImage.sprite = blobSprites[currentBlobImageIndex];

        LeanTween.scale(blobImage.rectTransform, toScaleToBlobImage, timeToScaleBlobImage).setOnComplete(() => NextStepBlobResize());
    }

    private void NextStepBlobResize()
    {
        LeanTween.scale(blobImage.rectTransform, toScaleMidBlobImage, timeToScaleBlobImage).setOnComplete(() => ResetBlobImageScale());
    }
    private void ResetBlobImageScale()
    {
        LeanTween.scale(blobImage.rectTransform, originalScaleBlobImage, timeToScaleBlobImage);
    }

    public void FlipSeeTutorials(bool displayTutorials)
    {
        TutorialManager.instance.showTutorials = displayTutorials;

        if (displayTutorials)
        {
            toggleOn.sprite = toggleOnSpriteGrey;
            toggleOff.sprite = toggleOffSpriteOrange;

            toggleOnOptions.sprite = toggleOnSpriteGrey;
            toggleOffOptions.sprite = toggleOffSpriteOrange;
        }
        else
        {
            toggleOn.sprite = toggleOnSpriteOrange;
            toggleOff.sprite = toggleOffSpriteGrey;

            toggleOnOptions.sprite = toggleOnSpriteOrange;
            toggleOffOptions.sprite = toggleOffSpriteGrey;
        }
    }
    public void FlipHearMusic()
    {
        if (SoundManager.instance.canHearMusic)
        {
            SoundManager.instance.canHearMusic = false;

            musicImage.sprite = musicSpriteGrey;
            musicImageOptions.sprite = musicSpriteGrey;
        }
        else
        {
            SoundManager.instance.canHearMusic = true;

            musicImage.sprite = musicSpriteOrange;
            musicImageOptions.sprite = musicSpriteOrange;
        }
    }
    public void FlipHearSounds()
    {
        if (SoundManager.instance.canHearSounds)
        {
            SoundManager.instance.canHearSounds = false;

            soundImage.sprite = soundSpriteGrey;
            soundImageOptions.sprite = soundSpriteGrey;
        }
        else
        {
            SoundManager.instance.canHearSounds = true;

            soundImage.sprite = soundSpriteOrange;
            soundImageOptions.sprite = soundSpriteOrange;
        }

    }


    public async void WinLevelAnimationSequence()
    {

        await EntityManager.instance.GetPlayer().PlayAnimation(AnimationType.Win);

        await MoveGameplayFoliage();
        await MoveGameplayPanelZones();

        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.WinLoseScreen, UIScreenTypes.GameBG });
        successTextMainPanel.gameObject.SetActive(true);
        defeatTextMainPanel.gameObject.SetActive(false);

        await FadeInSuccessText();
        await FadeOutSuccessText();

        SuccessText.alpha = 0;
        SetWinLoseScreenData();

        await animateWinScreen();


        await DisplayStarsForWinScreen();

        int score = ScoreManager.instance.calcualteEndLevelScore();
        SaveLoadManager.instance.SaveLevel(score);
        SaveLoadManager.instance.CheckMaxLevelReached(LevelManager.instance.currentLevel.levelID);
        SaveLoadManager.instance.SaveGameState();

        restartButton.interactable = true;
        continueButton.interactable = true;
    }
    public async void LoseLevelAnimationSequence()
    {
        await MoveGameplayFoliage();
        await MoveGameplayPanelZones();

        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.WinLoseScreen, UIScreenTypes.GameBG });
        successTextMainPanel.gameObject.SetActive(false);
        defeatTextMainPanel.gameObject.SetActive(true);

        await FadeInDefeatText();
        await FadeOutDefeatText();

        defeatText.alpha = 0;
        SetWinLoseScreenData();

        await animateWinScreen();


        await DisplayStarsForWinScreen();

        int score = ScoreManager.instance.calcualteEndLevelScore();
        SaveLoadManager.instance.SaveLevel(score);
        SaveLoadManager.instance.CheckMaxLevelReached(LevelManager.instance.currentLevel.levelID);
        SaveLoadManager.instance.SaveGameState();

        restartButton.interactable = true;
        continueButton.interactable = true;
    }

    public void SetWinLoseScreenData()
    {
        numberOfMovesText.text = ScoreManager.instance.currentLevelNumberOfMovesRemaining + "/" + LevelManager.instance.currentLevel.movesNeeded;
        foodEatenText.text = ScoreManager.instance.currentCollectedFood + "/" + LevelManager.instance.currentLevel.amountOfFood;
        blobsCollectedText.text = ScoreManager.instance.currentCollectedKnowledge + "/" + LevelManager.instance.currentLevel.amountOfKnowledge;
        levelNumText.text = "NIVEAU " + (LevelManager.instance.currentLevel.levelID + 1);

        int score = ScoreManager.instance.calcualteEndLevelScore();

        SaveLoadManager.instance.saveLoadDataObject.SaveLevel(score);

        //blobFragmentsText WHAT IS THIS
    }

    public async Task DisplayStarsForWinScreen()
    {
        int score = ScoreManager.instance.calcualteEndLevelScore();


        for (int i = 0; i < score; i++)
        {
            stars[i].SetActive(true);

            await Task.Delay(timeWaitBetweenStars);
        }

    }
    public async Task FadeInSuccessText()
    {
        FadeText(true, SuccessText, screenFadeSpeedSuccessText);

        float timeToWait = screenFadeSpeedSuccessText * 1000;

        await Task.Delay((int)timeToWait);
    }
    public async Task FadeOutSuccessText()
    {
        FadeText(false, SuccessText, screenFadeSpeedSuccessText);

        float timeToWait = screenFadeSpeedSuccessText * 1000;

        await Task.Delay((int)timeToWait);
    }
    public async Task FadeInDefeatText()
    {
        FadeText(true, defeatText, screenFadeSpeedSuccessText);

        float timeToWait = screenFadeSpeedSuccessText * 1000;

        await Task.Delay((int)timeToWait);
    }
    public async Task FadeOutDefeatText()
    {
        FadeText(false, defeatText, screenFadeSpeedSuccessText);

        float timeToWait = screenFadeSpeedSuccessText * 1000;

        await Task.Delay((int)timeToWait);
    }
    public async Task animateWinScreen()
    {
        LeanTween.move(Logo, LogoTargetPos, LogoMoveTime).setEaseOutBounce();

        await Task.Delay(timeBetweenLogoAndMain);

        LeanTween.move(mainPanel, mainPanelTargetPos, mainPanelMoveTime).setEaseOutBounce();

        await Task.Delay(1000);
    }

    public void ResetWinScreenPositions()
    {
        mainPanel.anchoredPosition = mainPanelOriginalPos;
        Logo.anchoredPosition = LogoOriginalPos;

        topZone.anchoredPosition = topZoneOriginPos;
        bottomZone.anchoredPosition = bototmZoneOriginPos;

        topLeftFoliage.anchoredPosition = topLeftFoliageOriginPos;
        topRightFoliage.anchoredPosition = topRightFoliageOriginPos;
        bottomLeftoliage.anchoredPosition = bottomLeftFoliageOriginPos;
        bottomRightFoliage.anchoredPosition = bottomRightFoliageOriginPos;

    }

    public async Task MoveGameplayPanelZones()
    {
        LeanTween.move(topZone, topZoneTargetPos, zonesMoveTime);
        LeanTween.move(bottomZone, bottomZoneTargetPos, zonesMoveTime);

        float timeToWait = zonesMoveTime * 1000;

        await Task.Delay((int)timeToWait);

    }
    public async Task MoveGameplayFoliage()
    {
        LeanTween.move(topLeftFoliage, topLeftFoliageTargetPos, foliageMoveTime);
        LeanTween.move(topRightFoliage, topRightFoliageTargetPos, foliageMoveTime);
        LeanTween.move(bottomLeftoliage, bottomLeftFoliageTargetPos, foliageMoveTime);
        LeanTween.move(bottomRightFoliage, bottomRightFoliageTargetPos, foliageMoveTime);


        float timeToWait = foliageMoveTime * 1000;

        await Task.Delay((int)timeToWait);

    }
}