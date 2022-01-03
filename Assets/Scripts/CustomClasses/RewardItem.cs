using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RewardItem
{
    public BlockSO block;
    public int count;
    public Sprite Sprite { get; private set; }
    public RewardItem(BlockSO block, int count)
    {
        this.block = block;
        this.count = count;
        Sprite = block.PrefabBlockScript.GetSprite();
    }
}