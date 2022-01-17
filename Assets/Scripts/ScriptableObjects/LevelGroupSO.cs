using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelGroup", menuName = "ScriptableObjects/LevelGroup")]
public class LevelGroupSO : ScriptableObject
{ 
    [System.Serializable]
    public class BlockWithRarity
    {
        public int rarity;
        public BlockSO blockSO;
    }
    public int length;
    public string mName;
    public List<MapSO> maps;
    public List<BlockWithRarity> rewardBlocks;
    public List<int> diggerDistribution;
    public int mapsPerLevel = 2;
    public BlockSO GetBlock()
    {
        int loopCount = 0;
        while (loopCount < 100)
        {
            var randomIndex = Random.Range(0, rewardBlocks.Count);
            var candidate = rewardBlocks[randomIndex];
            var selector = Random.value;
            if (selector < Constants.rarityValues[candidate.rarity])
            {
                return candidate.blockSO;
            }
            loopCount++;
        }
        return null;
    }
}