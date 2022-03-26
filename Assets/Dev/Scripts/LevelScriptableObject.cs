using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelScriptableObject : ScriptableObject
{
    [Header("Basic Data")]
    public int levelID;

    [Header("Level Specific Data")]
    public int maxNumberOfMoves;
    public int summonEnemyCooldown;
    public int maxConcurrentenemies;
    public int maxConcurrentSlugs;
    public int maxConcurrentBeetles;
    public int amountOfKnowledge;
    public int amountOfFood;
    public int amountToAddOnEatFood;
    public int amountToAddOnEatBlob;
    public bool hasEnemies;


    [Header("Level Display Data")]
    public Sprite levelMap;
    public Sprite levelObstacles;
    public Sprite levelBeetleData;
    public Sprite spriteLevelImage;
    public Sprite spriteLevelImageLocked;

    [Header("Level Win Data")]
    public int movesNeeded;
}
