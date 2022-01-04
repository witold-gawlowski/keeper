using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelGroup", menuName = "ScriptableObjects/LevelGroup")]
public class LevelGroupSO : ScriptableObject
{ 
    public int length;
    public string mName;
    public List<MapSO> maps;
}