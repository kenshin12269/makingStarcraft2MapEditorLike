using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MapUtil
{
    public class MapEditor : MonoBehaviour
    {
        public Vector2Int mapSize = new Vector2Int(3, 3);
        [SerializeField] DebugTileMap debugTileMap;
        int[,] tileHeightMap;
        int[,] tileTypeMap;
        public Transform tile;
        void Awake()
        {
            debugTileMap.Size = mapSize;
            tileHeightMap = new int[mapSize.x, mapSize.y];
            tileTypeMap = new int[mapSize.x, mapSize.y];
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    tileHeightMap[i, j] = 2;
                }
            }
            debugTileMap.OnDragObservable().Subscribe(pos =>
            {
                Debug.LogFormat("Dragging={0}", pos);
                tile.localPosition = debugTileMap.GetPosFromIdx(pos);
            }).AddTo(this);
        }
        public void SetTileHeight(Vector2Int pos, int value, out int before)
        {
            before = tileHeightMap[pos.x, pos.y];
            tileHeightMap[pos.x, pos.y] = value;
        }
        public int IncTileHeight(Vector2Int pos, out int before)
        {
            before = tileHeightMap[pos.x, pos.y];
            tileHeightMap[pos.x, pos.y] = before + 1;
            return before + 1;
        }
        public int DecTileHeight(Vector2Int pos, out int before)
        {
            before = tileHeightMap[pos.x, pos.y];
            if (before > 0)
                tileHeightMap[pos.x, pos.y] = before - 1;
            return tileHeightMap[pos.x, pos.y];
        }
    }
}