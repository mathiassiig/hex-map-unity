using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace HexMap
{
    public class HexCell : MonoBehaviour
    {
        public TextMeshPro Text;
        public HexCoordinates HexCoords;
        public Color Color;
        public int Elevation
        {
            get
            {
                return _elevation;
            }
            set
            {
                _elevation = value;
                Vector3 position = transform.localPosition;
                position.y = value * HexMetrics.ElevationStep;
                transform.localPosition = position;
            }
        }

        private int _elevation;

        [SerializeField] private HexCell[] _neighbors;

        public HexCell GetNeighbor(HexDirection direction)
        {
            return _neighbors[(int)direction];
        }

        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            _neighbors[(int)direction] = cell;
            cell._neighbors[(int)direction.Opposite()] = this;
        }
    }
}