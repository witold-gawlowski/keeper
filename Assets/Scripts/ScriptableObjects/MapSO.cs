using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 2)]
public class MapSO : ScriptableObject
{
    public GameObject prefab;
    public float targetCompletionFraction = 0.5f;
    [Range(0, 0.9f)]
    public float minTargetCompletionFraction = 0.5f;
    [Range(0, 0.9f)]
    public float maxTargetCompletionFraction = 0.5f;
    public float difficulcy = 0.5f;
}
