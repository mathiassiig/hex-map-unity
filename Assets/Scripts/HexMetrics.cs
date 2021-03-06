﻿using UnityEngine;
namespace HexMap
{
    public enum HexEdgeType
    {
        Flat, Slope, Cliff
    }

    public class HexMetrics
    {
        public const float OuterRadius = 10f;
        public const float InnerRadius = OuterRadius * 0.866025404f; // sqrt(3)/2
        public const float SolidFactor = 0.75f;
        public const float BlendFactor = 1f - SolidFactor;
        public const float ElevationStep = 5f;
        public const int TerracesPerSlope = 2;
        public const int TerraceSteps = TerracesPerSlope * 2 + 1;
        public const float HorizontalTerraceStepSize = 1f / TerraceSteps;
        public const float VerticalTerraceStepSize = 1f / (TerracesPerSlope + 1);

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

        public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
        {
            float h = step * HexMetrics.HorizontalTerraceStepSize;
            a.x += (b.x - a.x) * h;
            a.z += (b.z - a.z) * h;
            float v = ((step + 1) / 2) * HexMetrics.VerticalTerraceStepSize;
            a.y += (b.y - a.y) * v;
            return a;
        }

        public static Color TerraceLerp(Color a, Color b, int step)
        {
            float h = step * HexMetrics.HorizontalTerraceStepSize;
            return Color.Lerp(a, b, h);
        }

        public static HexEdgeType GetEdgeType(int elevation1, int elevation2)
        {
            if (elevation1 == elevation2) return HexEdgeType.Flat;
            var delta = elevation2 - elevation1;
            return Mathf.Abs(delta) == 1 ? HexEdgeType.Slope : HexEdgeType.Cliff;
        }

    }
}