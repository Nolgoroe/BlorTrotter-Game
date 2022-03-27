using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;
using System;
using System.Linq;

public enum TypeOfTutorial
{
    Godray, Food, SelectableTiles, Salt, SaltTile, Kinine, kinineTile, Slug, SlugMucus, Beetle
}

public class TutorialManager : MonoBehaviour, IManageable
{
    public static TutorialManager instance;

    public bool showTutorials;

    public void initManager()
    {
        instance = this;

        showTutorials = true;
        Debug.Log("success tutorial");
    }

    public void DisplayTutorialText(string text)
    {
        UIManager.instance.TypeWriterWrite(text, UIManager.instance.tutorialTextObject);
    }

    public void CheckDisplayTutorialText(Tile tileToCheck)
    {
        EventManager.instance.ShootEvent(tileToCheck.GetComponent<TutorialObject>().GetActionEvent());

        UIManager.instance.DisplayTutorialExclimation();
    }

    public void CheckShowSpecificTutorial(Tile t)
    {
        if (t.isBeetleForTutorial)
        {
            DisplaySpecificText(TypeOfTutorial.Beetle);
            return;
        }

        if (t.isLightTile)
        {
            DisplaySpecificText(TypeOfTutorial.Godray);
            return;
        }

        if (t.isSlugBody)
        {
            DisplaySpecificText(TypeOfTutorial.Slug);
            return;
        }

        if (t.isEnemyGooPiece)
        {
            DisplaySpecificText(TypeOfTutorial.SlugMucus);
            return;
        }
    }

    public void DisplaySpecificText(TypeOfTutorial typwIN)
    {
        typeTutorialDescriptionCombo combo = TutorialDescriptions.instance.descriptions.Where(p => p.type == typwIN).SingleOrDefault();

        DisplayTutorialText(combo.description);
    }

}
