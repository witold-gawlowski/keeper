using System.Collections;
using System.Collections.Generic;
public class MapData
{
    public MapSO map;
    public List<RewardItem> reward;
    public MapData(MapSO map, List<RewardItem> reward) { this.map = map; this.reward = reward; }
}
