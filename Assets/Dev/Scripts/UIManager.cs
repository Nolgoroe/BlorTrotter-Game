using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public enum UIScreenTypes
{
    screen1, //DELTE THIS AFTER SHOWING NATHAN
    screen2, //DELTE THIS AFTER SHOWING NATHAN
    screen3, //DELTE THIS AFTER SHOWING NATHAN
    screen4 //DELTE THIS AFTER SHOWING NATHAN
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

    public void initManager()
    {
        instance = this;

        screenTypeToObject = new Dictionary<UIScreenTypes, GameObject>();

        for (int i = 0; i < allGameScreens.Length; i++)
        {
            screenTypeToObject.Add((UIScreenTypes)i, allGameScreens[i]);
        }

        //DisableAllScreens();
        DisplaySpecificScreens(new UIScreenTypes[] {UIScreenTypes.screen1});

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
        }
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










    [ContextMenu("Test Display Screens")]
    public void Display() //DELTE THIS AFTER SHOWING NATHAN
    {
        DisplaySpecificScreens(new UIScreenTypes[] { UIScreenTypes.screen1, UIScreenTypes.screen2});
    }

    [ContextMenu("Test Display Screens no Deactivate")]
    public void Display2() //DELTE THIS AFTER SHOWING NATHAN
    {
        DisplaySpecificScreensNoDeactivate(new UIScreenTypes[] { UIScreenTypes.screen3});
    }

    [ContextMenu("Test Deactivate Specific Screens")]
    public void Display3() //DELTE THIS AFTER SHOWING NATHAN
    {
        DeactivateSpecificScreens(new UIScreenTypes[] { UIScreenTypes.screen3});
    }

    [ContextMenu("Test Deactivate ALL Screens")]
    public void Display4() //DELTE THIS AFTER SHOWING NATHAN
    {
        DisableAllScreens();
    }

    [ContextMenu("Test Fade In Screen")]
    public void CallFadeIn() //DELTE THIS AFTER SHOWING NATHAN
    {
        FadeImage(true, imageTest);
    }


    [ContextMenu("Test Fade Out Screen")]
    public void CallFadeOut() //DELTE THIS AFTER SHOWING NATHAN
    {
        FadeImage(false, imageTest);
    }

    [ContextMenu("Test Fade In TEXT")]
    public void CallFadeInText() //DELTE THIS AFTER SHOWING NATHAN
    {
        FadeText(true, testText);
    }


    [ContextMenu("Test Fade Out TEXT")]
    public void CallFadeOutText() //DELTE THIS AFTER SHOWING NATHAN
    {
        FadeText(false, testText);
    }
}
