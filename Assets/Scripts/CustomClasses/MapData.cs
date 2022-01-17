using System.Collections;
using System.Collections.Generic;
public class MapData
{
    public MapSO map;
    public List<IRewardItem> reward;
    public MapData(MapSO map, List<IRewardItem> reward) { this.map = map; this.reward = reward; }
}
