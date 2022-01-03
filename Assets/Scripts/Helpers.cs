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
    public static ContactFilter2D GetSingleLayerMaskContactFilter(string name)
    {
        var result = new ContactFilter2D();
        result.NoFilter();
        var layerMask = GetSingleLayerMask(name);
        result.SetLayerMask(layerMask);
        return result;
    }
    public static List<T> GetRandomSubset<T>(List<T> collection, int size)
    {
        var n = collection.Count;
        if(size >= n)
        {
            return new List<T>(collection);
        }
        var indexSet = new HashSet<int>();
        var result = new List<T>();
        while(indexSet.Count < size)
        {
            var randomIndex = Random.Range(0, n);
            if (!indexSet.Contains(randomIndex))
            {
                indexSet.Add(randomIndex);
                result.Add(collection[randomIndex]);
            }
        }
        return result;
    }
}
