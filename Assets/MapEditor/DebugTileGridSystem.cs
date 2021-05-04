using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapUtil
{
    public class DebugTileGridSystem : MonoBehaviour, IListenToPostRenderCallback
    {
        [SerializeField] PostRenderCallbackReceiver postRenderCallbackReceiver;
        public TileData TileData;
        public TileAndWorldCoordConversion conversion;
        void OnEnable()
        {
            postRenderCallbackReceiver.Listen(this);
        }
        void OnDisable()
        {
            postRenderCallbackReceiver.Remove(this);
        }

        public void OnPostRender()
        {
            if (TileData == null || conversion == null)
                return;

            GLDrawUtil.Begin();
            //GLDrawUtil.DrawLine(new Vector2(-5, -5), new Vector2(5, 5), Color.red);
            int mapSizeX = TileData.MapSize.x;
            int mapSizeY = TileData.MapSize.y;
            for (int x = 0; x < mapSizeX; x++)
            {
                Color colorForDrawRight = Color.black;
                if (x % 5 == 0)
                    colorForDrawRight = Color.red;
                for (int y = 0; y < mapSizeY; y++)
                {
                    Color colorForDrawDown = Color.black;
                    if (y % 5 == 0)
                        colorForDrawDown = Color.red;

                    var tileHeight = TileData.TileHeightMap[x, y];
                    var startPos = conversion.GetCenterPosFromIdx(new Vector2Int(x, y), tileHeight);
                    if (x < mapSizeX - 1)
                    {
                        var rightTileHeight = TileData.TileHeightMap[x + 1, y];
                        var rightPos = conversion.GetCenterPosFromIdx(new Vector2Int(x + 1, y), rightTileHeight);
                        GLDrawUtil.DrawLine(startPos, rightPos, colorForDrawDown);
                        // Debug.LogFormat("DrawTile X={0} Y={1} {2}:{3}", x, y, startPos, rightPos);
                    }
                    if (y < mapSizeY - 1)
                    {
                        var bottomTileHeight = TileData.TileHeightMap[x, y + 1];
                        var bottomPos = conversion.GetCenterPosFromIdx(new Vector2Int(x, y + 1), bottomTileHeight);
                        GLDrawUtil.DrawLine(startPos, bottomPos, colorForDrawRight);
                        // Debug.LogFormat("DrawTile X={0} Y={1} {2}:{3}", x, y, startPos, bottomPos);
                    }
                }
            }
            GLDrawUtil.End();
        }
    }
}