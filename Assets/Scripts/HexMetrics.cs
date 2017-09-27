using UnityEngine;
namespace HexMap
{
    public class HexMetrics
    {
        public const float OuterRadius = 10f;
        public const float InnerRadius = OuterRadius * 0.866025404f; // sqrt(3)/2
        public const float SolidFactor = 0.75f;
        public const float BlendFactor = 1f - SolidFactor;
        public const float ElevationStep = 5f;

        private static Vector3[] Corners = 
        {
            new Vector3(0f, 0f, OuterRadius),
            new Vector3(InnerRadius, 0f, 0.5f * OuterRadius),
            new Vector3(InnerRadius, 0f, -0.5f * OuterRadius),
            new Vector3(0f, 0f, -OuterRadius),
            new Vector3(-InnerRadius, 0f, -0.5f * OuterRadius),
            new Vector3(-InnerRadius, 0f, 0.5f * OuterRadius)
        };

        private static int Loop(int i)
        {
            return (int)Mathf.Repeat(i, Corners.Length);
        }

        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return Corners[(int)direction];
        }

        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return Corners[(int)Mathf.Repeat((int)direction + 1, Corners.Length)];
        }

        public static Vector3 GetFirstSolidCorner(HexDirection direction)
        {
            return Corners[(int)direction] * SolidFactor;
        }

        public static Vector3 GetSecondSolidCorner(HexDirection direction)
        {
            return Corners[Loop((int)direction+1)] * SolidFactor;
        }

        public static Vector3 GetBridge(HexDirection direction)
        {
            return (Corners[(int)direction] + Corners[Loop((int)direction + 1)]) * BlendFactor;
        }
    }
}