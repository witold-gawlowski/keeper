using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerRewardItem : IRewardItem
{
    public int count;
    public DiggerRewardItem(int count)
    {
        this.count = count;
    }
}
