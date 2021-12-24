using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 2)]
public class LevelSO : ScriptableObject
{
    public GameObject prefab;
    public float targetCompletionFraction;
    public int maxNumberOfHits;
}
