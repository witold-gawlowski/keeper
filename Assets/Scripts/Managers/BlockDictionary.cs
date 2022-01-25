using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockDictionary : GlobalManager<BlockDictionary>
{
    const int SAFETY_MAX_TRIAL_COUNT = 10;
    [SerializeField] private List<BlockSO> blocks;
    private void Awake()
    {
        base.Awake();
    }
    public List<BlockSO> GetStartingBlocks()
    {
        var result = new List<BlockSO>();
        foreach(var b in blocks)
        {
            if(b.initialCountInInventory > 0)
            {
                result.Add(b);
            }
        }
        return result;
    }
    public BlockSO GetBlockWithRarity()
    {
        var n = blocks.Count;
        BlockSO result = null;
        for(int i=0; i<SAFETY_MAX_TRIAL_COUNT; i++)
        {
            var randomIndex = Random.Range(0, n);
            var selectedBlock = blocks[randomIndex];
            var encounterFactor = selectedBlock.encounterProbabilityFactor;
            bool encounterSucessfull = Random.value < encounterFactor;
            if (encounterSucessfull)
            {
                result = selectedBlock;
                break;
            }
        }
        return result;
    }
}
