using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TypeOfTutorial
{
    Food, Kinine, Salt,
}

[Serializable]
public class typeTutorialDescriptionCombo
{
    public TypeOfTutorial type;
    public string description;
}
public class TutorialDescriptions : MonoBehaviour
{
    public static TutorialDescriptions instance;

    public typeTutorialDescriptionCombo[] descriptions;

    private void Start()
    {
        instance = this;
    }
}
