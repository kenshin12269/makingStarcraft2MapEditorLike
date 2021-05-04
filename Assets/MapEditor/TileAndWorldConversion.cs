using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    public class TileAndWorldCoordConversion
    {
        public float TileWorldScale => tileWorldScale;
        TileData tileData;
        float tileWorldScale;
        Vector3 offset;
        Vector3 totalHalf;
        Vector3 singleHalf;
        public TileAndWorldCoordConversion(TileData tileData, float tileWorldScale)
        {
            this.tileData = tileData;
            this.tileWorldScale = tileWorldScale;
            var totalSize = new Vector3(tileData.MapSize.x * tileWorldScale, 0, tileData.MapSize.y * tileWorldScale);
            singleHalf = new Vector3(tileWorldScale, 0, tileWorldScale) / 2.0f;

            totalHalf = totalSize / 2.0f;
            offset = -1.0f * totalHalf + singleHalf;
        }
        // public Vector2Int GetIdxFromPos(Vector3 pos)
        // {
        //     var tmp = pos + totalHalf;
        //     tmp = new Vector3(tmp.x / tileWorldScale, 0, tmp.z / tileWorldScale);
        //     return new Vector2Int(Mathf.FloorToInt(tmp.x), Mathf.FloorToInt(tmp.z));
        // }
        public Vector3 GetCenterPosFromIdx(Vector2Int idx, int height = 0)
        {
            var tmp = offset + new Vector3(idx.x * tileWorldScale, 0, idx.y * tileWorldScale);
            tmp += new Vector3(0, tileWorldScale * height);
            return tmp;
        }
        // public Vector3 GetBottomLeftPosFromIdx(Vector2Int idx, int height = 0)
        // {
        //     var tmp = offset + new Vector3(idx.x * tileWorldScale, 0, idx.y * tileWorldScale);
        //     tmp += new Vector3(0, tileWorldScale * height);
        //     tmp += new Vector3(-tileWorldScale, 0, -tileWorldScale);
        //     return tmp;
        // }
        // public Vector3 GetBottomRightPosFromIdx(Vector2Int idx, int height = 0)
        // {
        //     var tmp = offset + new Vector3(idx.x * tileWorldScale, 0, idx.y * tileWorldScale);
        //     tmp += new Vector3(0, tileWorldScale * height);
        //     tmp += new Vector3(+tileWorldScale, 0, -tileWorldScale);
        //     return tmp;
        // }
        // public Vector3 GetTopLeftPosFromIdx(Vector2Int idx, int height = 0)
        // {
        //     var tmp = offset + new Vector3(idx.x * tileWorldScale, 0, idx.y * tileWorldScale);
        //     tmp += new Vector3(0, tileWorldScale * height);
        //     tmp += new Vector3(-tileWorldScale, 0, +tileWorldScale);
        //     return tmp;
        // }
        // public Vector3 GetTopRightPosFromIdx(Vector2Int idx, int height = 0)
        // {
        //     var tmp = offset + new Vector3(idx.x * tileWorldScale, 0, idx.y * tileWorldScale);
        //     tmp += new Vector3(0, tileWorldScale * height);
        //     tmp += new Vector3(+tileWorldScale, 0, +tileWorldScale);
        //     return tmp;
        // }


        public Vector3 GetNewCenterPosFromIdx(Vector2Int idx, int height = 0)
        {
            var centerPos = GetCenterPosFromIdx(idx, height);
            centerPos += singleHalf;
            return centerPos;
        }
        public Vector2Int GetNearestIdxFromPos(Vector3 pos)
        {
            var tmp = pos + totalHalf;
            // tmp += singleHalf;
            tmp = new Vector3(tmp.x / tileWorldScale, 0, tmp.z / tileWorldScale);
            var result = new Vector2Int(Mathf.FloorToInt(tmp.x), Mathf.FloorToInt(tmp.z));
            result.x = Mathf.Clamp(result.x, 0, tileData.MapSize.x - 1);
            result.y = Mathf.Clamp(result.y, 0, tileData.MapSize.y - 1);
            return result;
        }
    }
}
