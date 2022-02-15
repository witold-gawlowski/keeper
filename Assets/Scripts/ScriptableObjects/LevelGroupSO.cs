using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelGroup", menuName = "ScriptableObjects/LevelGroup")]
public class LevelGroupSO : ScriptableObject
{ 
    public int length;
    public string mName;
    public List<MapSO> maps;
    public List<int> mapsPerLevelDistribution = new List<int>(){ 0, 0, 2, 3, 2, 1};
    public List<int> rewardCountDistribution = new List<int>() { 1, 2, 4, 3, 2, 1 };
    public List<int> diggerDistribution;
    public List<int> componentOrbDistribution = new List<int>() { 8, 4, 2, 1 };
}