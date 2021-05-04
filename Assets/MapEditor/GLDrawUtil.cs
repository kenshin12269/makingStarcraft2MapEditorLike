using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    public class GLDrawUtil
    {
        static Material lineMaterial;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }
        static bool didStartDraw = false;
        public static void Begin()
        {
            if (didStartDraw == true)
                throw new System.Exception("ERR");
            didStartDraw = true;
            CreateLineMaterial();
            CreateLineMaterial();
            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
        }
        public static void End()
        {
            if (didStartDraw == false)
                throw new System.Exception("ERR");
            didStartDraw = false;
            GL.End();
            GL.PopMatrix();
        }
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GL.Color(color);
            GL.Vertex(start);
            GL.Vertex(end);
        }
        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            GL.Color(color);
            GL.Vertex(new Vector3(start.x, 0, start.y));
            GL.Vertex(new Vector3(end.x, 0, end.y));
        }
        private const int CIRCLE_SEGMENT_CNT = 32;
        private static Vector2[] CIRCLE_VEC_ARR = new Vector2[CIRCLE_SEGMENT_CNT + 1];
        public static void DrawCircle(Vector2 pos, float radius, Color color)
        {
            var singleAngle = (2 * Mathf.PI) / CIRCLE_SEGMENT_CNT;
            for (int i = 0; i < CIRCLE_SEGMENT_CNT; i++)
            {
                var angle = singleAngle * i;
                var xPos = Mathf.Cos(angle);
                var yPos = Mathf.Sin(angle);
                var posVec = new Vector2(xPos, yPos) * radius;
                CIRCLE_VEC_ARR[i] = pos + posVec;
            }
            CIRCLE_VEC_ARR[CIRCLE_SEGMENT_CNT] = CIRCLE_VEC_ARR[0];

            for (int i = 0; i < CIRCLE_SEGMENT_CNT; i++)
            {
                DrawLine(CIRCLE_VEC_ARR[i], CIRCLE_VEC_ARR[i + 1], color);
            }
        }
        public static void _DrawAABB(Vector2 pos, Vector2 size, Color color)
        {
            var half = size / 2.0f;
            var bottomLeft = pos + new Vector2(-half.x, -half.y);
            var bottomRight = pos + new Vector2(+half.x, -half.y);
            var topLeft = pos + new Vector2(-half.x, +half.y);
            var topRight = pos + new Vector2(+half.x, +half.y);

            DrawLine(bottomLeft, bottomRight, color);
            DrawLine(bottomRight, topRight, color);
            DrawLine(topRight, topLeft, color);
            DrawLine(topLeft, bottomLeft, color);
        }
    }
}