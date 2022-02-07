using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IndexToGameobject
{
    public int tileIndex;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelScriptableObject : ScriptableObject
{
    public int levelID;
    public int maxNumberOfMoves;

    public List<IndexToGameobject> objectsOnGrid;
}
