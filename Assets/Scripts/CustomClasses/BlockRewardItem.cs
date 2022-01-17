using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockRewardItem : IRewardItem
{
    public BlockSO block;
    public int count;
    public Sprite Sprite { get; private set; }
    public BlockRewardItem(BlockSO block, int count)
    {
        this.block = block;
        this.count = count;
        Sprite = block.PrefabBlockScript.GetSprite();
    }
}