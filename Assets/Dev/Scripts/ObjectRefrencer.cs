using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRefrencer : MonoBehaviour, IManageable
{
    public static ObjectRefrencer instance;

    public GameObject enviroment;
    public GameObject levelMap;
    public GameObject enemies;
    public GameObject otherBlobs;

    public void initManager()
    {
        instance = this;
    }
}
