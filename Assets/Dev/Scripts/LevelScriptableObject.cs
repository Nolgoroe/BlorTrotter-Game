using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelScriptableObject : ScriptableObject
{
    public int levelID;
    public int maxNumberOfMoves;
    public int summonEnemyCooldown;
    public int maxConcurrentSlugs;
}
