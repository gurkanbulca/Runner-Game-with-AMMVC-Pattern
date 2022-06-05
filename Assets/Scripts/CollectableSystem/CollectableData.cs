using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CollectableData", fileName = "CollectableData")]
public class CollectableData : ScriptableObject
{
    public int currency;
    public int stackable;

    private void OnEnable()
    {
        Debug.Log("test");
    }
}
