using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MapUtil
{
    public class TileData
    {
        public int[,] TileHeightMap;
        public int[,] TileTypeMap;
        Vector2Int mapSize;
        public Vector2Int MapSize
        {
            get
            {
                return mapSize;
            }
            set
            {
                mapSize = value;
                TileHeightMap = new int[mapSize.x, mapSize.y];
                TileTypeMap = new int[mapSize.x, mapSize.y];
            }
        }
    }
}