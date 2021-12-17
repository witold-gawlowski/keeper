using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbePool : ObjectPool
{
    public override Poolable Spawn(Vector2 startPosition)
    {
        var result = base.Spawn(startPosition) as ProbeScript;
        return result;
    }
}
