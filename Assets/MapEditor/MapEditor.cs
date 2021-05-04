using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapUtil
{
    public class MapEditor : MonoBehaviour
    {
        public Vector2Int mapSize = new Vector2Int(3, 3);
        [SerializeField] DebugTileMap tileMap;
        int[,] mapData;
        void Awake()
        {

        }
        void Update()
        {

        }
        void ReloadStaticData()
        {

        }
    }
}