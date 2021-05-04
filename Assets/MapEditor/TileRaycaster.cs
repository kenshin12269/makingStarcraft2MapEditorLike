using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MapUtil
{
    public struct TileCastInfo
    {
        public bool isHit;
        public Vector2Int idx;
        public int height;
    }
    public class TileRaycaster : MonoBehaviour
    {
        [SerializeField] Camera srcCamera;
        [SerializeField] LayerMask tileMask;
        public TileData tileData;
        public TileAndWorldCoordConversion tileAndWorldConversion;
        public TileCastInfo TileCastHeight(Vector3 mousePosition, int height)
        {
            var ray = srcCamera.ScreenPointToRay(mousePosition);
            // RaycastHit hitInfo = default;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 1, 0) * height * tileAndWorldConversion.TileWorldScale);
            bool isHit = plane.Raycast(ray, out float dist);

            if (isHit == false)
            {
                return default;
            }
            var hitPos = ray.GetPoint(dist);
            var tileIdx = tileAndWorldConversion.GetNearestIdxFromPos(hitPos);
            var tileHeight = tileData.TileHeightMap[tileIdx.x, tileIdx.y];

            // Debug.LogFormat("Raycast success mousePos={0} hitPos={1} tileIdx={2}", mousePosition, hitPos, tileIdx);
            var tileInfo = new TileCastInfo
            {
                isHit = true,
                idx = new Vector2Int(tileIdx.x, tileIdx.y),
                height = tileHeight,
            };
            return tileInfo;
        }
        public TileCastInfo TileCast(Vector3 mousePosition)
        {
            var ray = srcCamera.ScreenPointToRay(mousePosition);
            for (int i = 10; i >= 0; i--)
            {
                // RaycastHit hitInfo = default;
                Plane plane = new Plane(Vector3.up, new Vector3(0, 1, 0) * i * tileAndWorldConversion.TileWorldScale);
                bool isHit = plane.Raycast(ray, out float dist);

                if (isHit == false)
                {
                    continue;
                }
                var hitPos = ray.GetPoint(dist);
                var tileIdx = tileAndWorldConversion.GetNearestIdxFromPos(hitPos);
                var curTileHeight = tileData.TileHeightMap[tileIdx.x, tileIdx.y];
                if (curTileHeight < i)
                    continue;
                // Debug.LogFormat("Raycast success mousePos={0} hitPos={1} tileIdx={2}", mousePosition, hitPos, tileIdx);
                var tileInfo = new TileCastInfo
                {
                    isHit = true,
                    idx = new Vector2Int(tileIdx.x, tileIdx.y),
                    height = curTileHeight,
                };
                return tileInfo;
            }
            return default;
        }
        // public TileCastInfo TileCast(Vector3 mousePosition)
        // {
        //     var ray = srcCamera.ScreenPointToRay(mousePosition);
        //     RaycastHit hitInfo = default;
        //     var isHit = Physics.Raycast(ray, out hitInfo, float.MaxValue, tileMask);
        //     if (isHit == false)
        //     {
        //         return default;
        //     }
        //     var hitPos = hitInfo.point;
        //     string[] splitted = hitInfo.transform.name.Split(':');
        //     int x = System.Convert.ToInt32(splitted[0]);
        //     int y = System.Convert.ToInt32(splitted[1]);
        //     int height = System.Convert.ToInt32(splitted[2]);
        //     var tileInfo = new TileCastInfo
        //     {
        //         isHit = true,
        //         idx = new Vector2Int(x, y),
        //         height = height,
        //     };
        //     return tileInfo;
        // }
    }
}