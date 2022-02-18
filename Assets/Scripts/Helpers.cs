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
    public static Color GetDarkenedColor(Color c, float factor)
    {
        return new Color(c.r * factor, c.g * factor, c.b * factor, c.a);
    }
    public static ContactFilter2D GetSingleLayerMaskContactFilter(string name)
    {
        var result = new ContactFilter2D();
        result.NoFilter();
        var layerMask = GetSingleLayerMask(name);
        result.SetLayerMask(layerMask);
        return result;
    }
    public static void ReplicateColliderToProbe(BlockScript sourceBlock, PolygonCollider2D targetCollider)
    {
        var collider = sourceBlock.GetCollider();
        targetCollider.pathCount = collider.pathCount;
        for (int i = 0; i < targetCollider.pathCount; i++)
        {
            targetCollider.SetPath(i, collider.GetPath(i));
        }
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
    public static int GetIntFromDistribution(List<int> d)
    {
        int n = d.Count;
        int total = 0;
        foreach(var val in d)
        {
            total += val;
        }
        int selector = Random.Range(0, total);
        int result = 0;
        for(; result < n; result++)
        {
            selector -= d[result];
            if(selector < 0)
            {
                break;
            }
        }
        return result;
    }
    public static float Normal(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * u2);
        float randNormal =
                     mean + stdDev * randStdNormal;
        return randNormal;
    }
}
