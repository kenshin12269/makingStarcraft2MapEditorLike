using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    public enum TileSideType
    {
        One,
        Two,
        TwoDiagonal,
        ThreeSide,
        FourSide,
    }
    [CreateAssetMenu(menuName = "Create Tile definition")]
    public class TileDef : ScriptableObject
    {
        //Tile기준으로 11,1,5,7시방향 Height값 4개를 기준으로, 1개값이 더 높으면, oneSide, 두개가 높으면 twoSide임.
        public GameObject[] oneSide;
        public GameObject[] twoSide;
        public GameObject[] twoSideDiagonal;
        public GameObject[] threeSide;
        public GameObject[] fourSide;

        //Ramp
        public GameObject[] rampRightSideHigh;
        public GameObject[] rampRightSideLow;
        public GameObject[] rampLeftSideHigh;
        public GameObject[] rampLeftSideLow;
        public GameObject[] rampLeftSideHalf;
        public GameObject[] rampRightSideHalf;
        public GameObject[] rampBothSideHigh;
        public GameObject[] rampBothSideLow;
        public (GameObject prefab, float rot) GetTilePrefab(int baseHeight, int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            (TileSideType sideType, float rot) = GetTileType(baseHeight, topLeft, topRight, bottomLeft, bottomRight);
            GameObject go = null;
            switch (sideType)
            {
                case TileSideType.FourSide:
                    go = fourSide[0];
                    break;
                case TileSideType.ThreeSide:
                    go = threeSide[0];
                    break;
                case TileSideType.Two:
                    go = twoSide[0];
                    break;
                case TileSideType.TwoDiagonal:
                    go = twoSideDiagonal[0];
                    break;
                case TileSideType.One:
                    go = oneSide[0];
                    break;
            }
            return (go, rot);
        }
        public (TileSideType sideType, float rot) GetTileType(int baseHeight, int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            //높이가 전부 같다면..
            if (baseHeight <= topLeft &&
                baseHeight <= topRight &&
                baseHeight <= bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.FourSide, 0.0f);
            }

            //어느한쪽이 낮다면..
            if (baseHeight > topLeft &&
                baseHeight <= topRight &&
                baseHeight <= bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.ThreeSide, 0.0f);
            }
            if (baseHeight <= topLeft &&
                baseHeight > topRight &&
                baseHeight <= bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.ThreeSide, 90.0f);
            }
            if (baseHeight <= topLeft &&
                baseHeight <= topRight &&
                baseHeight <= bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.ThreeSide, 180.0f);
            }
            if (baseHeight <= topLeft &&
                baseHeight <= topRight &&
                baseHeight > bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.ThreeSide, 270.0f);
            }

            //두군데가 낮다면..
            if (baseHeight > topLeft &&
                baseHeight > topRight &&
                baseHeight <= bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.Two, 0.0f);
            }
            if (baseHeight <= topLeft &&
                baseHeight > topRight &&
                baseHeight <= bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.Two, 90.0f);
            }
            if (baseHeight <= topLeft &&
                baseHeight <= topRight &&
                baseHeight > bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.Two, 180.0f);
            }
            if (baseHeight > topLeft &&
                baseHeight <= topRight &&
                baseHeight > bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.Two, 270.0f);
            }

            //두군데가 낮은데 대각선이라면
            if (baseHeight > topLeft &&
                baseHeight <= topRight &&
                baseHeight <= bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.TwoDiagonal, 0.0f);
            }
            if (baseHeight <= topLeft &&//
                baseHeight > topRight &&
                baseHeight > bottomLeft &&//
                baseHeight <= bottomRight)
            {
                return (TileSideType.TwoDiagonal, 90.0f);
            }

            //세군데가 낮다면..
            if (baseHeight > topLeft &&
                baseHeight > topRight &&
                baseHeight > bottomLeft &&
                baseHeight <= bottomRight)
            {
                return (TileSideType.One, 0.0f);
            }
            if (baseHeight > topLeft &&
                baseHeight > topRight &&
                baseHeight <= bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.One, 90.0f);
            }
            if (baseHeight <= topLeft &&
                baseHeight > topRight &&
                baseHeight > bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.One, 180.0f);
            }
            if (baseHeight > topLeft &&
                baseHeight <= topRight &&
                baseHeight > bottomLeft &&
                baseHeight > bottomRight)
            {
                return (TileSideType.One, 270.0f);
            }
            throw new System.Exception(string.Format("BaseHeight={0} tl:{1} tr:{2} bl:{3} br:{4}", baseHeight, topLeft, topRight, bottomLeft, bottomRight));
        }
    }
}

