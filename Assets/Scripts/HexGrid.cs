using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HexMap
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private int _width = 6;
        [SerializeField] private int _height = 6;
        [SerializeField] private HexCell _cellPrefab;
        [SerializeField] private HexMesh _mesh;
        public Color DefaultColor = Color.white;
        public Color TouchedColor = Color.magenta;

        public int Width => _width;
        public int Height => _height;
        public HexMesh Mesh => _mesh;
        public HexCell[] Cells => _cells;

        private HexCell[] _cells;

        void Awake()
        {
            _cells = new HexCell[_height * _width];

            for (int z = 0, i = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        }

        public void ColorCell(Vector3 position, Color color)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * _width + coordinates.Z / 2;
            HexCell cell = _cells[index];
            cell.Color = color;
            _mesh.Triangulate(_cells);
        }

        void Start()
        {
            _mesh.Triangulate(_cells);
        }

        void CreateCell(int x, int z, int i)
        {

            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.OuterRadius * 1.5f);

            HexCell cell = _cells[i] = Instantiate<HexCell>(_cellPrefab);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.HexCoords = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.Text.text = cell.HexCoords.ToStringOnSeparateLines();
            cell.Color = DefaultColor;
            SetUpNeighbors(cell, x, z, i);

        }

        private void SetUpNeighbors(HexCell cell, int x, int z, int i)
        {
            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, _cells[i - 1]);
            }
            if (z > 0)
            {
                if ((z & 1) == 0)
                {
                    cell.SetNeighbor(HexDirection.SE, _cells[i - _width]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, _cells[i - _width - 1]);
                    }
                }
                else
                {
                    cell.SetNeighbor(HexDirection.SW, _cells[i - _width]);
                    if (x < _width - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, _cells[i - _width + 1]);
                    }
                }
            }
        }
    }
}