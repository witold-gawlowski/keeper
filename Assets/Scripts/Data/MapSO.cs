using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 2)]
public class MapSO : ScriptableObject
{
    public GameObject prefab;
    public float targetCompletionFraction;
    public int maxNumberOfHits;
    public int progressIndex;
}
