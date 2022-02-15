using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentOrbRewardItem : IRewardItem
{
    public int count;
    public ComponentOrbRewardItem(int count)
    {
        this.count = count;
    }
}
