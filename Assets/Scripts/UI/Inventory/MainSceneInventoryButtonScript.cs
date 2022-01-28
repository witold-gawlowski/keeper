using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInventoryButtonScript : InventoryItemUIScript
{
    public System.Action<BlockSO> blockSelectedEvent;
    private BlockSO correspondingBlock;
    public void OnTapped()
    {
        blockSelectedEvent(correspondingBlock);
    }
    public void Init(BlockSO b)
    {
        correspondingBlock = b;
    }
}
