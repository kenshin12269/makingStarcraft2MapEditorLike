using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    public class TileRenderer : MonoBehaviour
    {
        [SerializeField] TileDef tileDef;
        [SerializeField] Transform targetRoot;
        TileData mapData;
        TileAndWorldCoordConversion conversion;
        List<GameObject>[,] instMap;
        int[,] tmpHeightMap = new int[3, 3];
        public void Init(TileData mapData, TileAndWorldCoordConversion conversion)
        {
            this.mapData = mapData;
            this.conversion = conversion;
            this.instMap = new List<GameObject>[mapData.MapSize.x - 1, mapData.MapSize.y - 1];
            for (int i = 0; i < mapData.MapSize.x - 1; i++)
            {
                for (int j = 0; j < mapData.MapSize.y - 1; j++)
                {
                    instMap[i, j] = new List<GameObject>();
                }
            }
            for (int x = 0; x < mapData.MapSize.x - 1; x++)
            {
                for (int y = 0; y < mapData.MapSize.y - 1; y++)
                {
                    //UpdateIdx(new Vector2Int(x, y));
                    UpdateTileIdx(new Vector2Int(x, y));
                }
            }
        }
        bool IsInBoundary(Vector2Int idx)
        {
            if (idx.x < 0)
                return false;
            if (idx.y < 0)
                return false;
            if (idx.x >= mapData.MapSize.x - 1)
                return false;
            if (idx.y >= mapData.MapSize.y - 1)
                return false;
            return true;
        }
        public void UpdateHeightIdx(Vector2Int idx)
        {
            var topLeftIdx = idx + new Vector2Int(-1, 0);
            if (IsInBoundary(topLeftIdx) == true)
                UpdateTileIdx(topLeftIdx);
            var topRightIdx = idx + new Vector2Int(0, 0);
            if (IsInBoundary(topRightIdx) == true)
                UpdateTileIdx(topRightIdx);
            var bottomLeftIdx = idx + new Vector2Int(-1, -1);
            if (IsInBoundary(bottomLeftIdx) == true)
                UpdateTileIdx(bottomLeftIdx);
            var bottomRightIdx = idx + new Vector2Int(0, -1);
            if (IsInBoundary(bottomRightIdx) == true)
                UpdateTileIdx(bottomRightIdx);
        }
        public void UpdateTileIdx(Vector2Int idx)
        {
            for (int i = 0; i < instMap[idx.x, idx.y].Count; i++)
            {
                var inst = instMap[idx.x, idx.y][i];
                GameObject.Destroy(inst);
            }
            instMap[idx.x, idx.y].Clear();

            var topLeftIdx = idx + new Vector2Int(0, 1);
            var topRightIdx = idx + new Vector2Int(1, 1);
            var bottomLeftIdx = idx + new Vector2Int(0, 0);
            var bottomRightIdx = idx + new Vector2Int(1, 0);
            var topLeftH = mapData.TileHeightMap[topLeftIdx.x, topLeftIdx.y];
            var topRightH = mapData.TileHeightMap[topRightIdx.x, topRightIdx.y];
            var bottomLeftH = mapData.TileHeightMap[bottomLeftIdx.x, bottomLeftIdx.y];
            var bottomRightH = mapData.TileHeightMap[bottomRightIdx.x, bottomRightIdx.y];

            //일단 최대 높이를 체크한다..
            var biggestHeight = 0;
            if (topLeftH > biggestHeight)
                biggestHeight = topLeftH;
            if (topRightH > biggestHeight)
                biggestHeight = topRightH;
            if (bottomLeftH > biggestHeight)
                biggestHeight = bottomLeftH;
            if (bottomRightH > biggestHeight)
                biggestHeight = bottomRightH;

            //높이마다 체크한다 타일을..
            for (int i = 0; i <= biggestHeight; i++)
            {
                (GameObject prefab, float rot) = tileDef.GetTilePrefab(i, topLeftH, topRightH, bottomLeftH, bottomRightH);

                var created = GameObject.Instantiate(prefab);
                // created.layer = LayerMask.NameToLayer("Tile");
                // created.name = string.Format("{0}:{1}:{2}", idx.x, idx.y, curHeight);
                instMap[idx.x, idx.y].Add(created);
                created.transform.SetParent(targetRoot, false);
                created.transform.localRotation = Quaternion.Euler(0, rot, 0);
                var worldPos = conversion.GetNewCenterPosFromIdx(idx, i);
                // created.AddComponent<BoxCollider>();
                created.transform.localPosition = worldPos;
                created.transform.localScale = new Vector3(2, 1, 2);
            }
        }
    }
}



