using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static int GetSingleLayerMask (string name){
        int mask = LayerMask.GetMask(name);
        if(mask != 0)
        {
            return mask;
        }
        Debug.LogError("There is no layer \"" + name + "\"!");
        return 0;
    }
}
