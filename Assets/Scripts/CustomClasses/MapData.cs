using System.Collections;
using System.Collections.Generic;
public class MapData
{
    public MapSO map;
    public List<IRewardItem> reward;
    public float completionFraction;
    public bool Surrendered { get; set; }
    public MapData(MapSO map, List<IRewardItem> reward, float completionFraction)
    { 
        this.map = map;
        this.reward = reward;
        this.completionFraction = completionFraction;
    }

}
